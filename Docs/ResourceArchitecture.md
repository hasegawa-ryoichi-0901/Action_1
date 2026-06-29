# Resource Architecture

## 目的

SAM の `Context` / `State` の骨格を維持したまま、3D モデル、アニメーション、VFX、ステージ配置などの実装を下層へ分離する。

この設計では以下を守る。

- `Context` は遷移と起動順の制御に集中する
- 3D リソースの生成と破棄は `ResourceLoader` に集約する
- 実体データは `ResourceModel`、見た目や GameObject 管理は `ResourceView` に分離する
- `Context` ごとの差分は `Loader` の差し替えで吸収し、`Context` 自体を肥大化させない

## 全体構造

```text
                   +-------------------------+
                   |      ISamContext        |
                   |  (ContextType, Setup)   |
                   +------------+------------+
                                |
                                v
 +------------------------- SamBaseContext<TModel> ---------------------------+
 |  - Model lifecycle                                                         |
 |  - StateMachine init                                                       |
 |  - Transition helpers (internal/external)                                  |
 +------------+-------------+-----------------------------+------------------+
              |             |                             |
              v             v                             v
   SamViewContext<TModel,TView>        SamBaseModel              SamBaseState
   (View load/instantiate/show/hide)   (Setup/Dispose)           (OnEnter)
              |                                                    ^
              +-------------+-----------------+--------------------+
                            |                 |
                            v                 v
              SamViewState.*           Custom Context/States
              (Init/Load/Show/Hide etc.)
                            |
                            v
          +------------------------------------------------------+
          |           IResourceLoader / BaseResourceLoader        |
          |   - 3D resource lifecycle                            |
          |   - model/view bind                                  |
          |   - load/spawn/release                               |
          +----------------------+-------------------------------+
                                 |
               +-----------------+------------------+
               |                                    |
               v                                    v
       BaseResourceModel                     BaseResourceView
       - runtime data                        - GameObject refs
       - pool refs                           - Animator/VFX refs
       - spawn state                         - attach/detach
```

## レイヤー責務

### 1. Context レイヤー

- `TitleContext`
- `MainContext`
- `ReloadContext`

責務:

- `ContextType` を表す
- `StateMachine` を組み立てる
- 初期 State を起動する
- Scene / UI / Sequence の開始順を制御する
- 必要な `ResourceLoader` を保持する

非責務:

- 3D モデルの生成詳細
- Animator 制御の中身
- ObjectPool の個別管理
- ステージ配置ルール

### 2. State レイヤー

- `Init`
- `Load`
- `Show`
- `Hide`
- `Unload`
- `Active`

責務:

- Context のライフサイクルを段階化する
- `ResourceLoader` の呼び出しタイミングを揃える

### 3. ResourceLoader レイヤー

責務:

- リソースのロード
- GameObject の生成
- `Model` と `View` の接続
- 破棄とプール返却
- Context から見た 3D リソースの操作窓口

非責務:

- シーン全体の遷移判断
- 複数システムのゲーム進行判断

### 4. ResourceModel レイヤー

責務:

- 生成済み実体の状態保持
- Pool の参照保持
- スポーン済みフラグ
- ステージ ID やプレハブキー等の実行データ保持

### 5. ResourceView レイヤー

責務:

- `GameObject`
- `Transform`
- `Animator`
- `PlayableDirector`
- `ParticleSystem`
- `Renderer`

など Unity 実体への参照管理

## 基本インターフェース

```csharp
using Cysharp.Threading.Tasks;

public interface IResourceLoader {
    UniTask SetupAsync();
    UniTask LoadAsync();
    UniTask SpawnAsync();
    UniTask DespawnAsync();
    UniTask UnloadAsync();
}
```

`Context` に依存させたい場合も、`Context` そのものをジェネリック制約に入れず、必要最小限の Action を受ける。

```csharp
public interface IResourceLoader<in TAction> : IResourceLoader {
    UniTask BindAsync(TAction action);
}
```

## 基底クラス

```csharp
using Cysharp.Threading.Tasks;

public abstract class BaseResourceModel {
    public bool IsLoaded { get; protected set; }
    public bool IsSpawned { get; protected set; }

    public virtual UniTask SetupAsync() => UniTask.CompletedTask;
    public virtual UniTask DisposeAsync() => UniTask.CompletedTask;
}

public abstract class BaseResourceView {
    public UnityEngine.GameObject RootObject { get; protected set; }
    public UnityEngine.Transform RootTransform => RootObject != null ? RootObject.transform : null;

    public virtual UniTask BindAsync(UnityEngine.GameObject rootObject) {
        RootObject = rootObject;
        return UniTask.CompletedTask;
    }

    public virtual UniTask ReleaseAsync() {
        RootObject = null;
        return UniTask.CompletedTask;
    }
}

public abstract class BaseResourceLoader<TModel, TView> : IResourceLoader
    where TModel : BaseResourceModel, new()
    where TView : BaseResourceView, new() {
    protected TModel Model { get; private set; }
    protected TView View { get; private set; }

    public virtual async UniTask SetupAsync() {
        Model = new TModel();
        View = new TView();
        await Model.SetupAsync();
    }

    public abstract UniTask LoadAsync();
    public abstract UniTask SpawnAsync();
    public abstract UniTask DespawnAsync();

    public virtual async UniTask UnloadAsync() {
        if (View != null) {
            await View.ReleaseAsync();
        }
        if (Model != null) {
            await Model.DisposeAsync();
        }
    }
}
```

## Context への載せ方

`Context` ごとに `BaseResourceLoader<TModel, TView>` の子クラスを 1 つだけ作るのではなく、機能ごとに分けて合成する。

良い例:

- `MainContext`
  - `StageResourceLoader`
  - `PlayerResourceLoader`
  - `EnemyResourceLoader`
  - `BossResourceLoader`
  - `EffectResourceLoader`

避ける例:

- `MainResourceLoader`
  - ステージ
  - プレイヤー
  - 敵
  - ボス
  - VFX
  - カメラ
  - UI

後者は責務過多になりやすい。

## MainContext の具体形

```csharp
using AsyncFSM;
using Cysharp.Threading.Tasks;

public sealed class MainContext : MainContext<MainModel, MainView> {
    private StageResourceLoader _stageLoader;
    private PlayerResourceLoader _playerLoader;
    private EnemyResourceLoader _enemyLoader;

    public override string ViewPrefabPath => "Prefabs/Contexts/MainView";
    public override ContextType ContextType => ContextType.Main;

    public override TTransform GetViewParent<TTransform>() {
        return (TTransform)ContextManager.Instance.Canvas.transform;
    }

    protected override StateMachine RegisterStates(StateMachine sm) {
        sm.RegisterState(new MainState.Init(this));
        sm.RegisterState(new MainState.Load(this));
        sm.RegisterState(new MainState.Show(this));
        sm.RegisterState(new MainState.Active(this));
        sm.RegisterState(new MainState.Hide(this));
        sm.RegisterState(new MainState.Unload(this));
        return sm;
    }

    protected override async UniTask SetInitialStateAsync() {
        await TransitionExternalAsync<MainState.Init, MainState.Param>();
    }

    public override async UniTask SetupAsync(SamSetupParam param) {
        await base.SetupAsync(param);

        _stageLoader = new StageResourceLoader();
        _playerLoader = new PlayerResourceLoader();
        _enemyLoader = new EnemyResourceLoader();

        await _stageLoader.SetupAsync();
        await _playerLoader.SetupAsync();
        await _enemyLoader.SetupAsync();
    }

    public async UniTask LoadResourcesAsync() {
        await _stageLoader.LoadAsync();
        await _playerLoader.LoadAsync();
        await _enemyLoader.LoadAsync();
    }

    public async UniTask SpawnResourcesAsync() {
        await _stageLoader.SpawnAsync();
        await _playerLoader.SpawnAsync();
        await _enemyLoader.SpawnAsync();
    }

    public async UniTask ReleaseResourcesAsync() {
        await _enemyLoader.DespawnAsync();
        await _playerLoader.DespawnAsync();
        await _stageLoader.DespawnAsync();
    }

    public async UniTask UnloadResourcesAsync() {
        await _enemyLoader.UnloadAsync();
        await _playerLoader.UnloadAsync();
        await _stageLoader.UnloadAsync();
    }
}

public abstract class MainContext<TModel, TView> : SamViewContext<TModel, TView>
    where TModel : MainModel, new()
    where TView : MainView {
}
```

## State から ResourceLoader を呼ぶ例

```csharp
namespace MainState {
    public sealed class Param : SamViewStateParam {
    }

    public sealed class Load : SamViewState.Load<MainContext, MainModel, Param, MainView> {
        public Load(MainContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await base.OnEnter();
            await Context.LoadResourcesAsync();
            await Context.SpawnResourcesAsync();
        }
    }

    public sealed class Unload : SamViewState.Unload<MainContext, MainModel, Param, MainView> {
        public Unload(MainContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await Context.ReleaseResourcesAsync();
            await Context.UnloadResourcesAsync();
            await base.OnEnter();
        }
    }
}
```

## ResourceLoader の具体クラス例

### Stage

```csharp
public sealed class StageResourceModel : BaseResourceModel {
    public string PrefabPath { get; } = "Prefabs/Stage/StageRoot";
}

public sealed class StageResourceView : BaseResourceView {
}

public sealed class StageResourceLoader
    : BaseResourceLoader<StageResourceModel, StageResourceView> {
    public override async UniTask LoadAsync() {
        Model.IsLoaded = true;
        await UniTask.CompletedTask;
    }

    public override async UniTask SpawnAsync() {
        var prefab = UnityEngine.Resources.Load<UnityEngine.GameObject>(Model.PrefabPath);
        var instance = UnityEngine.Object.Instantiate(prefab);
        await View.BindAsync(instance);
        Model.IsSpawned = true;
    }

    public override async UniTask DespawnAsync() {
        if (View.RootObject != null) {
            UnityEngine.Object.Destroy(View.RootObject);
        }
        Model.IsSpawned = false;
        await View.ReleaseAsync();
    }
}
```

### Player

```csharp
public sealed class PlayerResourceModel : BaseResourceModel {
    public string PrefabPath { get; } = "Prefabs/Character/Player";
}

public sealed class PlayerResourceView : BaseResourceView {
    public UnityEngine.Animator Animator { get; private set; }

    public override async UniTask BindAsync(UnityEngine.GameObject rootObject) {
        await base.BindAsync(rootObject);
        Animator = rootObject.GetComponent<UnityEngine.Animator>();
    }

    public override async UniTask ReleaseAsync() {
        Animator = null;
        await base.ReleaseAsync();
    }
}

public sealed class PlayerResourceLoader
    : BaseResourceLoader<PlayerResourceModel, PlayerResourceView> {
    public override async UniTask LoadAsync() {
        Model.IsLoaded = true;
        await UniTask.CompletedTask;
    }

    public override async UniTask SpawnAsync() {
        var prefab = UnityEngine.Resources.Load<UnityEngine.GameObject>(Model.PrefabPath);
        var instance = UnityEngine.Object.Instantiate(prefab);
        await View.BindAsync(instance);
        Model.IsSpawned = true;
    }

    public override async UniTask DespawnAsync() {
        if (View.RootObject != null) {
            UnityEngine.Object.Destroy(View.RootObject);
        }
        Model.IsSpawned = false;
        await View.ReleaseAsync();
    }
}
```

## 推奨ディレクトリ

```text
Assets/Project/Scripts
├─ Contexts
│  ├─ Title
│  ├─ Main
│  └─ Reload
├─ Resources3D
│  ├─ Interfaces
│  │  └─ IResourceLoader.cs
│  ├─ Base
│  │  ├─ BaseResourceLoader.cs
│  │  ├─ BaseResourceModel.cs
│  │  └─ BaseResourceView.cs
│  ├─ Stage
│  │  ├─ StageResourceLoader.cs
│  │  ├─ StageResourceModel.cs
│  │  └─ StageResourceView.cs
│  ├─ Character
│  │  ├─ PlayerResourceLoader.cs
│  │  ├─ PlayerResourceModel.cs
│  │  └─ PlayerResourceView.cs
│  └─ Effects
└─ Utils
   └─ Sam
```

## 設計判断

### 採用すること

- `Context` は複数の `ResourceLoader` を合成する
- `ResourceLoader` は 3D リソース単位で分ける
- `BaseResourceLoader<TModel, TView>` は共通ライフサイクルだけ持つ
- `State` から `Context` を経由して `Loader` を呼ぶ

### 採用しないこと

- `Context<TLoader>` のように `Context` が 1 つの `Loader` 型へ強く依存する構造
- `MainResourceLoader` 1 クラスに全 3D 責務を詰め込む構造
- Scene 遷移ロジックを `ResourceLoader` に持たせる構造

## 最小導入順

1. `IResourceLoader`
2. `BaseResourceModel`
3. `BaseResourceView`
4. `BaseResourceLoader<TModel, TView>`
5. `StageResourceLoader`
6. `PlayerResourceLoader`
7. `MainContext` からの呼び出し
8. 必要に応じて `Enemy` / `Boss` / `Effect` へ展開

## 結論

この設計では、SAM の `Context` と `State` を上位オーケストレーションに保ちつつ、3D 実装を `ResourceLoader` 群へ分離できる。

重要なのは次の 2 点。

- `Context` ごとに巨大な Loader を 1 個ぶら下げない
- `Context` は Loader を合成して使う

この形なら、タイトル遷移、メインシーン、ボス戦、リトライのいずれでも保守性を保ちやすい。

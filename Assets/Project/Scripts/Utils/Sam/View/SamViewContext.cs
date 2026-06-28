using System.Data;

using Cysharp.Threading.Tasks;

using R3;
using R3.Triggers;

using UnityEngine;

public abstract class SamViewContext<TModel, TView> : SamBaseContext<TModel>, ISamViewAction, ISamViewContext<TView>
    where TModel : SamBaseModel, new()
    where TView : MonoBehaviour {
    public TView View { get; protected set; }

    /// <summary>
    /// View が破棄されるまで StateMachine.Update を継続するかどうか。
    /// </summary>
    public virtual bool CanInvokeUpdateWhileAliveView => true;
    public abstract string ViewPrefabPath { get; }
    public GameObject ViewPrefab { get; protected set; }
    public GameObject ViewObject { get; protected set; }

    /// <summary>
    /// 現在アタッチされている View を指定型として取得する。
    /// </summary>
    public T GetView<T>() where T : MonoBehaviour => View as T;

    /// <summary>
    /// 既存 View を破棄し、新たにプレハブをロードする準備処理。
    /// </summary>
    public virtual async UniTask PrepareViewAsync() {
        await UniTask.SwitchToMainThread();
        if (ViewObject) {
            GameObject.Destroy(ViewObject);
            ViewObject = null;
        }
        if (ViewPrefab) {
            ViewPrefab = null;
        }
        ViewPrefab = await LoadViewPrefabAsync();
        if (!ViewPrefab) {
            throw new DataException(ViewPrefabPath);
        }
    }

    /// <summary>
    /// Resources から View プレハブを読み込む。
    /// </summary>
    public virtual async UniTask<GameObject> LoadViewPrefabAsync() {
        return Resources.Load(ViewPrefabPath) as GameObject;
    }

    /// <summary>
    /// ロード済みプレハブから View をインスタンス化し親 Transform にぶら下げる。
    /// </summary>
    public virtual void InstantiateViewObject() {
        var saveActive = ViewPrefab.activeSelf;
        ViewPrefab.SetActive(false);
        ViewObject = Object.Instantiate(ViewPrefab, GetViewParent<Transform>(), false);
        ViewPrefab.SetActive(saveActive);
    }

    /// <summary>
    /// View を配置する親 Transform を取得する。
    /// </summary>
    public abstract TTransform GetViewParent<TTransform>() where TTransform : Transform;

    /// <summary>
    /// View インスタンス生成直後に追加セットアップを行う。
    /// </summary>
    public virtual async UniTask OnPostInstantiateAsync() {
        View = ViewObject.GetComponent<TView>();
        if (CanInvokeUpdateWhileAliveView) {
            View?.OnDestroyAsObservable().Subscribe(_ => {
                if (IsDisposed) {
                    return;
                }
                StateMachine.RunUpdate = false;
            })
            .AddTo(View);
        }
    }

    /// <summary>
    /// View をアクティブ化し表示する。
    /// </summary>
    public virtual async UniTask ShowAsync() {
        await UniTask.SwitchToMainThread();
        ViewObject.SetActive(true);
    }

    /// <summary>
    /// View を非表示にする。
    /// </summary>
    public virtual async UniTask HideAsync() {
        await UniTask.SwitchToMainThread();
        ViewObject.SetActive(false);
    }

    /// <summary>
    /// View とモデルを破棄し、基底破棄処理を呼び出す。
    /// </summary>
    public override async UniTask DisposeAsync() {
        if (ViewObject) {
            GameObject.Destroy(ViewObject);
        }
        ViewObject = null;
        View = null;
        await base.DisposeAsync();
    }

    /// <summary>
    /// View・プレハブ・モデルを完全に解放する。
    /// </summary>
    public virtual async UniTask UnloadAsync() {
        await UniTask.SwitchToMainThread();
        if (ViewObject) {
            GameObject.Destroy(ViewObject);
        }
        ViewObject = null;
        ViewPrefab = null;
        View = null;
        await Resources.UnloadUnusedAssets();
        if (Model != null) {
            await Model.DisposeAsync();
        }
    }
}

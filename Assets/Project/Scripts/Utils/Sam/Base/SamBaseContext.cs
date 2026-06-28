using System;

using AsyncFSM;

using Cysharp.Threading.Tasks;

using R3;

using Random = UnityEngine.Random;

/// <summary>
/// Lifecycle
/// 1. <see cref="SetupAsync"/>
/// 2. InitialState
/// </summary>
/// <typeparam name="TModel">dao</typeparam>
public abstract class SamBaseContext<TModel> : ISamContext, ISamAction
    where TModel : ISamModel, new() {
    /// <summary>
    /// このコンテキストを識別する区分。派生クラスで上書きする。
    /// </summary>
    public virtual ContextType ContextType => ContextType.None;
    public CompositeDisposable AsDisposable { get; } = new();

    // public T GetContext<T>() where T: ISamContext => this as T;
    public AsyncReactiveProperty<ContextType> OnSetupCompleted { get; } =
        new AsyncReactiveProperty<ContextType>(ContextType.None);

    /// <summary>
    /// コンテキストで利用するモデルを生成・初期化する。
    /// </summary>
    protected virtual async UniTask<TModel> CreateModelAsync() {
        var model = new TModel();
        await model.SetupAsync(ContextType);
        return model;
    }

    /// <summary>
    /// ステートマシンを生成し、利用するステートを登録する。
    /// </summary>
    protected virtual StateMachine CreateStateMachine() =>
        RegisterStates(new StateMachine());

    /// <summary>
    /// 使用するステートをステートマシンへ登録する。
    /// </summary>
    protected abstract StateMachine RegisterStates(StateMachine sm);
    //     where TParam : BaseStateParam {
    //     sm.RegisterState(
    //         new BaseState.Init<BaseContext<TContext, TAction, TModel>, TParam>(this));
    //     sm.RegisterState(
    //         new BaseState.Load<BaseContext<TContext, TAction, TModel>, TParam>(this));
    //     sm.RegisterState(
    //         new BaseState.Show<BaseContext<TContext, TAction, TModel>, TParam>(this));
    //     sm.RegisterState(
    //         new BaseState.Hide<BaseContext<TContext, TAction, TModel>, TParam>(this));
    //     sm.RegisterState(
    //         new BaseState.Unload<BaseContext<TContext, TAction, TModel>, TParam>(this));
    //     return sm;
    // }

    /// <summary>
    /// this._sm.SetStartState();
    /// </summary>
    /// <summary>
    /// ステートマシンの初期ステートを設定する。
    /// </summary>
    protected abstract UniTask SetInitialStateAsync();

    protected StateMachine _sm;
    public virtual StateMachine StateMachine => _sm;

    /// <summary>
    /// 現在保持しているモデルを指定型にキャストして取得する。
    /// </summary>
    public T GetModel<T>() where T : ISamModel => Model as T;
    /// <summary>
    /// モデルインスタンスを外部から差し替える。
    /// </summary>
    public void SetModel<T>(T model) where T : ISamModel {
        Model = model as TModel;
    }

    public virtual T GetContext<T>() where T : SamBaseContext<TModel> {
        return this as T;
    }

    public TModel Model;

    protected SamBaseContext() {
    }

    /// <summary>
    /// モデルとステートマシンを初期化し、初期ステートへ遷移させる。
    /// </summary>
    public virtual async UniTask SetupAsync(SamSetupParam setupParam) {
        await UniTask.SwitchToMainThread();
        Model = await CreateModelAsync();
        _sm = CreateStateMachine();

        _sm.Run();
        await SetInitialStateAsync();
        OnSetupCompleted.Value = ContextType;
    }

    /// <summary>
    /// モデルとステートマシンを非同期に破棄する。
    /// </summary>
    public virtual async UniTask DisposeAsync() {
        if (Model != null) {
            await Model.DisposeAsync();
        }
        Dispose();
    }

    /// <summary>
    /// 非同期要素以外のリソースを破棄し、ステートマシンを停止する。
    /// </summary>
    public virtual void Dispose() {
        Model = null;

        _sm?.Stop();
        _sm = null;
        AsDisposable.Dispose();
        _onDispose.OnNext(Unit.Default);
    }

    /// <summary>
    /// ステートマシンに外部遷移を要求する。
    /// </summary>
    public async UniTask TransitionExternalAsync<TState1, TParam>(TParam param = null)
        where TState1 : IState
        where TParam : Options {
        if (StateMachine == null) {
            return;
        }
        await StateMachine.RequestTransitionExternalAsync<TState1, TParam>(param);
    }

    /// <summary>
    /// ステートマシンに内部遷移を 1 ステップ要求する。
    /// </summary>
    public async UniTask TransitionInternalAsync<TState1, TParam>(TParam param = null)
        where TState1 : IState
        where TParam : Options {
        if (StateMachine == null) {
            return;
        }
        await StateMachine.RequestTransitionInternalAsync<TState1, TParam>(param);
    }

    /// <summary>
    /// 2 連続の内部遷移を直列で要求するヘルパー。
    /// </summary>
    public async UniTask TransitionInternalAsync<TState1, TState2, TParam>(TParam param = null)
        where TState1 : IState
        where TState2 : IState
        where TParam : Options {
        await TransitionInternalAsync<TState1, TParam>(param);
        await TransitionInternalAsync<TState2, TParam>(param);
    }

    /// <summary>
    /// 3 連続の内部遷移を順番に実行する。
    /// </summary>
    public async UniTask TransitionInternalAsync<TState1, TState2, TState3, TParam>(TParam param = null)
        where TState1 : IState
        where TState2 : IState
        where TState3 : IState
        where TParam : Options {
        await TransitionInternalAsync<TState1, TState2, TParam>(param);
        await TransitionInternalAsync<TState3, TParam>(param);
    }

    /// <summary>
    /// 現在アクティブなステートが指定型かどうか判定する。
    /// </summary>
    public bool IsCurrentState<TState>() where TState : IState {
        return StateMachine.IsCurrentState<TState>();
    }

    /// <summary>
    /// 登録されているステートインスタンスを取得する。
    /// </summary>
    public TState GetState<TState>() where TState : IState {
        if (StateMachine == null) {
            return default(TState);
        }
        return StateMachine.GetState<TState>();
    }

    /// <summary>
    /// モデルが解放済みかどうかを示す。
    /// </summary>
    public virtual bool IsDisposed => Model == null;
    protected readonly Subject<Unit> _onDispose = new Subject<Unit>();
    /// <summary>
    /// Dispose 通知を購読するための Observable を返す。
    /// </summary>
    public Observable<Unit> OnDisposeAsObservable() => _onDispose.AsObservable();

}

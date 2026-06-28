using System;
using AsyncFSM;
using Cysharp.Threading.Tasks;
using R3;
using Random = UnityEngine.Random;

/// <summary>
/// base scene context class
/// </summary>
/// <remarks>
/// <para>
/// Lifecycle
/// 1. <see cref="SetupAsync"/>
/// 2.  InitialState
/// </para>
/// </remarks>
/// <typeparam name="TModel">dao</typeparam>
public abstract class SamBaseContext<TModel> : ISamContext, ISamAction
    where TModel : ISamModel, new() {
    public virtual ContextType ContextType => ContextType.None;
    public CompositeDisposable AsDisposable { get; } = new();

    // public T GetContext<T>() where T: ISamContext => this as T;
    public AsyncReactiveProperty<ContextType> OnSetupCompleted { get; } =
        new AsyncReactiveProperty<ContextType>(ContextType.None);

    protected virtual async UniTask<TModel> CreateModelAsync() {
        var model = new TModel();
        await model.SetupAsync(ContextType);
        return model;
    }

    protected virtual StateMachine CreateStateMachine() =>
        RegisterStates(new StateMachine());

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
    protected abstract UniTask SetInitialStateAsync();

    protected StateMachine _sm;
    public virtual StateMachine StateMachine => _sm;

    public T GetModel<T>() where T : ISamModel => Model as T;
    public void SetModel<T>(T model) where T : ISamModel {
        Model = model as TModel;
    }

    public virtual T GetContext<T>() where T : SamBaseContext<TModel> {
        return this as T;
    }
    /// <summary>
    /// dao
    /// </summary>
    public TModel Model;

    protected SamBaseContext() {
    }

    public virtual async UniTask SetupAsync(SamSetupParam setupParam) {
        await UniTask.SwitchToMainThread();
        Model = await CreateModelAsync();
        _sm = CreateStateMachine();

        _sm.Run();
        await SetInitialStateAsync();
        OnSetupCompleted.Value = ContextType;
    }

    public virtual async UniTask DisposeAsync() {
        if (Model != null) {
            await Model.DisposeAsync();
        }
        Dispose();
    }

    public virtual void Dispose() {
        Model = null;

        _sm?.Stop();
        _sm = null;
        AsDisposable.Dispose();
        _onDispose.OnNext(Unit.Default);
    }

    /// <summary>
    /// 次のUpdateで処理が開始される
    /// </summary>
    /// <param name="param"></param>
    /// <typeparam name="TState1"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <returns></returns>
    public async UniTask TransitionExternalAsync<TState1, TParam>(TParam param = null)
        where TState1 : IState
        where TParam : Options {
        if (StateMachine == null) {
            return;
        }
        await StateMachine.RequestTransitionExternalAsync<TState1, TParam>(param);
    }

    public async UniTask TransitionInternalAsync<TState1, TParam>(TParam param = null)
        where TState1 : IState
        where TParam : Options {
        if (StateMachine == null) {
            return;
        }
        await StateMachine.RequestTransitionInternalAsync<TState1, TParam>(param);
    }

    public async UniTask TransitionInternalAsync<TState1, TState2, TParam>(TParam param = null)
        where TState1 : IState
        where TState2 : IState
        where TParam : Options {
        await TransitionInternalAsync<TState1, TParam>(param);
        await TransitionInternalAsync<TState2, TParam>(param);
    }

    public async UniTask TransitionInternalAsync<TState1, TState2, TState3, TParam>(TParam param = null)
        where TState1 : IState
        where TState2 : IState
        where TState3 : IState
        where TParam : Options {
        await TransitionInternalAsync<TState1, TState2, TParam>(param);
        await TransitionInternalAsync<TState3, TParam>(param);
    }

    public bool IsCurrentState<TState>() where TState : IState {
        return StateMachine.IsCurrentState<TState>();
    }

    public TState GetState<TState>() where TState : IState {
        if (StateMachine == null) {
            return default(TState);
        }
        return StateMachine.GetState<TState>();
    }
    public virtual bool IsDisposed => Model == null;
    protected readonly Subject<Unit> _onDispose = new Subject<Unit>();
    public Observable<Unit> OnDisposeAsObservable() => _onDispose.AsObservable();
}

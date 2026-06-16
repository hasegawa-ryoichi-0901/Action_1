using AsyncFSM;

using Cysharp.Threading.Tasks;

using R3;

public interface ISamAction {
    public T GetModel<T>() where T : ISamModel;
    public void SetModel<T>(T model) where T : ISamModel;
    // public T GetContext<T>() where T : ISamContext;
    public UniTask TransitionExternalAsync<TState1, TParam>(TParam param)
        where TState1 : IState
        where TParam : Options;

    public UniTask TransitionInternalAsync<TState1, TParam>(TParam param)
        where TState1 : IState
        where TParam : Options;

    public UniTask TransitionInternalAsync<TState1, TState2, TParam>(TParam param)
        where TState1 : IState
        where TState2 : IState
        where TParam : Options;

    public UniTask TransitionInternalAsync<TState1, TState2, TState3, TParam>(TParam param)
        where TState1 : IState
        where TState2 : IState
        where TState3 : IState
        where TParam : Options;
    public bool IsCurrentState<TState>() where TState : IState;
    public TState GetState<TState>() where TState : IState;
    public bool IsDisposed { get; }
    public Observable<Unit> OnDisposeAsObservable();
    public CompositeDisposable AsDisposable { get; }
}

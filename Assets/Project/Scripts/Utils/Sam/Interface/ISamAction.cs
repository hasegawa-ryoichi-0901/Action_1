using AsyncFSM;

using Cysharp.Threading.Tasks;

using R3;

public interface ISamAction {
    T GetModel<T>() where T : ISamModel;
    void SetModel<T>(T model) where T : ISamModel;
    // public T GetContext<T>() where T : ISamContext;
    UniTask TransitionExternalAsync<TState1, TParam>(TParam param)
        where TState1 : IState
        where TParam : Options;

    UniTask TransitionInternalAsync<TState1, TParam>(TParam param)
        where TState1 : IState
        where TParam : Options;

    UniTask TransitionInternalAsync<TState1, TState2, TParam>(TParam param)
        where TState1 : IState
        where TState2 : IState
        where TParam : Options;

    UniTask TransitionInternalAsync<TState1, TState2, TState3, TParam>(TParam param)
        where TState1 : IState
        where TState2 : IState
        where TState3 : IState
        where TParam : Options;
    bool IsCurrentState<TState>() where TState : IState;
    TState GetState<TState>() where TState : IState;
    bool IsDisposed { get; }
    Observable<Unit> OnDisposeAsObservable();
    CompositeDisposable AsDisposable { get; }
}

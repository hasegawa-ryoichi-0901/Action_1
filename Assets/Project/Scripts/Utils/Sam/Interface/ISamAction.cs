using AsyncFSM;
using Cysharp.Threading.Tasks;
using R3;

/// <summary>
/// SAM コンテキスト・ステート間で共通して利用するアクション API。
/// </summary>
/// <remarks>
/// 使用例: <c>await context.TransitionInternalAsync<State, Options>(param);</c>
/// </remarks>
public interface ISamAction {

    /// <summary>
    /// 現在のコンテキストにバインドされたモデルを取得する。
    /// 使用例: <c>var model = action.GetModel<MyModel>();</c>
    /// </summary>
    T GetModel<T>() where T : ISamModel;
    /// <summary>
    /// コンテキストにモデルインスタンスを明示的に差し替える。
    /// 使用例: <c>action.SetModel(mockModel);</c>
    /// </summary>
    void SetModel<T>(T model) where T : ISamModel;
    // public T GetContext<T>() where T : ISamContext;

    /// <summary>
    /// 外部遷移
    /// </summary>
    /// <param name="param"></param>
    /// <typeparam name="TState1"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <returns></returns>
    /// <summary>
    /// 他コンテキストへ影響を与える外部遷移を要求する。
    /// 使用例: <c>await action.TransitionExternalAsync<HideState, Param>(options);</c>
    /// </summary>
    UniTask TransitionExternalAsync<TState1, TParam>(TParam param)
        where TState1 : IState
        where TParam : Options;

    /// <summary>
    /// 内部遷移
    /// </summary>
    /// <param name="param"></param>
    /// <typeparam name="TState1"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <returns></returns>
    /// <summary>
    /// 自コンテキスト内のステートを 1 つだけ切り替える。
    /// 使用例: <c>await action.TransitionInternalAsync<ShowState, Param>(options);</c>
    /// </summary>
    UniTask TransitionInternalAsync<TState1, TParam>(TParam param)
        where TState1 : IState
        where TParam : Options;

    /// <summary>
    /// 内部遷移
    /// </summary>
    /// <param name="param"></param>
    /// <typeparam name="TState1"></typeparam>
    /// <typeparam name="TState2"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <returns></returns>
    /// <summary>
    /// 2 つの内部ステート遷移を直列に実行するヘルパー。
    /// 使用例: <c>await action.TransitionInternalAsync<Hide, Unload, Param>(options);</c>
    /// </summary>
    UniTask TransitionInternalAsync<TState1, TState2, TParam>(TParam param)
        where TState1 : IState
        where TState2 : IState
        where TParam : Options;

    /// <summary>
    /// 内部遷移
    /// </summary>
    /// <param name="param"></param>
    /// <typeparam name="TState1"></typeparam>
    /// <typeparam name="TState2"></typeparam>
    /// <typeparam name="TState3"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <returns></returns>
    /// <summary>
    /// 3 つの内部ステート遷移を順番に実行する。
    /// 使用例: <c>await action.TransitionInternalAsync<Init, Load, Show, Param>(options);</c>
    /// </summary>
    UniTask TransitionInternalAsync<TState1, TState2, TState3, TParam>(TParam param)
        where TState1 : IState
        where TState2 : IState
        where TState3 : IState
        where TParam : Options;
    /// <summary>
    /// 現在のステートが指定型かどうか調べる。
    /// 使用例: <c>if (action.IsCurrentState<ShowState>()) ...</c>
    /// </summary>
    bool IsCurrentState<TState>() where TState : IState;
    /// <summary>
    /// ステートインスタンスを取得する。存在しない場合は default。
    /// 使用例: <c>var state = action.GetState<LoadState>();</c>
    /// </summary>
    TState GetState<TState>() where TState : IState;
    /// <summary>
    /// コンテキスト/モデル破棄済みかを示すフラグ。
    /// 使用例: <c>if (action.IsDisposed) return;</c>
    /// </summary>
    bool IsDisposed { get; }
    /// <summary>
    /// 破棄タイミングを監視する Observable。
    /// 使用例: <c>action.OnDisposeAsObservable().Subscribe(...);</c>
    /// </summary>
    Observable<Unit> OnDisposeAsObservable();
    /// <summary>
    /// ライフタイム管理用のディスポーザブルコンテナ。
    /// 使用例: <c>action.AsDisposable.Add(subscription);</c>
    /// </summary>
    CompositeDisposable AsDisposable { get; }
}

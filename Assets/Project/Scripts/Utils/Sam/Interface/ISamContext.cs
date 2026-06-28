using AsyncFSM;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using R3;

/// <summary>
/// 各コンテキストの初期化時に渡されるセットアップ情報のベース。
/// </summary>
public class SamSetupParam {
}

/// <summary>
/// SAM における各コンテキストの基本契約。
/// </summary>
/// <remarks>
/// 使用例: <c>await context.SetupAsync(new SamSetupParam());</c>
/// </remarks>
public interface ISamContext {
    /// <summary>
    /// コンテキストが所有するステートマシン インスタンス。
    /// 使用例: <c>context.StateMachine.RunUpdate = true;</c>
    /// </summary>
    abstract StateMachine StateMachine { get; }

    /// <summary>
    /// Setup 完了時にコンテキスト種別を通知する ReactiveProperty。
    /// 使用例: <c>await context.OnSetupCompleted.WaitAsync(...);</c>
    /// </summary>
    AsyncReactiveProperty<ContextType> OnSetupCompleted { get; }
    IUniTaskAsyncEnumerable<ContextType> OnSetupCompletedAsAsyncEnumerable() =>
        OnSetupCompleted.AsUniTaskAsyncEnumerable();
    /// <summary>
    /// コンテキストを識別する種別。
    /// 使用例: <c>if (context.ContextType == ContextType.Title) ...</c>
    /// </summary>
    abstract ContextType ContextType { get; }
    /// <summary>
    /// モデル・ステートマシンの初期化を実行する。
    /// 使用例: <c>await context.SetupAsync(param);</c>
    /// </summary>
    abstract UniTask SetupAsync(SamSetupParam setupParam = null);
    /// <summary>
    /// コンテキストを非同期に破棄する。
    /// 使用例: <c>await context.DisposeAsync();</c>
    /// </summary>
    abstract UniTask DisposeAsync();
    /// <summary>
    /// サブスクリプションをまとめて破棄するためのコンテナ。
    /// 使用例: <c>context.AsDisposable.Add(subscription);</c>
    /// </summary>
    CompositeDisposable AsDisposable { get; }
}

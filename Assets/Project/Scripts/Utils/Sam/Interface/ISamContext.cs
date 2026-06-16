
using AsyncFSM;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using R3;

public class SamSetupParam {

}
/// <summary>
///
/// </summary>
public interface ISamContext {
    abstract StateMachine StateMachine { get; }
    AsyncReactiveProperty<ContextType> OnSetupCompleted { get; }
    IUniTaskAsyncEnumerable<ContextType> OnSetupCompletedAsAsyncEnumerable() =>
        OnSetupCompleted.AsUniTaskAsyncEnumerable();
    abstract ContextType ContextType { get; }
    abstract UniTask SetupAsync(SamSetupParam setupParam = null);
    CompositeDisposable AsDisposable { get; }
}

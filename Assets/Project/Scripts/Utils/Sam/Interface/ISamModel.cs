using Cysharp.Threading.Tasks;

/// <summary>
/// Scene Model base class.
/// Each scene model should inherit from this class and implement the DisposeAsync method to clean up resources when the scene is unloaded.
/// </summary>
public abstract class ISamModel {
    public ContextType ContextType { get; protected set; }
    public virtual async UniTask SetupAsync(ContextType contextType) {
        ContextType = contextType;
    }
    public abstract UniTask DisposeAsync();
}

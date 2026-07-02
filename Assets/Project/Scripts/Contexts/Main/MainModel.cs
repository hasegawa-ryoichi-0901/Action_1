
using Cysharp.Threading.Tasks;

public class MainModel : SamBaseModel {
    public override async UniTask SetupAsync(ContextType contextType) {
        await base.SetupAsync(contextType);
    }
}

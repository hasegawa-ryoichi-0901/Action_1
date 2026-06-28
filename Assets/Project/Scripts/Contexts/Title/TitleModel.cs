using Cysharp.Threading.Tasks;

public class TitleModel : SamBaseModel {
    public override async UniTask SetupAsync(ContextType contextType) {
        await base.SetupAsync(contextType);
    }
}

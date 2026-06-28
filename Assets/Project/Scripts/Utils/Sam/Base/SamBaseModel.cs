using Cysharp.Threading.Tasks;

public abstract class SamBaseModel : ISamModel {

    /// <summary>
    /// モデルが保持するリソースを解放する。必要に応じて派生でオーバーライドする。
    /// </summary>
    public override async UniTask DisposeAsync() {
    }

}

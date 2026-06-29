using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Loaderのデータモデルクラス
/// </summary>
public abstract class BaseResourceModel : IBaseResourceModel {
    public virtual async UniTask SetupAsync() {
    }
    public abstract UniTask DisposeAsync();
}

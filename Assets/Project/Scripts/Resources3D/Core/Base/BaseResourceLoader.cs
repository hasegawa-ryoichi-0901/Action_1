using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 3Dモデルのライフサイクルを管理するクラス
/// </summary>
/// <typeparam name="TModel"></typeparam>
public abstract class BaseResourceLoader<TModel> : IResourcesLoader, IResourceLoaderAction
    where TModel : BaseResourceModel, new() {
    protected TModel _model { get; private set; }

    public virtual async UniTask SetupAsync() {
        _model = new TModel();
        await _model.SetupAsync();
    }

    public virtual async UniTask LoadAsync() { }
    public virtual async UniTask SpawnAsync() { }
    public virtual async UniTask DespawnAsync() { }
    public virtual async UniTask UnloadAsync() { }
}

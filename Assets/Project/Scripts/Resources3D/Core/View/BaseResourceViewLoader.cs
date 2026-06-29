using UnityEngine;

/// <summary>
/// GameObjectのライフサイクル
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TView"></typeparam>
public abstract class BaseResourceViewLoader<TModel, TView, TAction> : BaseResourceLoader<TModel>,
    IResourceViewLoader<TView, TModel, TAction>, IResourceViewAction
    where TModel : BaseResourceModel, new()
    where TAction : class, IResourceViewAction
    where TView : BaseResourceView<TModel, TAction> {
    public abstract string ResourceViewPath { get; }
    public TView View { get; protected set; }
    public GameObject ViewPrefab { get; protected set; }
    public GameObject ViewObject { get; protected set; }
}

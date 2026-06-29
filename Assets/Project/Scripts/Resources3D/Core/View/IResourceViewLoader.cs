using UnityEngine;

public interface IResourceViewLoader<TView, TModel, TAction> : IResourcesLoader
    where TModel : IBaseResourceModel, new()
    where TAction : class, IResourceViewAction
    where TView : BaseResourceView<TModel, TAction> {
    public abstract string ResourceViewPath { get; }
    public GameObject ViewPrefab { get; }
    public GameObject ViewObject { get; }
    public TView View { get; }
}

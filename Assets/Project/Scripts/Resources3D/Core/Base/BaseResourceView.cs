using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームオブジェクトの基底レイヤー
/// </summary>
public class BaseResourceView<TModel, TAction> : MonoBehaviour
    where TModel : IBaseResourceModel
    where TAction : IResourceViewAction {

    protected TAction _action { get; private set; }
    protected TModel _model { get; private set; }
    public virtual async UniTask SetupAsync(TAction action, TModel model) {
        _action = action;
        _model = model;
    }
}

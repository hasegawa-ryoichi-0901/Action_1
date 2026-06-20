using AsyncFSM;

using Cysharp.Threading.Tasks;

using R3;
using R3.Triggers;

using UnityEngine;

public abstract class SamViewContext<TModel, TView> : SamBaseContext<TModel>, ISamViewAction
    where TModel : SamViewModel<TView>, new()
    where TView : MonoBehaviour {
    public TView View {
        get => Model.View;
        protected set => Model.View = value;
    }

    /// <summary>
    /// Viewオブジェクトが存在する間にUpdate処理を行うか
    /// </summary>
    public virtual bool CanInvokeUpdateWhileAliveView => true;
    public virtual async UniTask<GameObject> LoadViewPrefabAsync() {
        return Resources.Load(Model.ViewPrefabPath) as GameObject;
    }
    public virtual void InstantiateViewObject() {
        var saveActive = Model.ViewPrefab.activeSelf;
        Model.ViewPrefab.SetActive(false);
        Model.ViewObject = Object.Instantiate(Model.ViewPrefab, GetViewParent(), false);
        Model.ViewPrefab.SetActive(saveActive);
        var a = this.AsDisposable;
    }

    public virtual RectTransform GetViewParent() {
        //return ContextManager.Instance.RootRectTrans;
        //todo: ContextManagerからRootのTransformを取得してくる
        return null;
    }

    public virtual async UniTask OnPostInstantiateAsync() {
        Model.View = Model.ViewObject.GetComponent<TView>();
        if (this.CanInvokeUpdateWhileAliveView) {
            Model.View?.OnDestroyAsObservable().Subscribe(_ => {
                if (IsDisposed) {
                    return;
                }

                StateMachine.RunUpdate = false;
            });
        }
    }

    public virtual async UniTask ShowAsync() {
        await UniTask.SwitchToMainThread();
        Model.ViewObject.SetActive(true);
    }

    public virtual async UniTask HideAsync() {
        await UniTask.SwitchToMainThread();
        Model.ViewObject.SetActive(false);
    }

    public override async UniTask DisposeAsync() {
        if (Model != null) {
            if (Model.ViewObject) {
                GameObject.Destroy(Model.ViewObject);
            }

            Model.ViewObject = null;
            Model.View = null;
        }

        await base.DisposeAsync();
    }
}

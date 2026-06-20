using System.Data;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using UnityEngine;
using R3;


public class SamViewStateParam : SamBaseStateParam {
}

namespace SamViewState {
    /// <summary>
    /// NO Operation
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TStateParam"></typeparam>
    /// <typeparam name="TView"></typeparam>
    public class Default<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {
        public TView View => Model.View;

        public Default(TContext context) : base(context) {
        }
    }

    public class Init<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {
        public TView View => Model.View;

        public Init(TContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await base.OnEnter();
            await UniTask.SwitchToMainThread();

            if (Model.ViewObject) {
                GameObject.Destroy(Model.ViewObject);
                Model.ViewObject = null;
            }

            if (Model.ViewPrefab) {
                Resources.UnloadAsset(Model.ViewPrefab);
                Model.ViewPrefab = null;
            }

            Model.ViewPrefab = await Context.LoadViewPrefabAsync();
            if (!Model.ViewPrefab) {
                throw new DataException(Model.ViewPrefabPath);
            }
        }
    }

    public class Load<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {

        public Load(TContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await base.OnEnter();
            await UniTask.SwitchToMainThread();
            Context.InstantiateViewObject();
            await Context.OnPostInstantiateAsync();
        }
    }

    public class Show<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {
        public TView View => Model.View;

        public Show(TContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await base.OnEnter();
            await Context.ShowAsync();
        }

    }

    public class Active<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {
        public TView View => Model.View;

        public Active(TContext context) : base(context) {
        }
    }

    public class Hide<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {
        public TView View => Model.View;

        public Hide(TContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await base.OnEnter();
            await HideAsync();
        }

        protected virtual async UniTask HideAsync() {
            await UniTask.SwitchToMainThread();
            Model.ViewObject.SetActive(false);
        }
    }

    public class Unload<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamViewModel<TView>, new()
        where TStateParam : SamBaseStateParam
        where TView : MonoBehaviour {
        public TView View => Model.View;

        public Unload(TContext context) : base(context) {
        }

        public override async UniTask OnEnter() {
            await UniTask.SwitchToMainThread();
            if (Model.ViewObject) {
                GameObject.Destroy(Model.ViewObject);
            }

            Model.ViewObject = null;
            Model.View = null;

            if (Model != null) {
                Model.ViewPrefab = null;
                await Resources.UnloadUnusedAssets();
                await Model.DisposeAsync();
            }

            await base.OnEnter();

        }
    }
}

using Cysharp.Threading.Tasks;

namespace MainState {

    public class Param : SamViewStateParam {

    }

    public class Default<TContext, TModel, TStateParam, TView> : SamViewState.Default<TContext, TModel, TStateParam, TView>
        where TContext : IMainAction, new()
        where TModel : MainModel, new()
        where TStateParam : Param
        where TView : MainView {
        public Default(TContext context) : base(context) {

        }
    }

    public class Init<TContext, TModel, TStateParam, TView> : SamViewState.Init<TContext, TModel, TStateParam, TView>
        where TContext : IMainAction, new()
        where TModel : MainModel, new()
        where TStateParam : Param
        where TView : MainView {
        public Init(TContext context) : base(context) {
        }
    }

    public class Load<TContext, TModel, TStateParam, TView> : SamViewState.Load<TContext, TModel, TStateParam, TView>
        where TContext : IMainAction, new()
        where TModel : MainModel, new()
        where TStateParam : Param
        where TView : MainView {
        public Load(TContext context) : base(context) {

        }

        public override async UniTask OnEnter() {
            await base.OnEnter();
            await _view.SetupAsync(Context);
        }
    }

    public class Show<TContext, TModel, TStateParam, TView> : SamViewState.Show<TContext, TModel, TStateParam, TView>
        where TContext : IMainAction, new()
        where TModel : MainModel, new()
        where TStateParam : Param
        where TView : MainView {

        public Show(TContext context) : base(context) {

        }
    }

    public class Hide<TContext, TModel, TStateParam, TView> : SamViewState.Hide<TContext, TModel, TStateParam, TView>
        where TContext : IMainAction, new()
        where TModel : MainModel, new()
        where TStateParam : Param
        where TView : MainView {

        public Hide(TContext context) : base(context) {

        }
    }

    public class Unload<TContext, TModel, TStateParam, TView> : SamViewState.Unload<TContext, TModel, TStateParam, TView>
        where TContext : IMainAction, new()
        where TModel : MainModel, new()
        where TStateParam : Param
        where TView : MainView {

        public Unload(TContext context) : base(context) {

        }
    }
}



using AsyncFSM;
using Cysharp.Threading.Tasks;
using MainState;

public class MainContext : MainContext<MainModel, MainView> {
    public override string ViewPrefabPath => "Prefabs/Contexts/MainView";

    public override TTransform GetViewParent<TTransform>() {
        return (TTransform)ContextManager.Instance.Canvas.transform;
    }

    protected override StateMachine RegisterStates(StateMachine sm) {
        sm.RegisterState(new Init<MainContext, MainModel, Param, MainView>(this));
        sm.RegisterState(new Load<MainContext, MainModel, Param, MainView>(this));
        sm.RegisterState(new Show<MainContext, MainModel, Param, MainView>(this));
        sm.RegisterState(new Default<MainContext, MainModel, Param, MainView>(this));
        sm.RegisterState(new Hide<MainContext, MainModel, Param, MainView>(this));
        sm.RegisterState(new Unload<MainContext, MainModel, Param, MainView>(this));
        return sm;
    }

    protected override async UniTask SetInitialStateAsync() {
        await _sm.RequestTransitionExternalAsync<Init<MainContext, MainModel, Param, MainView>, Param>();
    }

    public override async UniTask SetupAsync(SamSetupParam param) {
        await base.SetupAsync(param);
        await TransitionInternalAsync<Load<MainContext, MainModel, Param, MainView>, Param>();
        await TransitionInternalAsync<Show<MainContext, MainModel, Param, MainView>, Param>();
    }
}

public abstract class MainContext<TModel, TView> : SamViewContext<TModel, TView>, IMainContext<TView>, IMainAction
    where TModel : MainModel, new()
    where TView : MainView {

}

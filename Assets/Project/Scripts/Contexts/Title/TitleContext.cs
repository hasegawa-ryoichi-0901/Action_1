using AsyncFSM;
using Cysharp.Threading.Tasks;
using TitleState;

public class TitleSetupParam : SamSetupParam {
}

public class TitleContext : TitleContext<TitleModel, TitleView> {
    #region Singleton
    private static TitleContext _instance;
    public static TitleContext Instance => _instance ??= new TitleContext();
    #endregion Singleton
    public override string ViewPrefabPath => "Prefabs/Contexts/TitleView";

    public override TTransform GetViewParent<TTransform>() {
        return (TTransform)ContextManager.Instance.Canvas.transform;
    }

    protected override StateMachine RegisterStates(StateMachine sm) {
        sm.RegisterState(new Init<TitleContext, TitleModel, Param, TitleView>(this));
        sm.RegisterState(new Load<TitleContext, TitleModel, Param, TitleView>(this));
        sm.RegisterState(new Show<TitleContext, TitleModel, Param, TitleView>(this));
        sm.RegisterState(new Default<TitleContext, TitleModel, Param, TitleView>(this));
        sm.RegisterState(new Hide<TitleContext, TitleModel, Param, TitleView>(this));
        sm.RegisterState(new Unload<TitleContext, TitleModel, Param, TitleView>(this));
        return sm;
    }

    protected override async UniTask SetInitialStateAsync() {
        await _sm.RequestTransitionExternalAsync<Init<TitleContext, TitleModel, Param, TitleView>, Param>();
    }

    public override async UniTask SetupAsync(SamSetupParam param) {
        await base.SetupAsync(param);
        await TransitionInternalAsync<Load<TitleContext, TitleModel, Param, TitleView>, Param>();
        await TransitionInternalAsync<Show<TitleContext, TitleModel, Param, TitleView>, Param>();
    }
}

public abstract class TitleContext<TModel, TView> : SamViewContext<TModel, TView>, ITitleAction, ITitleContext
    where TModel : TitleModel, new()
    where TView : TitleView {
    TitleView ISamViewContext<TitleView>.View => View;
}

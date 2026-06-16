using AsyncFSM;
public class SamBaseStateParam : Options {

}

public abstract class SamBaseState<TContext, TModel, TStateParam> : State<TStateParam>
    where TContext : ISamAction, new()
    where TModel : SamBaseModel, new()
    where TStateParam : SamBaseStateParam {
    public override bool HasUpdate => false;
    public TContext Context { get; protected set; }
    public TModel Model { get; protected set; }

    public SamBaseState(TContext context) {
        this.Context = context;
        this.Model = this.Context.GetModel<TModel>();
    }
}

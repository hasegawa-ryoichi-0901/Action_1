using AsyncFSM;

/// <summary>
/// ステート遷移時に渡すパラメータの共通基底。
/// </summary>
public class SamBaseStateParam : Options {
}

public abstract class SamBaseState<TContext, TModel, TStateParam> : State<TStateParam>
    where TContext : ISamAction, new()
    where TModel : SamBaseModel, new()
    where TStateParam : SamBaseStateParam {
    public override bool HasUpdate => false;
    public TContext Context { get; protected set; }
    public TModel Model { get; protected set; }

    /// <summary>
    /// ステートをコンテキスト・モデルに紐付けて初期化する。
    /// </summary>
    public SamBaseState(TContext context) {
        Context = context;
        Model = Context.GetModel<TModel>();
    }
}

using Cysharp.Threading.Tasks;

using UnityEngine;

/// <summary>
/// View 系ステートで利用する追加パラメータの基底。
/// </summary>
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
    public class Default<TContext, TModel, TStateParam, TView> : SamViewBaseState<TContext, TModel, TStateParam, TView>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {

        public Default(TContext context) : base(context) {

        }
    }

    public class Init<TContext, TModel, TStateParam, TView> : SamViewBaseState<TContext, TModel, TStateParam, TView>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {

        public Init(TContext context) : base(context) {
        }

        /// <summary>
        /// View プレハブ読み込み前の準備を実行する。
        /// </summary>
        public override async UniTask OnEnter() {
            await base.OnEnter();
            await Context.PrepareViewAsync();
        }
    }

    public class Load<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {

        public Load(TContext context) : base(context) {
        }

        /// <summary>
        /// プレハブから View を生成し、追加セットアップを行う。
        /// </summary>
        public override async UniTask OnEnter() {
            await base.OnEnter();
            await UniTask.SwitchToMainThread();
            Context.InstantiateViewObject();
            await Context.OnPostInstantiateAsync();
        }
    }

    public class Show<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {

        public Show(TContext context) : base(context) {
        }

        /// <summary>
        /// View を表示状態へ切り替える。
        /// </summary>
        public override async UniTask OnEnter() {
            await base.OnEnter();
            await Context.ShowAsync();
        }

    }

    public class Active<TContext, TModel, TStateParam, TView> : SamViewBaseState<TContext, TModel, TStateParam, TView>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {

        public Active(TContext context) : base(context) {
        }
    }

    public class Hide<TContext, TModel, TStateParam, TView> : SamViewBaseState<TContext, TModel, TStateParam, TView>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {
        public Hide(TContext context) : base(context) {
        }

        /// <summary>
        /// View を非表示にして更新ループを停止する。
        /// </summary>
        public override async UniTask OnEnter() {
            await base.OnEnter();
            await HideAsync();
        }

        /// <summary>
        /// メインスレッドで View を無効化する。
        /// </summary>
        protected virtual async UniTask HideAsync() {
            await UniTask.SwitchToMainThread();
            await Context.HideAsync();
        }
    }

    public class Unload<TContext, TModel, TStateParam, TView> : SamViewBaseState<TContext, TModel, TStateParam, TView>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {

        public Unload(TContext context) : base(context) {
        }

        /// <summary>
        /// View・モデルを破棄して状態を初期化する。
        /// </summary>
        public override async UniTask OnEnter() {
            await UniTask.SwitchToMainThread();
            await Context.UnloadAsync();
            await base.OnEnter();
        }
    }

    public class SamViewBaseState<TContext, TModel, TStateParam, TView> : SamBaseState<TContext, TModel, TStateParam>
        where TContext : ISamViewAction, new()
        where TModel : SamBaseModel, new()
        where TStateParam : SamViewStateParam
        where TView : MonoBehaviour {
        /// <summary>
        /// コンテキストから取得した View インスタンス。
        /// </summary>
        public TView View { get; protected set; }

        public SamViewBaseState(TContext context) : base(context) {
            View = Context.GetView<TView>();
        }
    }
}

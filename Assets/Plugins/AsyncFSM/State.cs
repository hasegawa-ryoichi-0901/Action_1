using Cysharp.Threading.Tasks;

namespace AsyncFSM {
    public abstract class State : State<Options> {
    }

    public abstract class State<T> : IState where T : Options {
        public StateMachine StateMachine { get; set; }

        protected T Param { get; private set; }

        public virtual async UniTask OnEnter() {
        }

        public virtual async UniTask OnExit() {
        }

        public void SetOptions(Options options) {
            if (options is T stateOptions) {
                Param = stateOptions;
            }
        }

        public abstract bool HasUpdate { get; }
        public virtual void OnUpdate(float deltaTime) {
        }

    }
}
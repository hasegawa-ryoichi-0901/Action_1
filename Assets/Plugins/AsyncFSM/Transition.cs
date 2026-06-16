using System;
using Cysharp.Threading.Tasks;

namespace AsyncFSM
{
    public class Transition : Transition<Options>
    {
        public Transition(Type type, Options options) : base(type, options)
        {
        }
    }

    public abstract class Transition<T> where T : Options
    {
        public Type Type { get; }
        public T Options { get; }

        protected UniTaskCompletionSource<T> _tcs = new UniTaskCompletionSource<T>();
        public UniTask<T> Task => _tcs.Task;
        public void OnCompleted()
        {
            this._tcs.TrySetResult(this.Options);
        }

        protected Transition(Type type, T options)
        {
            Type = type;
            Options = options;
        }
    }
}

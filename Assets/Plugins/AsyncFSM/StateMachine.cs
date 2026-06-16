using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AsyncFSM {
    public class StateMachine {
        private IState _currentState;
        private IState _previousState;
        private readonly Dictionary<Type, IState> _states = new();
        private readonly ConcurrentQueue<Transition> _transitionQueue = new();
        private CancellationTokenSource _cancellationTokenSource;
        public bool RunUpdate { get; set; } = true;

        public void Run() {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            Update();
        }

        public void Stop() {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void RegisterState(IState state) {
            state.StateMachine = this;

            _states.Add(state.GetType(), state);
        }

        /// <summary>
        /// 次のUpdateで状態遷移する
        /// 同StateMachineのState内のOnEnter, OnExitで実行しないこと
        /// </summary>
        /// <param name="stateType"></param>
        /// <param name="param"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public UniTask RequestTransitionExternalAsync<T>(Type stateType, T param) where T : Options {
            var transition = new Transition(stateType, param);
            _transitionQueue.Enqueue(transition);
            return transition.Task;
        }

        /// <summary>
        /// 次のUpdateで状態遷移する
        /// 同StateMachineのState内のOnEnter, OnExitで実行しないこと
        /// </summary>
        /// <param name="param"></param>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        /// <returns></returns>
        public UniTask RequestTransitionExternalAsync<TState, TParam>(TParam param = null)
            where TState : IState
            where TParam : Options {
            var transition = new Transition(typeof(TState), param);
            _transitionQueue.Enqueue(transition);
            return transition.Task;
        }


        /// <summary>
        /// 同フレーム中での状態遷移
        /// 同StateMachineのState内のOnEnter, OnExitで実行できる
        /// </summary>
        /// <param name="param"></param>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TParam"></typeparam>
        public async UniTask RequestTransitionInternalAsync<TState, TParam>(TParam param) where TParam : Options {
            await UniTask.SwitchToMainThread();
            await ChangeToAsync(typeof(TState), param);
        }

        private async UniTask ChangeToAsync<TParam>(Type stateType, TParam options) where TParam : Options {
            if (_currentState != null) {
                _previousState = _currentState;
                _currentState = null;
                if (_previousState != null) {
                    await _previousState.OnExit();
                }
            }

            if (_states.TryGetValue(stateType, out IState nextState)) {
                nextState.SetOptions(options);
                _currentState = nextState;
                await nextState.OnEnter();
            } else {
                throw new Exception($"State: {stateType.Name} is not registered to state machine.");
            }
        }

        private async UniTask<bool> ProcessTransitionAsync() {
            if (_cancellationTokenSource.IsCancellationRequested ||
                !_transitionQueue.TryDequeue(out var transition)) {
                return false;
            }

            await ChangeToAsync(transition.Type, transition.Options);
            if (_cancellationTokenSource.IsCancellationRequested) {
                return false;
            }

            transition.OnCompleted();
            return true;
        }

        private async void Update() {
            try {
                await foreach (var _ in UniTaskAsyncEnumerable
                                   .EveryUpdate()
                                   .WithCancellation(_cancellationTokenSource.Token)) {
                    while (!await ProcessTransitionAsync()) {
                        break;
                    }

                    if (this.RunUpdate && _currentState.HasUpdate) {
                        _currentState.OnUpdate(Time.deltaTime);
                    }
                }
            } catch (OperationCanceledException e) {
                // Debug.Log("StateMachine.Update() Canceled!");
            }
        }

        public bool IsCurrentState<T>() where T : IState => _currentState is T;
        public T GetState<T>() where T : IState {
            if (_states.TryGetValue(typeof(T), out var state)) {
                return (T)state;
            }
            throw new Exception($"State: {typeof(T).Name} is not registered to state machine.");
        }
    }
}
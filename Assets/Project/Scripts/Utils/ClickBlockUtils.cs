using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;

public static class ClickBlockUtils {
    private static readonly object clickBlockLock = new object();
    private static int _clickBlockCounter;
    private const float BlockSeconds = 0.3f;
    private static readonly TimeSpan blockSpan = TimeSpan.FromSeconds(BlockSeconds);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClickBlock(Action action, TimeSpan delay) {
        lock (clickBlockLock) {
            if (_clickBlockCounter > 0) {
                return;
            }

            ++_clickBlockCounter;
        }

        action();
        _ = StartClickBlockClearTimer(delay);
    }


    public static async UniTask GrabClickBlockOn(Func<UniTask> func) {
        while (true) {
            lock (clickBlockLock) {
                if (_clickBlockCounter == 0) {
                    ++_clickBlockCounter;
                    break;
                }
            }

            await UniTask.Yield();
        }

        await func();

        lock (clickBlockLock) {
            --_clickBlockCounter;
        }
    }

    public static async UniTask GrabClickBlockAsync() {
        while (true) {
            lock (clickBlockLock) {
                if (_clickBlockCounter > 0) {
                    ++_clickBlockCounter;
                    break;
                }
            }

            await UniTask.Yield();
        }
    }

    public static async UniTask ReleaseClickBlockAsync() {
        while (true) {
            lock (clickBlockLock) {
                if (_clickBlockCounter > 0) {
                    --_clickBlockCounter;
                    break;
                }
            }

            await UniTask.Yield();
        }
    }

    public static async UniTask ClickBlockAsync(TimeSpan duration) {
        lock (clickBlockLock) {
            if (_clickBlockCounter > 0) {
                return;
            }
            ++_clickBlockCounter;
        }
        await StartClickBlockClearTimer(duration);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async UniTask StartClickBlockClearTimer(TimeSpan delay) {
        await UniTask.Delay(delay);
        lock (clickBlockLock) {
            --_clickBlockCounter;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable SubscribeClick<T>(this Observable<T> observable, Action<T> action,
        TimeSpan? delay = null) {
        return observable.Subscribe(a => ClickBlock(() => action(a), delay ?? blockSpan));
    }
}
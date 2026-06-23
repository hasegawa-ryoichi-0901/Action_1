#define ASYNC_LOCK_CHANNEL
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if ASYNC_LOCK_CHANNEL
using System.Threading.Channels;
#endif

#if !ASYNC_LOCK_CHANNEL
public sealed class AsyncLock
{
    private readonly System.Threading.SemaphoreSlim m_semaphore 
        = new System.Threading.SemaphoreSlim(1, 1);
    private Task<IDisposable> m_releaser;
    public AsyncLock()
    {
        m_releaser = Task.FromResult((IDisposable) new Releaser(this));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public System.Threading.SemaphoreSlim Get () {
        return this.m_semaphore;
    }
    public async Task WaitAsync () {
        await this.m_semaphore.WaitAsync();
    }
    private static readonly Func<Task, object?, IDisposable> ContinuationFunc = ((_, o) => (IDisposable)o);
    public Task<IDisposable> LockAsync() {
        var wait = m_semaphore.WaitAsync();
        if (wait.IsCompleted) {
            return this.m_releaser;
        }

        return wait.ContinueWith(
            ContinuationFunc,
            m_releaser.Result,
            System.Threading.CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default
        );
    }
    private sealed class Releaser : IDisposable
    {
        private AsyncLock m_toRelease;
        internal Releaser(AsyncLock toRelease) { m_toRelease = toRelease; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose () {
            m_toRelease.m_semaphore.Release();
        }
    }
}
#else
public class AsyncLock
{
    private readonly Channel<bool> _channel;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AsyncLock()
    {
        // 単一アイテムのバッファで、ロックトークンの排他制御
        _channel = Channel.CreateBounded<bool>(1);
        _channel.Writer.TryWrite(true); // 最初にロック権を入れておく
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<IDisposable> LockAsync()
    {
        // ロック権を取得（チャネルから読み取る）
        await _channel.Reader.ReadAsync();
        return new Releaser(_channel.Writer);
    }

    private readonly struct Releaser : IDisposable
    {
        private readonly ChannelWriter<bool> _writer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Releaser(ChannelWriter<bool> writer)
        {
            _writer = writer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            // ロック解放（チャネルに戻す）
            _writer.TryWrite(true);
        }
    }
}
#endif
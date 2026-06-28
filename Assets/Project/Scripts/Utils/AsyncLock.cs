#define ASYNC_LOCK_CHANNEL
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if ASYNC_LOCK_CHANNEL
using System.Threading.Channels;
#endif

#if !ASYNC_LOCK_CHANNEL
/// <summary>
/// 非同期処理同士の排他制御を行うための軽量ロックです。
/// </summary>
/// <remarks>
/// <para>
/// <see cref="LockAsync"/> でロックを取得し、返却される <see cref="IDisposable"/> を
/// <c>using</c> で破棄することでロックを解放します。
/// </para>
/// <para>
/// 使用例:
/// <code>
/// private readonly AsyncLock _lock = new AsyncLock();
///
/// public async Task UpdateAsync()
/// {
///     using (await _lock.LockAsync())
///     {
///         await DoSomethingAsync();
///     }
/// }
/// </code>
/// </para>
/// <para>
/// 同じインスタンスに対してネストしてロック取得すると、自分自身の解放待ちになります。
/// 再入可能なロックではないため、同一フローからの二重取得は避けてください。
/// </para>
/// </remarks>
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
    /// <summary>
    /// ロックが解放されるまで待機します。
    /// </summary>
    /// <remarks>
    /// 手動で <see cref="Get"/> と組み合わせて使うための低レベル API です。
    /// 通常は解放漏れを防ぎやすい <see cref="LockAsync"/> の使用を推奨します。
    /// </remarks>
    public async Task WaitAsync () {
        await this.m_semaphore.WaitAsync();
    }
    private static readonly Func<Task, object?, IDisposable> ContinuationFunc = ((_, o) => (IDisposable)o);
    /// <summary>
    /// ロックを非同期で取得し、解放用のハンドルを返します。
    /// </summary>
    /// <returns>
    /// <c>Dispose</c> 時にロックを解放する <see cref="IDisposable"/>。
    /// </returns>
    /// <remarks>
    /// 返却値は必ず <c>using</c> または <c>try/finally</c> で破棄してください。
    /// 破棄されない場合、後続処理が永続的に待機する可能性があります。
    /// </remarks>
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
/// <summary>
/// 非同期処理同士の排他制御を行うための軽量ロックです。
/// </summary>
/// <remarks>
/// <para>
/// <see cref="LockAsync"/> でロックを取得し、返却される <see cref="IDisposable"/> を
/// <c>using</c> で破棄することでロックを解放します。
/// </para>
/// <para>
/// 使用例:
/// <code>
/// private readonly AsyncLock _lock = new AsyncLock();
///
/// public async Task UpdateAsync()
/// {
///     using (await _lock.LockAsync())
///     {
///         await DoSomethingAsync();
///     }
/// }
/// </code>
/// </para>
/// <para>
/// 同じインスタンスに対してネストしてロック取得すると、自分自身の解放待ちになります。
/// 再入可能なロックではないため、同一フローからの二重取得は避けてください。
/// </para>
/// </remarks>
public class AsyncLock {
    private readonly Channel<bool> _channel;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AsyncLock() {
        // 単一アイテムのバッファで、ロックトークンの排他制御
        _channel = Channel.CreateBounded<bool>(1);
        _channel.Writer.TryWrite(true); // 最初にロック権を入れておく
    }

    /// <summary>
    /// ロックを非同期で取得し、解放用のハンドルを返します。
    /// </summary>
    /// <returns>
    /// <c>Dispose</c> 時にロックを解放する <see cref="IDisposable"/>。
    /// </returns>
    /// <remarks>
    /// 返却値は必ず <c>using</c> または <c>try/finally</c> で破棄してください。
    /// 破棄されない場合、後続処理が永続的に待機する可能性があります。
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<IDisposable> LockAsync() {
        // ロック権を取得（チャネルから読み取る）
        await _channel.Reader.ReadAsync();
        return new Releaser(_channel.Writer);
    }

    private readonly struct Releaser : IDisposable {
        private readonly ChannelWriter<bool> _writer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Releaser(ChannelWriter<bool> writer) {
            _writer = writer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() {
            // ロック解放（チャネルに戻す）
            _writer.TryWrite(true);
        }
    }
}
#endif

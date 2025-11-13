using Medallion.Threading;
using Medallion.Threading.Redis;
using StackExchange.Redis;

namespace Logic.Semaphore;

public class DistributedSemaphore: IDistributedSemaphore
{
    public required string Name { get; set; }
    public required int MaxCount { get; set; }
    private readonly RedisDistributedSemaphore _semaphore = new(new RedisKey());

    
    public IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = new TimeSpan(),
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _semaphore.TryAcquire(timeout, cancellationToken);
    }

    public IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _semaphore.Acquire(timeout, cancellationToken);
    }

    public async ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(TimeSpan timeout = new TimeSpan(),
        CancellationToken cancellationToken = new CancellationToken())
    {
        return await _semaphore.TryAcquireAsync(timeout, cancellationToken);
    }

    public async ValueTask<IDistributedSynchronizationHandle> AcquireAsync(TimeSpan? timeout = null, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        return await _semaphore.AcquireAsync(timeout, cancellationToken);
    }
}
using System.Diagnostics;
using Medallion.Threading;
using StackExchange.Redis;

namespace Logic.Semaphore;

public class RedisDistributedSemaphore : IDistributedSemaphore, IDisposable
{
    public string Name { get; }
    public int MaxCount { get; }
    
    private readonly IDatabase _redisDatabase;
    private readonly string _key;
    private readonly int _maxCount;
    private readonly TimeSpan _expiry;
    private readonly string _lockId;
    private bool _disposed;

    private static readonly string AcquireScript = @"
        local key = KEYS[1]
        local maxCount = tonumber(ARGV[1])
        local lockId = ARGV[2]
        local currentTime = tonumber(ARGV[3])
        local expiry = tonumber(ARGV[4])
        
        redis.call('zremrangebyscore', key, 0, currentTime - 30)
        
        local count = redis.call('zcard', key)
        
        if count < maxCount then
            redis.call('zadd', key, currentTime, lockId)
            redis.call('expire', key, expiry)
            return 1
        else
            return 0
        end
    ";

    private static readonly string ReleaseScript = @"
        local key = KEYS[1]
        local lockId = ARGV[1]
        local expiry = tonumber(ARGV[2])
        
        local removed = redis.call('zrem', key, lockId)
        if redis.call('exists', key) == 1 then
            redis.call('expire', key, expiry)
        end
        return removed
    ";

    private static readonly string InitialScript = @"
        local key = KEYS[1]
        local maxCount = tonumber(ARGV[1])
        local expiry = tonumber(ARGV[2])
        
        if redis.call('exists', key) == 0 then
            for i = 1, maxCount do
                redis.call('zadd', key, 0, 'init_' .. i)
            end
            redis.call('expire', key, expiry)
        end
        return 1
    ";

    public RedisDistributedSemaphore(IDatabase redisDatabase, string name, int maxCount, TimeSpan? expiry = null)
    {
        _redisDatabase = redisDatabase ?? throw new ArgumentNullException(nameof(redisDatabase));
        _key = $"semaphore:{name}";
        _maxCount = maxCount;
        _expiry = expiry ?? TimeSpan.FromSeconds(30);
        _lockId = $"{Environment.MachineName}:{Guid.NewGuid()}";
        Name = name;
        MaxCount = maxCount;

        InitializeSemaphore();
    }

    private void InitializeSemaphore()
    {
        try
        {
            _redisDatabase.ScriptEvaluate(InitialScript,
                [_key],
                [_maxCount, (int)_expiry.TotalSeconds]);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Semaphore initialization error: {ex.Message}");
        }
    }

    public IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = default, 
        CancellationToken cancellationToken = default)
    {
        return TryAcquireAsync(timeout, cancellationToken).Result;
    }

    public IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null, 
        CancellationToken cancellationToken = default)
    {
        var handle = TryAcquire(timeout ?? Timeout.InfiniteTimeSpan, cancellationToken);
        if (handle == null)
        {
            throw new TimeoutException($"Failed to acquire semaphore '{Name}' within {timeout}");
        }
        return handle;
    }

    public async ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(
        TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        if (timeout == TimeSpan.Zero)
        {
            timeout = TimeSpan.FromSeconds(30);
        }
        
        var stopwatch = Stopwatch.StartNew();
        var currentTime = GetUnixTimestamp();
        
        while (!cancellationToken.IsCancellationRequested && stopwatch.Elapsed < timeout)
        {
            try
            {
                var result = (int?)await _redisDatabase.ScriptEvaluateAsync(AcquireScript,
                    [_key],
                    [_maxCount, _lockId, currentTime, (int)_expiry.TotalSeconds]);
                
                if (result == 1)
                {
                    return new RedisSemaphoreHandle(this);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Semaphore acquisition error: {ex.Message}");
            }
            
            if (stopwatch.Elapsed < timeout)
            {
                await Task.Delay(100, cancellationToken);
            }
            currentTime = GetUnixTimestamp();
        }
        
        cancellationToken.ThrowIfCancellationRequested();
        return null;
    }

    public async ValueTask<IDistributedSynchronizationHandle> AcquireAsync(
        TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        var handle = await TryAcquireAsync(timeout ?? Timeout.InfiniteTimeSpan, cancellationToken);
        return handle ?? throw new TimeoutException($"Failed to acquire semaphore '{Name}' within {timeout}");
    }

    public async Task ReleaseAsync()
    {
        if (!_disposed)
        {
            try
            {
                await _redisDatabase.ScriptEvaluateAsync(ReleaseScript,
                    [_key],
                    [_lockId, (int)_expiry.TotalSeconds]);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Semaphore release error: {ex.Message}");
            }
        }
    }
    
    private static long GetUnixTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public void Dispose()
    {
        _disposed = true;
    }
}

public class RedisSemaphoreHandle(RedisDistributedSemaphore semaphore) : IDistributedSynchronizationHandle
{
    private bool _disposed;

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _ = semaphore.ReleaseAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _disposed = true;
            await semaphore.ReleaseAsync();
        }
    }

    public CancellationToken HandleLostToken => CancellationToken.None;
}
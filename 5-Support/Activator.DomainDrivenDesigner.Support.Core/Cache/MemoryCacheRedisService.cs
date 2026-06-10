using Common.Core.DependencyInjection;
using Common.Core.Shared.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Activator.DomainDrivenDesigner.Support.Core.Cache;

[ServiceLocate(typeof(IMemoryCacheService))]
public class MemoryCacheRedisService : IMemoryCacheService
{
    private readonly IDistributedCache _redisCache;

    public MemoryCacheRedisService(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<(bool Success, T? CachedValue)> GetValue<T>(string cacheKeyUnique, CancellationToken? token)
    {
        var value = await _redisCache.GetStringAsync(cacheKeyUnique, token ?? CancellationToken.None).ConfigureAwait(false);

        if (value == null) return (false, default);

        var cachedValue = JsonSerializer.Deserialize<T>(value);

        if (cachedValue == null) return (false, default);

        return (true, cachedValue);
    }

    public async Task<bool> InsertIfNotExist<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire, CancellationToken? token)
    {
        var value = await _redisCache.GetAsync(cacheKeyUnique, token ?? CancellationToken.None).ConfigureAwait(false);
        if(value is null)
        {
            return await Upsert(cacheKeyUnique, jsonValue, expire, token).ConfigureAwait(false);
        }
        return true;
    }

    public async Task<bool> Upsert<T>(string cacheKeyUnique, T jsonValue, TimeSpan expire, CancellationToken? token)
    {
        await _redisCache.SetStringAsync(cacheKeyUnique, JsonSerializer.Serialize(jsonValue), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expire
        }, token ?? CancellationToken.None).ConfigureAwait(false);
        
        return true;
    }
}

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MusicShop.Application.Common.Interfaces;

namespace MusicShop.Infrastructure.Cache;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, ct);

        if (cachedValue is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        string jsonValue = JsonSerializer.Serialize(value);

        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
        };

        await _distributedCache.SetStringAsync(key, jsonValue, options, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await _distributedCache.RemoveAsync(key, ct);
    }
}

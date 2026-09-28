using Microsoft.Extensions.Caching.Memory;
using NeuroPath.Core.Interfaces;

namespace NeuroPath.Core.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    public CacheService(IMemoryCache cache) => _cache = cache;
    public T? Get<T>(string key) => _cache.TryGetValue(key, out T? v) ? v : default;
    public void Set<T>(string key, T value, TimeSpan expiration) => _cache.Set(key, value, expiration);
    public void Remove(string key) => _cache.Remove(key);
    public bool TryGet<T>(string key, out T? value) => _cache.TryGetValue(key, out value);
}

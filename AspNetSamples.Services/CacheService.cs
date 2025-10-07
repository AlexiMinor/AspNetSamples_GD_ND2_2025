using AspNetSamples.Services.Abstractions;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using MemoryCache = System.Runtime.Caching.MemoryCache;

namespace AspNetSamples.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCacheModern;
    private readonly ObjectCache _memoryCache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(ILogger<CacheService> logger, 
        IMemoryCache memoryCacheModern)
    {
        _memoryCache = MemoryCache.Default;
        _logger = logger;
        _memoryCacheModern = memoryCacheModern;
    }

    public T? Get<T>(string key)
    {
        try
        {
            
            if (_memoryCache.Contains(key))
            {
                var item = (T)_memoryCache.Get(key);
                return item;
            }
            else
            {
                return default(T);
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting item from cache");
            throw;
        }
    }

    public bool Set<T>(string key, T value, DateTimeOffset expirationTime)
    {
        if (string.IsNullOrWhiteSpace(key) || value == null)
        {
            return false;
        }

        try
        {
            _memoryCache.Set(key, value, expirationTime);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while setting item in cache");
            throw;
        }
    }

    public bool Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        try
        {
            _memoryCache.Remove(key);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while setting item in cache");
            throw;
        }
    }
}
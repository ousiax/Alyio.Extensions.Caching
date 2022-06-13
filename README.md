# Alyio.Extensions.Caching

![Build Status](https://github.com/qqbuby/Alyio.Extensions.Caching/actions/workflows/ci.yml/badge.svg?branch=main)

*Alyio.Extensions.Caching* provides extension methods for converting a POCO to an byte array, and set into redis.

```cs
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Alyio.Extensions.Caching;

/// <summary>
/// Extension methods for <see cref="IDistributedCache"/>.
/// </summary>
public static partial class DistributedCacheExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T? Get<T>(this IDistributedCache cache, string key)
    {
        var bytes = cache.Get(key);
        if (bytes == null)
        {
            return default;
        }
        else
        {
            return DeserializeAsync<T>(bytes).Result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        var bytes = await cache.GetAsync(key, token);
        if (bytes == null)
        {
            return default;
        }
        else
        {
            return await DeserializeAsync<T>(bytes);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public static void Set<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = SerializeAsync<T>(value).Result;
        cache.Set(key, bytes, options);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        var bytes = await SerializeAsync<T>(value);
        await cache.SetAsync(key, bytes, options, token);
    }
}
```
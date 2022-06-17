using Microsoft.Extensions.Caching.Distributed;
using static Alyio.Extensions.Caching.JsonDeSerializer;

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
    public static async ValueTask<T?> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        var bytes = await cache.GetAsync(key, token).ConfigureAwait(false);
        if (bytes == null)
        {
            return default;
        }
        else
        {
            return await DeserializeAsync<T>(bytes).ConfigureAwait(false);
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
        var bytes = SerializeAsync(value).Result;
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
    public static async ValueTask SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        var bytes = await SerializeAsync(value).ConfigureAwait(false);
        await cache.SetAsync(key, bytes, options, token).ConfigureAwait(false);
    }
}

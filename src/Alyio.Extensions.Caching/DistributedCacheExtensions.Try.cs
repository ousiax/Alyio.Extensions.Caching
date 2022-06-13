using System;
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
    public static (T? value, string? error) TryGet<T>(this IDistributedCache cache, string key)
    {
        try
        {
            var bytes = cache.Get(key);

            if (bytes != null)
            {
                var value = DeserializeAsync<T>(bytes).Result;
                return (value, null);
            }

            return (default, null);
        }
        catch (Exception ex)
        {
            return (default, ex.Message);
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
    public static async Task<(T? value, string? error)> TryGetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        try
        {
            var bytes = await cache.GetAsync(key, token);
            if (bytes != null)
            {
                var value = await DeserializeAsync<T>(bytes);
                return (value, null);
            }

            return (default, null);
        }
        catch (Exception ex)
        {
            return (default, ex.Message);
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
    public static string? TrySet<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        try
        {
            var bytes = SerializeAsync<T>(value).Result;
            cache.Set(key, bytes, options);

            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
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
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task<string?> TrySetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        try
        {
            var bytes = await SerializeAsync<T>(value);
            await cache.SetAsync(key, bytes, options, token);

            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
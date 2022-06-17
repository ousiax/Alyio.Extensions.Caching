using Microsoft.Extensions.Caching.Distributed;
using static Alyio.Extensions.Caching.DeSerializer;

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
    /// <returns>A tuple (null, error) is returned if some exception occured.</returns>
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
    /// <returns>A tuple (null, error) is returned if some exception occured.</returns>
    public static async ValueTask<(T? value, string? error)> TryGetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        try
        {
            var bytes = await cache.GetAsync(key, token).ConfigureAwait(false);
            if (bytes != null)
            {
                var value = await DeserializeAsync<T>(bytes).ConfigureAwait(false);
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
    /// <returns>An error is returned if value was not set successfully; otherwise null.</returns>
    public static string? TrySet<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        try
        {
            var bytes = SerializeAsync(value).Result;
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
    /// <returns>An error is returned if value was not set successfully; otherwise null.</returns>
    public static async ValueTask<string?> TrySetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        try
        {
            var bytes = await SerializeAsync(value).ConfigureAwait(false);
            await cache.SetAsync(key, bytes, options, token).ConfigureAwait(false);

            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
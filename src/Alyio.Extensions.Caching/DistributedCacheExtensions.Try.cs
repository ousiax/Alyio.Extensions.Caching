using Microsoft.Extensions.Caching.Distributed;
using static Alyio.Extensions.Caching.DeSerializer;

namespace Alyio.Extensions.Caching;

/// <summary>
/// Extension methods for <see cref="IDistributedCache"/>.
/// </summary>
public static partial class DistributedCacheExtensions
{
    /// <summary>
    /// Try to get a value from the specified cache with the specified key. A return value indicates whether the operation succeeded.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
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
    /// Asynchronously try to get a value from the specified cache with the specified key.  A return value indicates whether the operation succeeded.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
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
    /// Try to set a value in the specified cache with the specified key. A return value indicates whether the operation succeeded.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">Optional. The cache options for the entry.</param>
    /// <returns>An error is returned if value was not set successfully; otherwise null.</returns>
    public static string? TrySet<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions? options = default)
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
    /// Asynchronously try to set a value in the specified cache with the specified key. A return value indicates whether the operation succeeded.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">Optional. The cache options for the entry.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>An error is returned if value was not set successfully; otherwise null.</returns>
    public static async ValueTask<string?> TrySetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions? options = default, CancellationToken token = default)
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
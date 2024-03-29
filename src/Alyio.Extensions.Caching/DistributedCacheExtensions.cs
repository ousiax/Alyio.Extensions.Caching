﻿using static Alyio.Extensions.Caching.DeSerializer;

namespace Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Extension methods for <see cref="IDistributedCache"/>.
/// </summary>
public static partial class DistributedCacheExtensions
{
    private static readonly DistributedCacheEntryOptions EmptyOptions = new() { };

    /// <summary>
    /// Gets a value from the specified cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <returns>The value from the stored cache key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
    public static T? Get<T>(this IDistributedCache cache, string? key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        var bytes = cache.Get(key);
        if (bytes == null)
        {
            return default;
        }
        else
        {
            return DeserializeAsync<T>(bytes).AsTask().Result;
        }
    }

    /// <summary>
    /// Asynchronously gets a value from the specified cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that gets the value from the stored cache key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
    public static async ValueTask<T?> GetAsync<T>(this IDistributedCache cache, string? key, CancellationToken token = default)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

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
    ///  Sets a value in the specified cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">Optional. The cache options for the entry.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
    public static void Set<T>(this IDistributedCache cache, string? key, T? value, DistributedCacheEntryOptions? options = default)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var bytes = SerializeAsync(value).AsTask().Result;
        cache.Set(key, bytes, options ?? EmptyOptions);
    }

    /// <summary>
    /// Asynchronously sets a value in the specified cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">Optional. The cache options for the entry.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
    public static async ValueTask SetAsync<T>(this IDistributedCache cache, string? key, T? value, DistributedCacheEntryOptions? options = default, CancellationToken token = default)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var bytes = await SerializeAsync(value).ConfigureAwait(false);
        await cache.SetAsync(key, bytes, options ?? EmptyOptions, token).ConfigureAwait(false);
    }
}

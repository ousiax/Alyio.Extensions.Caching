using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Alyio.Extensions.Caching;

/// <summary>
/// Extension methods for <see cref="IDistributedCache"/>.
/// </summary>
public static partial class DistributedCacheExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };

    private static async Task<byte[]> SerializeAsync<T>(T data)
    {
        using (var ms = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(ms, data, _jsonSerializerOptions);
            return ms.ToArray();
        }
    }

    private static async Task<T?> DeserializeAsync<T>(byte[] bytes)
    {
        return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(bytes), _jsonSerializerOptions);
    }
}
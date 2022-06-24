using System.Text.Json;
using System.Text.Json.Serialization;
using static Alyio.Extensions.Caching.SimpleDeSerializer;

namespace Alyio.Extensions.Caching;

internal static class DeSerializer
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { ReferenceHandler = ReferenceHandler.Preserve };

    public static async ValueTask<byte[]> SerializeAsync<T>(T? data)
    {
        if (data is null)
        {
            return Array.Empty<byte>();
        }

        if (TryGetBytes(data, out var bytes))
        {
            return bytes;
        }

        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, data, _jsonSerializerOptions).ConfigureAwait(false);
        return ms.ToArray();
    }

    public static async ValueTask<T?> DeserializeAsync<T>(byte[] bytes)
    {
        if (TryGetValue<T>(bytes, out var val))
        {
            return val;
        }

        return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(bytes), _jsonSerializerOptions).ConfigureAwait(false);
    }
}

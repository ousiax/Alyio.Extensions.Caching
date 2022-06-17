using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Benchmarks")]

namespace Alyio.Extensions.Caching;

internal static class JsonDeSerializer
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { ReferenceHandler = ReferenceHandler.Preserve };

    public static async ValueTask<byte[]> SerializeAsync<T>(T data)
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, data, _jsonSerializerOptions).ConfigureAwait(false);
        return ms.ToArray();
    }

    public static async ValueTask<T?> DeserializeAsync<T>(byte[] bytes)
    {
        return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(bytes), _jsonSerializerOptions).ConfigureAwait(false);
    }
}

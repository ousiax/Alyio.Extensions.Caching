using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Benchmarks")]

namespace Alyio.Extensions.Caching;

internal static class DeSerializer
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { ReferenceHandler = ReferenceHandler.Preserve };

    public static async ValueTask<byte[]> SerializeAsync<T>(T data)
    {
        if (typeof(T) == typeof(string))
        {
            return Encoding.UTF8.GetBytes((string)Convert.ChangeType(data, typeof(string)));
        }

        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, data, _jsonSerializerOptions).ConfigureAwait(false);
        return ms.ToArray();
    }

    public static async ValueTask<T?> DeserializeAsync<T>(byte[] bytes)
    {
        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(Encoding.UTF8.GetString(bytes), typeof(T));
        }

        return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(bytes), _jsonSerializerOptions).ConfigureAwait(false);
    }
}

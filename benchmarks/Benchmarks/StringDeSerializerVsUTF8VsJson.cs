using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using static Alyio.Extensions.Caching.DeSerializer;

namespace Benchmarks;

public class StringDeSerializerVsUTF8VsJson
{
    private const int N = 10000;
    private readonly string data;

    public StringDeSerializerVsUTF8VsJson()
    {
        var buf = new byte[N];
        new Random(42).NextBytes(buf);
        data = Encoding.UTF8.GetString(buf);
    }

    [Benchmark]
    public string? DeSerializer()
    {
        var result = SerializeAsync(data).AsTask().GetAwaiter().GetResult();
        return DeserializeAsync<string>(result).AsTask().GetAwaiter().GetResult();
    }

    [Benchmark]
    public string UTF8()
    {
        var result = Encoding.UTF8.GetBytes(data);
        return Encoding.UTF8.GetString(result);
    }

    [Benchmark]
    public string JsonUTF8()
    {
        var result = JsonSerializer.SerializeToUtf8Bytes(data, typeof(string));
        return JsonSerializer.Deserialize<string>(new MemoryStream(result))!;
    }
}

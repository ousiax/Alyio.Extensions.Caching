using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;
using static Alyio.Extensions.Caching.JsonDeSerializer;

BenchmarkRunner.Run<JsonDeSerializerVsUTF8>();
// Console.WriteLine(new JsonDeSerializerVsUTF8().JsonDeSerializer());

public class JsonDeSerializerVsUTF8
{
    private const int N = 10000;
    private readonly string data;

    public JsonDeSerializerVsUTF8()
    {
        var buf = new byte[N];
        new Random(42).NextBytes(buf);
        data = Encoding.UTF8.GetString(buf);
    }

    [Benchmark]
    public string? JsonDeSerializer()
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
}
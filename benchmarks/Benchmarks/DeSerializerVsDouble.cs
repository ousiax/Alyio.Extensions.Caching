using BenchmarkDotNet.Attributes;
using System.Text;
using static Alyio.Extensions.Caching.DeSerializer;

namespace Benchmarks;

public class DeSerializerVsDouble
{
    private readonly double data;

    public DeSerializerVsDouble()
    {
        data = new Random(53).NextDouble();
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
        var result = Encoding.UTF8.GetBytes(data.ToString());
        return Encoding.UTF8.GetString(result);
    }
}

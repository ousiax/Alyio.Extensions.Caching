using System.Globalization;
using System.Text;
using System.Text.Json;

using BenchmarkDotNet.Attributes;

using static Alyio.Extensions.Caching.DeSerializer;

namespace Benchmarks;

public class DoubleDeSerializerVsUTF8VsJson
{
    private readonly double _data;

    public DoubleDeSerializerVsUTF8VsJson()
    {
        _data = new Random(53).NextDouble();
    }

    [Benchmark]
    public double? DeSerializer()
    {
        var result = SerializeAsync(_data).AsTask().GetAwaiter().GetResult();
        return DeserializeAsync<double>(result).AsTask().GetAwaiter().GetResult();
    }

    [Benchmark]
    public double UTF8()
    {
        var result = Encoding.UTF8.GetBytes(Convert.ToString(_data, CultureInfo.InvariantCulture));
        return Convert.ToDouble(Encoding.UTF8.GetString(result), CultureInfo.InvariantCulture);
    }

    [Benchmark]
    public double JsonUTF8()
    {
        var result = JsonSerializer.SerializeToUtf8Bytes(_data, typeof(double));
        return JsonSerializer.Deserialize<double>(new MemoryStream(result));
    }
}

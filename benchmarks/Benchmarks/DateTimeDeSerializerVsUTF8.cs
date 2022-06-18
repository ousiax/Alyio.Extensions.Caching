using System.Text;
using BenchmarkDotNet.Attributes;
using static Alyio.Extensions.Caching.DeSerializer;

namespace Benchmarks;

public class DateTimeDeSerializerVsUTF8
{
    private readonly DateTime data;

    public DateTimeDeSerializerVsUTF8()
    {
        data = DateTime.MaxValue;
    }

    [Benchmark]
    public DateTime? DeSerializer()
    {
        var result = SerializeAsync(data).AsTask().GetAwaiter().GetResult();
        return DeserializeAsync<DateTime>(result).AsTask().GetAwaiter().GetResult();
    }

    [Benchmark]
    public DateTime UTF8()
    {
        var result = Encoding.UTF8.GetBytes(data.ToString());
        return Convert.ToDateTime(Encoding.UTF8.GetString(result));
    }
}

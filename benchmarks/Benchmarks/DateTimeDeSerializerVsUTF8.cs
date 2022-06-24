using System.Globalization;
using System.Text;

using BenchmarkDotNet.Attributes;

using static Alyio.Extensions.Caching.DeSerializer;

namespace Benchmarks;

public class DateTimeDeSerializerVsUTF8
{
    private readonly DateTime _data;

    public DateTimeDeSerializerVsUTF8()
    {
        _data = DateTime.MaxValue;
    }

    [Benchmark]
    public DateTime? DeSerializer()
    {
        var result = SerializeAsync(_data).AsTask().GetAwaiter().GetResult();
        return DeserializeAsync<DateTime>(result).AsTask().GetAwaiter().GetResult();
    }

    [Benchmark]
    public DateTime UTF8()
    {
        var result = Encoding.UTF8.GetBytes(_data.ToString(CultureInfo.InvariantCulture));
        return Convert.ToDateTime(Encoding.UTF8.GetString(result), CultureInfo.InvariantCulture);
    }
}

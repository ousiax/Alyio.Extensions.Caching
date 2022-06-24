using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using System.Text;

namespace Benchmarks;

public class SetStringVsGenericSetString
{
    private const int N = 10000;
    private readonly string _data;

    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options = new() { };

    public SetStringVsGenericSetString()
    {
        var buf = new byte[N];
        new Random(42).NextBytes(buf);
        _data = Encoding.UTF8.GetString(buf);

        _cache = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider().GetRequiredService<IDistributedCache>();
    }

    [Benchmark]
    public string? GenericSetString()
    {
        _cache.Set(_data, _data, _options);
        return _cache.Get<string>(_data);
    }

    [Benchmark]
    public string SetString()
    {
        _cache.SetString(_data, _data, _options);
        return _cache.GetString(_data);
    }
}

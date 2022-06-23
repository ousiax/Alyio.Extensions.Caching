using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Benchmarks;

public class SetStringVsGenericSetString
{
    private const int N = 10000;
    private readonly string data;

    private readonly IDistributedCache cache;
    private readonly DistributedCacheEntryOptions options = new DistributedCacheEntryOptions { };

    public SetStringVsGenericSetString()
    {
        var buf = new byte[N];
        new Random(42).NextBytes(buf);
        data = Encoding.UTF8.GetString(buf);

        cache = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider().GetRequiredService<IDistributedCache>();
    }

    [Benchmark]
    public string? GenericSetString()
    {
        cache.Set(data, data, options);
        return cache.Get<string>(data);
    }

    [Benchmark]
    public string SetString()
    {
        cache.SetString(data, data, options);
        return cache.GetString(data);
    }
}

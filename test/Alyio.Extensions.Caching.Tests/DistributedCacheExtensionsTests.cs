using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Alyio.Extensions.Caching;

namespace Alyio.DistributedCacheExtensions.Json.Tests;

public class DistributedCacheExtensionsTests
{

    [Fact]
    public async Task Test_Set_Get_Async()
    {

        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var obj = new CacheObj
        {
            MyProperty1 = 10,
            MyProperty2 = "MyProperty2",
            MyProperty3 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } }
        };
        var cache = services.GetRequiredService<IDistributedCache>();
        await cache.SetAsync<CacheObj>("key", obj, new DistributedCacheEntryOptions { });
        var cacheObj = await cache.GetAsync<CacheObj>("key");
        Assert.NotNull(cacheObj);
        Assert.Equal(obj.MyProperty1, cacheObj!.MyProperty1);
        Assert.Equal(obj.MyProperty2, cacheObj!.MyProperty2);
        Assert.NotNull(cacheObj.MyProperty3);
        Assert.Equal(obj.MyProperty3.Count, cacheObj.MyProperty3!.Count);
        Assert.Equal(obj.MyProperty3[1], cacheObj.MyProperty3![1]);
        Assert.Equal(obj.MyProperty3[2], cacheObj.MyProperty3![2]);
    }

    private class CacheObj
    {
        public int MyProperty1 { get; set; }

        public string? MyProperty2 { get; set; }

        public IDictionary<int, string>? MyProperty3 { get; set; }
    }
}
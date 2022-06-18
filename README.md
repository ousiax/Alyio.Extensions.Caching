# Alyio.Extensions.Caching

![Build Status](https://github.com/qqbuby/Alyio.Extensions.Caching/actions/workflows/ci.yml/badge.svg?branch=main)

*Alyio.Extensions.Caching* provides extension methods for `IDistributedCache` to serialize a POCO from/to byte array.

```sh
dotnet add package Alyio.Extensions.Caching --version 2.0.0
```

```cs
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Alyio.Extensions.Caching;

var services = new ServiceCollection()
    .AddDistributedMemoryCache()
    .BuildServiceProvider();

var cache = services.GetRequiredService<IDistributedCache>();

cache.Set("string", "value", new DistributedCacheEntryOptions { });
cache.Get<string>("string");

await cache.SetAsync("string", "value", new DistributedCacheEntryOptions { });
await cache.GetAsync<string>("string");

await cache.SetAsync("double", 1_024d, new DistributedCacheEntryOptions { });
await cache.GetAsync<double>("double");

await cache.SetAsync("cacheobj", new CacheObj { MyProperty1 = 1_024 }, new DistributedCacheEntryOptions { });
await cache.GetAsync<CacheObj>("cacheobj");

// ....

class CacheObj
{
    public int MyProperty1 { get; set; }

    public string? MyProperty2 { get; set; }
}
```

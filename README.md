# Alyio.Extensions.Caching

![Build Status](https://github.com/qqbuby/Alyio.Extensions.Caching/actions/workflows/ci.yml/badge.svg?branch=main)

*Alyio.Extensions.Caching* provides extension methods for [IDistributedCache](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.distributed.idistributedcache) to serialize a POCO from/to byte array.

```sh
dotnet add package Alyio.Extensions.Caching --version 2.0.2
```

```cs
using Microsoft.Extensions.Caching.Distributed;

var cache = services.GetRequiredService<IDistributedCache>();

cache.Set("string", "value");
cache.Set("string", "value", new DistributedCacheEntryOptions { });

cache.Get<string>("string");

await cache.SetAsync("string", "value");
await cache.SetAsync("string", "value", new DistributedCacheEntryOptions { });
await cache.GetAsync<string>("string");

cache.Set("double", 1_024d);
cache.Set("double", 1_024d, new DistributedCacheEntryOptions { });
cache.Get<double>("double");

await cache.SetAsync("double", 1_024d);
await cache.SetAsync("double", 1_024d, new DistributedCacheEntryOptions { });
await cache.GetAsync<double>("double");

cache.Set("cacheobj", new CacheObj { MyProperty1 = 1_024 });
cache.Set("cacheobj", new CacheObj { MyProperty1 = 1_024 }, new DistributedCacheEntryOptions { });
cache.Get<CacheObj>("cacheobj");

await cache.SetAsync("cacheobj", new CacheObj { MyProperty1 = 1_024 });
await cache.SetAsync("cacheobj", new CacheObj { MyProperty1 = 1_024 }, new DistributedCacheEntryOptions { });
await cache.GetAsync<CacheObj>("cacheobj");

cache.TrySet("double", 1_024D);
cache.TrySet("double", 1_024D, new DistributedCacheEntryOptions { });
cache.TryGet<double>("double");

await cache.TrySetAsync("double", 1_024D);
await cache.TrySetAsync("double", 1_024D, new DistributedCacheEntryOptions { });
await cache.TryGetAsync<double>("double");

// ....

class CacheObj
{
    public int MyProperty1 { get; set; }

    public string? MyProperty2 { get; set; }
}
```

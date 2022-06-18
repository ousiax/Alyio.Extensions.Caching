using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Alyio.Extensions.Caching.Tests;

public class DistributedCacheExtensionsTests
{
    /// <summary>
    /// https://www.newtonsoft.com/json/help/html/ReferenceLoopHandlingIgnore.htm
    /// </summary>
    public class Employee
    {
        public string? Name { get; set; }
        public Employee? Manager { get; set; }
    }

    [Fact]
    public async Task Test_ReferenceLoopHandling_Async()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();

        var joe = new Employee { Name = "Joe User" };
        var mike = new Employee { Name = "Mike Manager" };
        joe.Manager = mike;
        mike.Manager = mike;

        // Act
        await cache.SetAsync("joe", joe, new DistributedCacheEntryOptions { });
        await cache.SetAsync("mike", mike, new DistributedCacheEntryOptions { });
        var joe2 = await cache.GetAsync<Employee>("joe");
        var mike2 = await cache.GetAsync<Employee>("mike");

        // Assert
        Assert.NotNull(joe2!);
        Assert.NotNull(mike2!);
        Assert.NotNull(joe2!.Manager);
        Assert.NotNull(mike2!.Manager);

        Assert.Equal(mike.Name, mike2!.Manager!.Name);
        Assert.Equal(joe.Manager.Name, joe2!.Manager!.Name);
    }

    [Fact]
    public void Test_GetSet_ReferenceLoopHandling_Sync()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();

        var joe = new Employee { Name = "Joe User" };
        var mike = new Employee { Name = "Mike Manager" };
        joe.Manager = mike;
        mike.Manager = mike;

        // Act
        cache.Set("joe", joe, new DistributedCacheEntryOptions { });
        cache.Set("mike", mike, new DistributedCacheEntryOptions { });
        var joe2 = cache.Get<Employee>("joe");
        var mike2 = cache.Get<Employee>("mike");

        // Assert
        Assert.NotNull(joe2!);
        Assert.NotNull(mike2!);
        Assert.NotNull(joe2!.Manager);
        Assert.NotNull(mike2!.Manager);

        Assert.Equal(mike.Name, mike2!.Manager!.Name);
        Assert.Equal(joe.Manager.Name, joe2!.Manager!.Name);
    }

    [Fact]
    public async Task Test_Get_Set_Async()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var key = "key";
        var val = "hello 世界！";

        await cache.SetAsync(key, val, new DistributedCacheEntryOptions { });
        var actual = await cache.GetAsync<string>(key);

        Assert.Equal(val, actual);
    }

    [Fact]
    public void Test_Get_Set_Throw_ArgumentNullException_Sync()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var options = new DistributedCacheEntryOptions { };

        Assert.Throws<ArgumentNullException>(() =>
        {
            cache.Get<string>(null);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            cache.Set<string>(null, string.Empty, options);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            cache.Set<string>(string.Empty, null, options);
        });
    }

    [Fact]
    public async Task Test_Get_Set_Throw_ArgumentNullException_Async()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var options = new DistributedCacheEntryOptions { };

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await cache.GetAsync<string>(null);
            });

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await cache.SetAsync<string>(null, string.Empty, options);
          });

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
              {
                  await cache.SetAsync<string>(string.Empty, null, options);
              });
    }

    [Fact]
    public void Test_Get_Set_Sync()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var key = "key";
        var val = "hello 世界！";

        cache.Set(key, val, new DistributedCacheEntryOptions { });
        var actual = cache.Get<string>(key);

        Assert.Equal(val, actual);
    }

    [Fact]
    public async Task Test_Try_Get_Set_Async()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var key = "key";
        var val = "hello 世界！";

        var err = await cache.TrySetAsync(key, val, new DistributedCacheEntryOptions { });
        var (actual, err2) = await cache.TryGetAsync<string>(key);

        Assert.Null(err);
        Assert.Null(err2);
        Assert.Equal(val, actual);
    }

    [Fact]
    public void Test_Try_Get_Set_Sync()
    {
        // Arrange
        using var services = new ServiceCollection().AddDistributedMemoryCache().BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var key = "key";
        var val = "hello 世界！";

        var err = cache.TrySet(key, val, new DistributedCacheEntryOptions { });
        var (actual, err2) = cache.TryGet<string>(key);

        Assert.Null(err);
        Assert.Null(err2);
        Assert.Equal(val, actual);
    }

    [Fact]
    public async Task Test_Try_Get_Set_with_error_Async()
    {
        // Arrange
        using var services = new ServiceCollection()
            .AddSingleton<IDistributedCache, NotImplementedDistributedCache>()
            .BuildServiceProvider();
        var cache = services.GetRequiredService<IDistributedCache>();
        var key = "key";
        var val = "hello 世界！";

        var err = await cache.TrySetAsync(key, val, new DistributedCacheEntryOptions { });
        var (actual, err2) = await cache.TryGetAsync<string>(key);

        Assert.NotNull(err);
        Assert.NotNull(err2);
        Assert.Null(actual);
    }
}
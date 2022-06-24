using static Alyio.Extensions.Caching.DeSerializer;

namespace Alyio.Extensions.Caching.Tests;

public class DeSerializerTests
{
    [Theory]
    [InlineData("Hello, 世界!")]
    [InlineData("")]
    public async Task TestDeSerStringAync(string data)
    {
        var bytes = await SerializeAsync(data);
        var val = await DeserializeAsync<string>(bytes);
        Assert.Equal(data, val);
    }

    [Fact]
    public async Task TestDeSerNullStringAsync()
    {
        string data = null!;
        var bytes = await SerializeAsync(data);

        Assert.NotNull(bytes);

        var val = await DeserializeAsync<string>(bytes);
        Assert.Equal(string.Empty, val);
    }
}

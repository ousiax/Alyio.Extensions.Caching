using static Alyio.Extensions.Caching.SimpleDeSerializer;

namespace Alyio.Extensions.Caching.Tests;

public class SimpleDeSerializerTests
{
    [Theory]
    [InlineData(default(bool))]
    [InlineData(default(char))]
    [InlineData(default(double))]
    [InlineData(default(short))]
    [InlineData(default(int))]
    [InlineData(default(long))]
    [InlineData(default(float))]
    [InlineData(default(ushort))]
    [InlineData(default(uint))]
    [InlineData(default(ulong))]
    public void TestDeSer<T>(T data)
    {
        Assert.True(TryGetBytes(data, out var bytes));
        Assert.True(TryGetValue(bytes!, out T? val));
        Assert.Equal(data, val);
    }

    [Fact]
    public void TestDeSerDecimal()
    {
        var datas = new[] { decimal.Zero, decimal.MinValue, decimal.MaxValue };

        foreach (var data in datas)
        {
            Assert.True(TryGetBytes(data, out var bytes));
            Assert.True(TryGetValue<decimal>(bytes!, out var val));
            Assert.Equal(data, val);
        }
    }

    [Fact]
    public void TestDeSerDouble()
    {
        var datas = new[] { double.MinValue, 0D, double.MaxValue };

        foreach (var data in datas)
        {
            Assert.True(TryGetBytes(data, out var bytes));
            Assert.True(TryGetValue<double>(bytes!, out var val));
            Assert.Equal(data, val);
        }
    }

    [Fact]
    public void TestDeSerDateTime()
    {
        var datas = new[] { DateTime.MinValue, DateTime.Now, DateTime.UtcNow, DateTime.UnixEpoch, DateTime.MaxValue };

        foreach (var data in datas)
        {
            Assert.True(TryGetBytes(data, out var bytes));
            Assert.True(TryGetValue<DateTime>(bytes!, out var val));
            Assert.Equal(data, val);
        }
    }

    [Fact]
    public void TestDeSerNotSupported()
    {
        var datas = new object[] { byte.MaxValue, IntPtr.Zero, UIntPtr.Zero, new { A = 3 }, DateTime.UtcNow };

        foreach (var data in datas)
        {
            Assert.False(TryGetBytes(data, out _));
        }
    }
}

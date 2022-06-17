using System.Globalization;
using System.Text;

namespace Alyio.Extensions.Caching;

/// <summary>
/// The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
/// </summary>
internal static class SimpleDeSerializer
{
    public static bool TryGetBytes<T>(T data, out byte[] bytes)
    {
        if (typeof(T) == typeof(string))
        {
            bytes = Encoding.UTF8.GetBytes((string)Convert.ChangeType(data, typeof(string)));
            return true;
        }

        try
        {
            var result = Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte or
                TypeCode.SByte or
                TypeCode.Boolean or
                TypeCode.Char or
                TypeCode.Decimal or
                TypeCode.Double or
                TypeCode.Int16 or
                TypeCode.Int32 or
                TypeCode.Int64 or
                TypeCode.Single or
                TypeCode.UInt16 or
                TypeCode.UInt32 or
                TypeCode.UInt64 => Convert.ToString(data, CultureInfo.InvariantCulture),
                _ => throw new NotSupportedException()
            };
            bytes = Encoding.UTF8.GetBytes(result);
            return true;
        }
        catch (NotSupportedException)
        {
            bytes = new byte[] { };
            return false;
        }
    }

    public static bool TryGetValue<T>(byte[] bytes, out T? val)
    {
        if (typeof(T) == typeof(string))
        {
            val = (T)Convert.ChangeType(Encoding.UTF8.GetString(bytes), typeof(T));
            return true;
        }

        try
        {
            var strVal = Encoding.UTF8.GetString(bytes);
            var result = Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte => Convert.ChangeType(Convert.ToByte(strVal, CultureInfo.InvariantCulture), typeof(T)),
                TypeCode.SByte => Convert.ToSByte(strVal, CultureInfo.InvariantCulture),
                TypeCode.Boolean => Convert.ToBoolean(strVal, CultureInfo.InvariantCulture),
                TypeCode.Char => Convert.ToChar(strVal, CultureInfo.InvariantCulture),
                TypeCode.Decimal => Convert.ToDecimal(strVal, CultureInfo.InvariantCulture),
                TypeCode.Double => Convert.ToDouble(strVal, CultureInfo.InvariantCulture),
                TypeCode.Int16 => Convert.ToInt16(strVal, CultureInfo.InvariantCulture),
                TypeCode.Int32 => Convert.ToInt32(strVal, CultureInfo.InvariantCulture),
                TypeCode.Int64 => Convert.ToInt64(strVal, CultureInfo.InvariantCulture),
                TypeCode.Single => Convert.ToSingle(strVal, CultureInfo.InvariantCulture),
                TypeCode.UInt16 => Convert.ToUInt16(strVal, CultureInfo.InvariantCulture),
                TypeCode.UInt32 => Convert.ToUInt32(strVal, CultureInfo.InvariantCulture),
                TypeCode.UInt64 => Convert.ToUInt64(strVal, CultureInfo.InvariantCulture),
                _ => throw new NotSupportedException()
            };
            val = (T)result;
            return true;
        }
        catch (NotSupportedException)
        {
            val = default;
            return false;
        }
    }
}

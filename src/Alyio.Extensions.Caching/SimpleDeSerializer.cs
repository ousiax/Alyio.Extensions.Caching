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
        try
        {
            bytes = Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.String => Encoding.UTF8.GetBytes((string)Convert.ChangeType(data, typeof(string))),

                TypeCode.Boolean => BitConverter.GetBytes((bool)Convert.ChangeType(data, TypeCode.Boolean)),

                TypeCode.Char => BitConverter.GetBytes((char)Convert.ChangeType(data, TypeCode.Char)),

                TypeCode.Int16 => BitConverter.GetBytes((short)Convert.ChangeType(data, TypeCode.Int16)),
                TypeCode.Int32 => BitConverter.GetBytes((int)Convert.ChangeType(data, TypeCode.Int32)),
                TypeCode.Int64 => BitConverter.GetBytes((long)Convert.ChangeType(data, TypeCode.Int64)),

                TypeCode.UInt16 => BitConverter.GetBytes((ushort)Convert.ChangeType(data, TypeCode.UInt16)),
                TypeCode.UInt32 => BitConverter.GetBytes((uint)Convert.ChangeType(data, TypeCode.UInt32)),
                TypeCode.UInt64 => BitConverter.GetBytes((ulong)Convert.ChangeType(data, TypeCode.UInt64)),

                TypeCode.Single => BitConverter.GetBytes((float)Convert.ChangeType(data, TypeCode.Single)),
                TypeCode.Double => BitConverter.GetBytes((double)Convert.ChangeType(data, TypeCode.Double)),

                TypeCode.Decimal => Encoding.UTF8.GetBytes(Convert.ToString(data, CultureInfo.InvariantCulture)),

                TypeCode.DateTime => BitConverter.GetBytes(((DateTime)Convert.ChangeType(data, TypeCode.DateTime)).ToBinary()),

                _ => throw new NotSupportedException()
            };
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
        try
        {
            var result = Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.String => Convert.ChangeType(Encoding.UTF8.GetString(bytes), typeof(T)),

                TypeCode.Boolean => Convert.ChangeType(BitConverter.ToBoolean(bytes, 0), typeof(T)),

                TypeCode.Char => Convert.ChangeType(BitConverter.ToChar(bytes, 0), typeof(T)),

                TypeCode.Int16 => Convert.ChangeType(BitConverter.ToInt16(bytes, 0), typeof(T)),
                TypeCode.Int32 => Convert.ChangeType(BitConverter.ToInt32(bytes, 0), typeof(T)),
                TypeCode.Int64 => Convert.ChangeType(BitConverter.ToInt64(bytes, 0), typeof(T)),

                TypeCode.UInt16 => Convert.ChangeType(BitConverter.ToUInt16(bytes, 0), typeof(T)),
                TypeCode.UInt32 => Convert.ChangeType(BitConverter.ToUInt32(bytes, 0), typeof(T)),
                TypeCode.UInt64 => Convert.ChangeType(BitConverter.ToUInt64(bytes, 0), typeof(T)),

                TypeCode.Single => Convert.ChangeType(BitConverter.ToSingle(bytes, 0), typeof(T)),
                TypeCode.Double => Convert.ChangeType(BitConverter.ToDouble(bytes, 0), typeof(T)),

                TypeCode.Decimal => Convert.ToDecimal(Encoding.UTF8.GetString(bytes), CultureInfo.InvariantCulture),

                TypeCode.DateTime => DateTime.FromBinary(BitConverter.ToInt64(bytes, 0)),

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

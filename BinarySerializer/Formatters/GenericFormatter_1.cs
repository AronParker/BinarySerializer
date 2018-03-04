using System.Runtime.CompilerServices;
using BinarySerializer.Formatters.Arrays;
using BinarySerializer.Formatters.Enums;
using BinarySerializer.Formatters.Objects;
using BinarySerializer.Formatters.Primitives;
using BinarySerializer.Formatters.Primitives.Arrays;
using BinarySerializer.Formatters.Unions;

namespace BinarySerializer.Formatters
{
    internal static class GenericFormatter<T>
    {
        public static IFormatter<T> CachedInstance = Create();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IFormatter<T> Create()
        {
            if (typeof(T) == typeof(sbyte))
                return (IFormatter<T>)(object)new SByteFormatter();

            if (typeof(T) == typeof(byte))
                return (IFormatter<T>)(object)new ByteFormatter();

            if (typeof(T) == typeof(short))
                return (IFormatter<T>)(object)new Int16Formatter();

            if (typeof(T) == typeof(ushort))
                return (IFormatter<T>)(object)new UInt16Formatter();

            if (typeof(T) == typeof(int))
                return (IFormatter<T>)(object)new Int32Formatter();

            if (typeof(T) == typeof(uint))
                return (IFormatter<T>)(object)new UInt32Formatter();

            if (typeof(T) == typeof(long))
                return (IFormatter<T>)(object)new Int64Formatter();

            if (typeof(T) == typeof(ulong))
                return (IFormatter<T>)(object)new UInt64Formatter();

            if (typeof(T) == typeof(bool))
                return (IFormatter<T>)(object)new BooleanFormatter();

            if (typeof(T) == typeof(char))
                return (IFormatter<T>)(object)new CharFormatter();

            if (typeof(T) == typeof(float))
                return (IFormatter<T>)(object)new SingleFormatter();

            if (typeof(T) == typeof(double))
                return (IFormatter<T>)(object)new DoubleFormatter();

            if (typeof(T) == typeof(string))
                return (IFormatter<T>)(object)new StringFormatter();
            
            if (typeof(T) == typeof(sbyte[]))
                return (IFormatter<T>)(object)new SByteArrayFormatter();

            if (typeof(T) == typeof(byte[]))
                return (IFormatter<T>)(object)new ByteArrayFormatter();

            if (typeof(T) == typeof(short[]))
                return (IFormatter<T>)(object)new Int16ArrayFormatter();

            if (typeof(T) == typeof(ushort[]))
                return (IFormatter<T>)(object)new UInt16ArrayFormatter();

            if (typeof(T) == typeof(int[]))
                return (IFormatter<T>)(object)new Int32ArrayFormatter();

            if (typeof(T) == typeof(uint[]))
                return (IFormatter<T>)(object)new UInt32ArrayFormatter();

            if (typeof(T) == typeof(long[]))
                return (IFormatter<T>)(object)new Int64ArrayFormatter();

            if (typeof(T) == typeof(ulong[]))
                return (IFormatter<T>)(object)new UInt64ArrayFormatter();

            if (typeof(T) == typeof(bool[]))
                return (IFormatter<T>)(object)new BooleanArrayFormatter();

            if (typeof(T) == typeof(char[]))
                return (IFormatter<T>)(object)new CharArrayFormatter();

            if (typeof(T) == typeof(float[]))
                return (IFormatter<T>)(object)new SingleArrayFormatter();

            if (typeof(T) == typeof(double[]))
                return (IFormatter<T>)(object)new DoubleArrayFormatter();

            if (typeof(T) == typeof(string[]))
                return (IFormatter<T>)(object)new StringArrayFormatter();

            if (typeof(T).IsEnum)
                return EnumFormatter.Create<T>();

            if (typeof(T).IsArray)
                return ArrayFormatter.Create<T>();

            if (typeof(T).IsAbstract)
                return UnionFormatter.Create<T>();

            return ObjectFormatter.Create<T>();
        }
    }
}

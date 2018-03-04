using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BinarySerializer.Formatters.Primitives
{
    public static class PrimitiveTypes
    {
        public static PrimitiveType GetPrimitiveType(Type type)
        {
            if (type == typeof(sbyte))
                return PrimitiveType.SByte;
            if (type == typeof(byte))
                return PrimitiveType.Byte;
            if (type == typeof(short))
                return PrimitiveType.Int16;
            if (type == typeof(ushort))
                return PrimitiveType.UInt16;
            if (type == typeof(int))
                return PrimitiveType.Int32;
            if (type == typeof(uint))
                return PrimitiveType.UInt32;
            if (type == typeof(long))
                return PrimitiveType.Int64;
            if (type == typeof(ulong))
                return PrimitiveType.UInt64;
            if (type == typeof(bool))
                return PrimitiveType.Boolean;
            if (type == typeof(char))
                return PrimitiveType.Char;
            if (type == typeof(float))
                return PrimitiveType.Single;
            if (type == typeof(double))
                return PrimitiveType.Double;
            
            return PrimitiveType.NotPrimitive;
        }
    }
}
using System;
using System.Diagnostics;
using System.Reflection.Emit;
using BinarySerializer.Extensions;

namespace BinarySerializer.Formatters.Enums
{
    internal static class EnumFormatter
    {
        public static IFormatter<T> Create<T>()
        {
            Debug.Assert(typeof(T).IsEnum);

            var underlyingType = typeof(T).GetEnumUnderlyingType();

            var getSizeFunc = CreateEnumGetSizeFunc<T>(underlyingType);
            var serializationFunc = CreateEnumSerializationFunc<T>(underlyingType);
            var deserializationFunc = CreateEnumDeserializationFunc<T>(underlyingType);
            
            return new FuncFormatter<T>(getSizeFunc, serializationFunc, deserializationFunc);
        }

        private static GetSizeFunc<T> CreateEnumGetSizeFunc<T>(Type underlyingType)
        {
            var getSizeFunc = new DynamicMethod(string.Empty, typeof(int), new[] { typeof(T), typeof(int), typeof(int) }, true);
            var il = getSizeFunc.GetILGenerator();

            il.EmitLoadArgument(0);

            if (underlyingType == typeof(sbyte))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetSByteSize), BindingFlagsEx.Static));
            else if (underlyingType == typeof(byte))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetByteSize), BindingFlagsEx.Static));
            else if (underlyingType == typeof(short))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetInt16Size), BindingFlagsEx.Static));
            else if (underlyingType == typeof(ushort))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetUInt16Size), BindingFlagsEx.Static));
            else if (underlyingType == typeof(int))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedInt32Size), BindingFlagsEx.Static));
            else if (underlyingType == typeof(uint))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedUInt32Size), BindingFlagsEx.Static));
            else if (underlyingType == typeof(long))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedInt64Size), BindingFlagsEx.Static));
            else if (underlyingType == typeof(ulong))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedUInt64Size), BindingFlagsEx.Static));
            else
                Debug.Assert(false);

            il.Emit(OpCodes.Ret);

            return (GetSizeFunc<T>)getSizeFunc.CreateDelegate(typeof(GetSizeFunc<T>));
        }

        private static SerializationFunc<T> CreateEnumSerializationFunc<T>(Type underlyingType)
        {
            var serialzationFunc = new DynamicMethod(string.Empty, typeof(int), new[] { typeof(T), typeof(byte[]), typeof(int), typeof(int), typeof(int), typeof(int) }, true);
            var il = serialzationFunc.GetILGenerator();

            il.EmitLoadArgument(0);
            il.EmitLoadArgument(1);
            il.EmitLoadArgument(2);
            il.EmitLoadArgument(3);

            if (underlyingType == typeof(sbyte))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteSByte), BindingFlagsEx.Static));
            else if (underlyingType == typeof(byte))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteByte), BindingFlagsEx.Static));
            else if (underlyingType == typeof(short))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteInt16), BindingFlagsEx.Static));
            else if (underlyingType == typeof(ushort))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteUInt16), BindingFlagsEx.Static));
            else if (underlyingType == typeof(int))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedInt32), BindingFlagsEx.Static));
            else if (underlyingType == typeof(uint))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedUInt32), BindingFlagsEx.Static));
            else if (underlyingType == typeof(long))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedInt64), BindingFlagsEx.Static));
            else if (underlyingType == typeof(ulong))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedUInt64), BindingFlagsEx.Static));
            else
                Debug.Assert(false);

            il.Emit(OpCodes.Ret);

            return (SerializationFunc<T>)serialzationFunc.CreateDelegate(typeof(SerializationFunc<T>));
        }

        private static DeserializationFunc<T> CreateEnumDeserializationFunc<T>(Type underlyingType)
        {
            var deserialzationFunc = new DynamicMethod(string.Empty, typeof(T), new[] { typeof(byte[]), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int).MakeByRefType() }, true);
            var il = deserialzationFunc.GetILGenerator();

            il.EmitLoadArgument(0);
            il.EmitLoadArgument(1);
            il.EmitLoadArgument(2);
            il.EmitLoadArgument(5);

            if (underlyingType == typeof(sbyte))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadSByte), BindingFlagsEx.Static));
            else if (underlyingType == typeof(byte))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadByte), BindingFlagsEx.Static));
            else if (underlyingType == typeof(short))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadInt16), BindingFlagsEx.Static));
            else if (underlyingType == typeof(ushort))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadUInt16), BindingFlagsEx.Static));
            else if (underlyingType == typeof(int))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedInt32), BindingFlagsEx.Static));
            else if (underlyingType == typeof(uint))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedUInt32), BindingFlagsEx.Static));
            else if (underlyingType == typeof(long))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedInt64), BindingFlagsEx.Static));
            else if (underlyingType == typeof(ulong))
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedUInt64), BindingFlagsEx.Static));
            else
                Debug.Assert(false);

            il.Emit(OpCodes.Ret);

            return (DeserializationFunc<T>)deserialzationFunc.CreateDelegate(typeof(DeserializationFunc<T>));
        }
    }
}

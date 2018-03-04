using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using BinarySerializer.Attributes;
using BinarySerializer.Extensions;

namespace BinarySerializer.Formatters.Unions
{
    internal class UnionFormatter
    {
        public static IFormatter<T> Create<T>()
        {
            Debug.Assert(typeof(T).IsAbstract);

            var types = typeof(T).GetCustomAttribute<OneOfAttribute>()?.Types;

            if (!AreSerializableSubTypes<T>(types))
                return null;
            
            var getSizeFunc = CreateUnionGetSizeFunc<T>(types);
            var serializationFunc = CreateUnionSerializationFunc<T>(types);
            var deserializationFunc = CreateUnionDeserializationFunc<T>(types);

            return new FuncFormatter<T>(getSizeFunc, serializationFunc, deserializationFunc);
        }
        
        private static bool AreSerializableSubTypes<T>(Type[] types)
        {
            if (types == null)
                return false;

            foreach (var type in types)
                if (!IsSerializableSubType<T>(type))
                    return false;

            return true;
        }

        private static bool IsSerializableSubType<T>(Type type)
        {
            if (type == null)
                return false;

            if (type == typeof(T))
                return false;

            if (!typeof(T).IsAssignableFrom(type))
                return false;

            return GenericFormatter.IsSerializableType(type);
        }

        private static GetSizeFunc<T> CreateUnionGetSizeFunc<T>(Type[] types)
        {
            var getSizeFunc = new DynamicMethod(string.Empty, typeof(int), new[] { typeof(T), typeof(int), typeof(int) }, true);
            var il = getSizeFunc.GetILGenerator();

            {
                var valueNotNull = il.DefineLabel();
                
                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Brtrue_S, valueNotNull);

                il.EmitLoadDeclaredConstant(0);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedUInt32Size), BindingFlagsEx.Static));
                il.Emit(OpCodes.Ret);

                il.MarkLabel(valueNotNull);
            }
            
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var skipLabel = il.DefineLabel();

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Isinst, type);
                il.Emit(OpCodes.Brfalse_S, skipLabel);

                il.EmitLoadDeclaredConstant(i+1);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedUInt32Size), BindingFlagsEx.Static));
                
                GenericFormatter.EmitLoadCachedInstance(il, type);
                il.EmitLoadArgument(0);
                
                if (type.IsClass)
                {
                    il.Emit(OpCodes.Castclass, type);
                }
                else
                {
                    Debug.Assert(type.IsValueType);
                    il.Emit(OpCodes.Unbox_Any, type);
                }
                
                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                GenericFormatter.EmitGetSize(il, type);

                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Ret);

                il.MarkLabel(skipLabel);
            }

            il.Emit(OpCodes.Ldstr, "Failed to get the size of the object, because no implementation was found.");
            il.Emit(OpCodes.Ldstr, "value");
            il.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new[] { typeof(string), typeof(string) }));
            il.Emit(OpCodes.Throw);
            
            return (GetSizeFunc<T>)getSizeFunc.CreateDelegate(typeof(GetSizeFunc<T>));
        }

        private static SerializationFunc<T> CreateUnionSerializationFunc<T>(Type[] types)
        {
            var serialzationFunc = new DynamicMethod(string.Empty, typeof(int), new[] { typeof(T), typeof(byte[]), typeof(int), typeof(int), typeof(int), typeof(int) }, true);
            var il = serialzationFunc.GetILGenerator();

            var indexBytesWritten = il.DeclareLocal(typeof(int));
            
            {
                var valueNotNull = il.DefineLabel();

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Brtrue_S, valueNotNull);
                
                il.EmitLoadDeclaredConstant(0);
                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                il.EmitLoadArgument(3);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedUInt32), BindingFlagsEx.Static));
                il.Emit(OpCodes.Ret);

                il.MarkLabel(valueNotNull);
            }
            
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var skipLabel = il.DefineLabel();

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Isinst, type);
                il.Emit(OpCodes.Brfalse_S, skipLabel);

                il.EmitLoadDeclaredConstant(i + 1);
                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                il.EmitLoadArgument(3);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedUInt32), BindingFlagsEx.Static));
                il.EmitStoreLocal(indexBytesWritten.LocalIndex);
                
                GenericFormatter.EmitLoadCachedInstance(il, type);
                il.EmitLoadArgument(0);

                if (type.IsClass)
                {
                    il.Emit(OpCodes.Castclass, type);
                }
                else
                {
                    Debug.Assert(type.IsValueType);
                    il.Emit(OpCodes.Unbox_Any, type);
                }

                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                il.EmitLoadLocal(indexBytesWritten.LocalIndex);
                il.Emit(OpCodes.Add);
                il.EmitLoadArgument(3);
                il.EmitLoadLocal(indexBytesWritten.LocalIndex);
                il.Emit(OpCodes.Sub);
                il.EmitLoadArgument(4);
                il.EmitLoadArgument(5);
                GenericFormatter.EmitSerialize(il, type);
                il.EmitLoadLocal(indexBytesWritten.LocalIndex);
                il.Emit(OpCodes.Add);
                il.Emit(OpCodes.Ret);

                il.MarkLabel(skipLabel);
            }

            il.Emit(OpCodes.Ldstr, "Failed to serialize the object, because no implementation was found.");
            il.Emit(OpCodes.Ldstr, "value");
            il.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new[] { typeof(string), typeof(string) }));
            il.Emit(OpCodes.Throw);

            return (SerializationFunc<T>)serialzationFunc.CreateDelegate(typeof(SerializationFunc<T>));
        }

        private static DeserializationFunc<T> CreateUnionDeserializationFunc<T>(Type[] types)
        {
            var deserialzationFunc = new DynamicMethod(string.Empty, typeof(T), new[] { typeof(byte[]), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int).MakeByRefType() }, true);
            var il = deserialzationFunc.GetILGenerator();

            var index = il.DeclareLocal(typeof(int));
            var indexSize = il.DeclareLocal(typeof(int));
            var objectSize = il.DeclareLocal(typeof(int));
            var result = il.DeclareLocal(typeof(T));
            
            il.EmitLoadArgument(0);
            il.EmitLoadArgument(1);
            il.EmitLoadArgument(2);
            il.EmitLoadLocalAddress(indexSize.LocalIndex);
            il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedInt32), BindingFlagsEx.Static));
            il.EmitStoreLocal(index.LocalIndex);

            {
                var validIndex = il.DefineLabel();
                
                il.EmitLoadLocal(index.LocalIndex);
                il.EmitLoadDeclaredConstant(types.Length);
                il.Emit(OpCodes.Ble_S, validIndex);
                il.Emit(OpCodes.Ldstr, "Failed to deserialize the object, because its union index was out of bounds.");
                il.Emit(OpCodes.Newobj, typeof(SerializationException).GetConstructor(new[] { typeof(string) }));
                il.Emit(OpCodes.Throw);

                il.MarkLabel(validIndex);
            }

            {
                var valueNotNull = il.DefineLabel();

                il.EmitLoadLocal(result.LocalIndex);
                il.Emit(OpCodes.Brtrue_S, valueNotNull);

                il.EmitLoadArgument(5);
                il.EmitLoadLocal(indexSize.LocalIndex);
                il.Emit(OpCodes.Stind_I4);

                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ret);

                il.MarkLabel(valueNotNull);
            }
            
            il.EmitLoadArgument(1);
            il.EmitLoadLocal(indexSize.LocalIndex);
            il.Emit(OpCodes.Add);
            il.EmitStoreArgument(1);

            il.EmitLoadArgument(2);
            il.EmitLoadLocal(indexSize.LocalIndex);
            il.Emit(OpCodes.Sub);
            il.EmitStoreArgument(2);

            il.EmitLoadLocal(index.LocalIndex);
            il.EmitLoadDeclaredConstant(1);
            il.Emit(OpCodes.Sub);
            il.EmitStoreLocal(index.LocalIndex);

            var jumpTable = new Label[types.Length];
            var endOfSwitch = il.DefineLabel();

            for (var i = 0; i < types.Length; i++)
                jumpTable[i] = il.DefineLabel();

            il.EmitLoadLocal(index.LocalIndex);
            il.Emit(OpCodes.Switch, jumpTable);

            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];

                il.MarkLabel(jumpTable[i]);
                
                GenericFormatter.EmitLoadCachedInstance(il, type);
                il.EmitLoadArgument(0);
                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                il.EmitLoadArgument(3);
                il.EmitLoadArgument(4);
                il.EmitLoadLocalAddress(objectSize.LocalIndex);
                GenericFormatter.EmitDeserialize(il, type);
                
                if (type.IsValueType)
                    il.Emit(OpCodes.Box, type);

                il.EmitStoreLocal(result.LocalIndex);
                il.Emit(OpCodes.Br, endOfSwitch);
            }

            il.MarkLabel(endOfSwitch);
            
            il.EmitLoadArgument(5);
            il.EmitLoadLocal(indexSize.LocalIndex);
            il.EmitLoadLocal(objectSize.LocalIndex);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stind_I4);

            il.EmitLoadLocal(result.LocalIndex);
            il.Emit(OpCodes.Ret);
            
            return (DeserializationFunc<T>)deserialzationFunc.CreateDelegate(typeof(DeserializationFunc<T>));
        }
    }
}
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using BinarySerializer.Extensions;
using BinarySerializer.Formatters.Primitives;

namespace BinarySerializer.Formatters.Objects
{
    internal class ObjectFormatter
    {
        public static IFormatter<T> Create<T>()
        {
            if (!IsSerializable(typeof(T)))
                return null;

            var objectInfo = ObjectInfo.Create(typeof(T));

            if (objectInfo == null)
                return null;

            var getSizeFunc = CreateObjectGetSizeFunc<T>(objectInfo);
            var serializationFunc = CreateObjectSerializationFunc<T>(objectInfo);
            var deserializationFunc = CreateObjectDeserializationFunc<T>(objectInfo);

            return new FuncFormatter<T>(getSizeFunc, serializationFunc, deserializationFunc);
        }
        
        private static bool IsSerializable(Type type)
        {
            Debug.Assert(!type.IsEnum && !type.IsArray && !type.IsAbstract);

            if (type.IsExplicitLayout)
                return false;
            
            if (type.IsClass && !type.IsCOMObject && type != typeof(Delegate) && !type.IsSubclassOf(typeof(Delegate)))
                return true;
            
            if (type.IsValueType)
                return true;

            return false;
        }

        private static GetSizeFunc<T> CreateObjectGetSizeFunc<T>(ObjectInfo objectInfo)
        {
            var getSizeFunc = new DynamicMethod(string.Empty, typeof(int), new[] { typeof(T), typeof(int), typeof(int) }, true);
            var il = getSizeFunc.GetILGenerator();

            var totalSize = il.DeclareLocal(typeof(int));

            {
                var recursionLimitNotReached = il.DefineLabel();

                il.EmitLoadArgument(2);
                il.EmitLoadDeclaredConstant(0);
                il.Emit(OpCodes.Bgt_S, recursionLimitNotReached);
                il.Emit(OpCodes.Ldstr, "Failed to get the size of the object, because the recursion limit was reached.");
                il.Emit(OpCodes.Ldstr, "value");
                il.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new[] { typeof(string), typeof(string) }));
                il.Emit(OpCodes.Throw);

                il.MarkLabel(recursionLimitNotReached);

                il.EmitLoadArgument(2);
                il.EmitLoadDeclaredConstant(1);
                il.Emit(OpCodes.Sub);
                il.EmitStoreArgument(2);
            }

            if (typeof(T).IsClass)
            {
                var valueNotNull = il.DefineLabel();

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Cgt_Un);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetBooleanSize), BindingFlagsEx.Static));
                il.EmitStoreLocal(totalSize.LocalIndex);

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Brtrue_S, valueNotNull);
                il.EmitLoadLocal(totalSize.LocalIndex);
                il.Emit(OpCodes.Ret);

                il.MarkLabel(valueNotNull);
            }
            else
            {
                il.EmitLoadDeclaredConstant(0);
                il.EmitStoreLocal(totalSize.LocalIndex);
            }

            foreach (var field in objectInfo.ConstructorFields.Concat(objectInfo.Fields))
            {
                il.EmitLoadLocal(totalSize.LocalIndex);
                GetFieldSize<T>(il, field);
                il.Emit(OpCodes.Add);
                il.EmitStoreLocal(totalSize.LocalIndex);
            }

            il.EmitLoadLocal(totalSize.LocalIndex);
            il.Emit(OpCodes.Ret);

            return (GetSizeFunc<T>)getSizeFunc.CreateDelegate(typeof(GetSizeFunc<T>));
        }

        private static SerializationFunc<T> CreateObjectSerializationFunc<T>(ObjectInfo objectInfo)
        {
            var serialzationFunc = new DynamicMethod(string.Empty, typeof(int), new[] { typeof(T), typeof(byte[]), typeof(int), typeof(int), typeof(int), typeof(int) }, true);
            var il = serialzationFunc.GetILGenerator();

            var size = il.DeclareLocal(typeof(int));
            var start = il.DeclareLocal(typeof(int));

            {
                var recursionLimitNotReached = il.DefineLabel();

                il.EmitLoadArgument(5);
                il.EmitLoadDeclaredConstant(0);
                il.Emit(OpCodes.Bgt_S, recursionLimitNotReached);
                il.Emit(OpCodes.Ldstr, "Failed to serialize the object, because the recursion limit was reached.");
                il.Emit(OpCodes.Ldstr, "value");
                il.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new[] { typeof(string), typeof(string) }));
                il.Emit(OpCodes.Throw);

                il.MarkLabel(recursionLimitNotReached);

                il.EmitLoadArgument(5);
                il.EmitLoadDeclaredConstant(1);
                il.Emit(OpCodes.Sub);
                il.EmitStoreArgument(5);
            }

            if (typeof(T).IsClass)
            {
                var valueNotNull = il.DefineLabel();

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Cgt_Un);
                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                il.EmitLoadArgument(3);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteBoolean), BindingFlagsEx.Static));
                il.EmitStoreLocal(size.LocalIndex);

                il.EmitLoadArgument(0);
                il.Emit(OpCodes.Brtrue_S, valueNotNull);

                il.EmitLoadLocal(size.LocalIndex);
                il.Emit(OpCodes.Ret);

                il.MarkLabel(valueNotNull);
            }

            il.EmitLoadArgument(2);
            il.EmitStoreLocal(start.LocalIndex);

            if (typeof(T).IsClass)
            {
                IncrementOffset(il, 2, size);
                DecrementCount(il, 3, size);
            }

            foreach (var field in objectInfo.ConstructorFields.Concat(objectInfo.Fields))
            {
                SerializeField<T>(il, field);
                il.EmitStoreLocal(size.LocalIndex);

                IncrementOffset(il, 2, size);
                DecrementCount(il, 3, size);
            }

            il.EmitLoadArgument(2);
            il.EmitLoadLocal(start.LocalIndex);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);

            return (SerializationFunc<T>)serialzationFunc.CreateDelegate(typeof(SerializationFunc<T>));
        }

        private static DeserializationFunc<T> CreateObjectDeserializationFunc<T>(ObjectInfo objectInfo)
        {
            var deserialzationFunc = new DynamicMethod(string.Empty, typeof(T), new[] { typeof(byte[]), typeof(int), typeof(int), typeof(int).MakeByRefType(), typeof(int), typeof(int) }, true);
            var il = deserialzationFunc.GetILGenerator();

            var size = il.DeclareLocal(typeof(int));
            var start = il.DeclareLocal(typeof(int));
            var result = il.DeclareLocal(typeof(T));

            {
                var recursionLimitNotReached = il.DefineLabel();

                il.EmitLoadArgument(4);
                il.EmitLoadDeclaredConstant(0);
                il.Emit(OpCodes.Bgt_S, recursionLimitNotReached);
                il.Emit(OpCodes.Ldstr, "Failed to deserialize the object, because the recursion limit was reached.");
                il.Emit(OpCodes.Newobj, typeof(SerializationException).GetConstructor(new[] { typeof(string) }));
                il.Emit(OpCodes.Throw);

                il.MarkLabel(recursionLimitNotReached);

                il.EmitLoadArgument(4);
                il.EmitLoadDeclaredConstant(1);
                il.Emit(OpCodes.Sub);
                il.EmitStoreArgument(4);
            }

            if (typeof(T).IsClass)
            {
                var valueNotNull = il.DefineLabel();
                
                il.EmitLoadArgument(0);
                il.EmitLoadArgument(1);
                il.EmitLoadArgument(2);
                il.EmitLoadLocalAddress(size.LocalIndex);
                il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadBoolean), BindingFlagsEx.Static));
                il.Emit(OpCodes.Brtrue_S, valueNotNull);

                il.EmitLoadArgument(3);
                il.EmitLoadLocal(size.LocalIndex);
                il.Emit(OpCodes.Stind_I4);

                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ret);

                il.MarkLabel(valueNotNull);

                IncrementOffset(il, 1, size);
                DecrementCount(il, 2, size);
            }
            
            il.EmitLoadArgument(1);
            il.EmitStoreLocal(start.LocalIndex);

            if (objectInfo.Constructor == null)
            {
                Debug.Assert(typeof(T).IsValueType);
                
                il.EmitLoadLocalAddress(result.LocalIndex);
                il.Emit(OpCodes.Initobj, typeof(T));
            }
            else
            {
                if (typeof(T).IsValueType)
                    il.EmitLoadLocalAddress(result.LocalIndex);
                
                foreach (var field in objectInfo.ConstructorFields)
                {
                    DeserializeField(il, field, size);
                    IncrementOffset(il, 1, size);
                    DecrementCount(il, 2, size);
                }
                
                if (typeof(T).IsClass)
                {
                    il.Emit(OpCodes.Newobj, objectInfo.Constructor);
                    il.EmitStoreLocal(result.LocalIndex);
                }
                else
                {
                    Debug.Assert(typeof(T).IsValueType);
                    
                    il.Emit(OpCodes.Call, objectInfo.Constructor);
                }
            }
                
            foreach (var field in objectInfo.Fields)
            {
                il.EmitLoadLocal(result.LocalIndex);
                DeserializeField(il, field, size);
                il.Emit(OpCodes.Stfld, field);

                IncrementOffset(il, 1, size);
                DecrementCount(il, 2, size);
            }

            il.EmitLoadArgument(3);
            il.EmitLoadArgument(1);
            il.EmitLoadLocal(start.LocalIndex);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Stind_I4);

            il.EmitLoadLocal(result.LocalIndex);
            il.Emit(OpCodes.Ret);
            
            return (DeserializationFunc<T>)deserialzationFunc.CreateDelegate(typeof(DeserializationFunc<T>));
        }

        private static void GetFieldSize<T>(ILGenerator il, FieldInfo field)
        {
            var type = PrimitiveTypes.GetPrimitiveType(field.FieldType);
            
            if (type == PrimitiveType.NotPrimitive)
                GenericFormatter.EmitLoadCachedInstance(il, field.FieldType);

            LoadFieldValue<T>(il, field);
            
            switch (type)
            {
                case PrimitiveType.NotPrimitive:
                    il.EmitLoadArgument(1);
                    il.EmitLoadArgument(2);
                    
                    GenericFormatter.EmitGetSize(il, field.FieldType);
                    break;
                case PrimitiveType.SByte:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetSByteSize), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Byte:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetByteSize), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int16:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetInt16Size), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt16:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetUInt16Size), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int32:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedInt32Size), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt32:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedUInt32Size), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int64:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedInt64Size), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt64:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGet7BitEncodedUInt64Size), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Boolean:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetBooleanSize), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Char:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetCharSize), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Single:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetSingleSize), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Double:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalGetDoubleSize), BindingFlagsEx.Static));
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        private static void SerializeField<T>(ILGenerator il, FieldInfo field)
        {
            var type = PrimitiveTypes.GetPrimitiveType(field.FieldType);
            
            if (type == PrimitiveType.NotPrimitive)
                GenericFormatter.EmitLoadCachedInstance(il, field.FieldType);
            
            LoadFieldValue<T>(il, field);
            il.EmitLoadArgument(1);
            il.EmitLoadArgument(2);
            il.EmitLoadArgument(3);

            switch (type)
            {
                case PrimitiveType.NotPrimitive:
                    il.EmitLoadArgument(4);
                    il.EmitLoadArgument(5);

                    GenericFormatter.EmitSerialize(il, field.FieldType);
                    break;
                case PrimitiveType.SByte:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteSByte), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Byte:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteByte), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int16:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteInt16), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt16:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteUInt16), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int32:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedInt32), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt32:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedUInt32), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int64:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedInt64), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt64:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWrite7BitEncodedUInt64), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Boolean:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteBoolean), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Char:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteChar), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Single:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteSingle), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Double:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalWriteDouble), BindingFlagsEx.Static));
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        private static void DeserializeField(ILGenerator il, FieldInfo field, LocalBuilder size)
        {
            var type = PrimitiveTypes.GetPrimitiveType(field.FieldType);
            
            if (type == PrimitiveType.NotPrimitive)
                GenericFormatter.EmitLoadCachedInstance(il, field.FieldType);
            
            il.EmitLoadArgument(0);
            il.EmitLoadArgument(1);
            il.EmitLoadArgument(2);
            il.EmitLoadLocalAddress(size.LocalIndex);

            switch (type)
            {
                case PrimitiveType.NotPrimitive:
                    il.EmitLoadArgument(4);
                    il.EmitLoadArgument(5);
                    GenericFormatter.EmitDeserialize(il, field.FieldType);
                    break;
                case PrimitiveType.SByte:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadSByte), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Byte:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadByte), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int16:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadInt16), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt16:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadUInt16), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int32:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedInt32), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt32:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedUInt32), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Int64:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedInt64), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.UInt64:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalRead7BitEncodedUInt64), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Boolean:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadBoolean), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Char:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadChar), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Single:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadSingle), BindingFlagsEx.Static));
                    break;
                case PrimitiveType.Double:
                    il.Emit(OpCodes.Call, typeof(Binary).GetMethod(nameof(Binary.InternalReadDouble), BindingFlagsEx.Static));
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        private static void LoadFieldValue<T>(ILGenerator il, FieldInfo field)
        {
            if (typeof(T).IsClass)
            {
                il.EmitLoadArgument(0);
            }
            else
            {
                Debug.Assert(typeof(T).IsValueType);
                
                il.EmitLoadArgumentAddress(0);
            }
            
            il.Emit(OpCodes.Ldfld, field);
        }
        
        private static void IncrementOffset(ILGenerator il, int offsetArgumentIndex, LocalBuilder size)
        {
            il.EmitLoadArgument(offsetArgumentIndex);
            il.EmitLoadLocal(size.LocalIndex);
            il.Emit(OpCodes.Add);
            il.EmitStoreArgument(offsetArgumentIndex);
        }

        private static void DecrementCount(ILGenerator il, int countArgumentIndex, LocalBuilder size)
        {
            il.EmitLoadArgument(countArgumentIndex);
            il.EmitLoadLocal(size.LocalIndex);
            il.Emit(OpCodes.Sub);
            il.EmitStoreArgument(countArgumentIndex);
        }
    }
}

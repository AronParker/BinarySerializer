using System;
using System.Diagnostics;
using System.Reflection.Emit;
using BinarySerializer.Extensions;

namespace BinarySerializer.Formatters
{
    internal static class GenericFormatter
    {
        public static bool IsSerializableType(Type type)
        {
            if (type.ContainsGenericParameters)
                return false;

            return typeof(GenericFormatter<>)
                .MakeGenericType(type)
                .GetField(nameof(GenericFormatter<object>.CachedInstance), BindingFlagsEx.Static)
                .GetValue(null) != null;
        }

        public static void EmitLoadCachedInstance(ILGenerator il, Type type)
        {
            Debug.Assert(!type.ContainsGenericParameters);
            
            var genericFormatter = typeof(GenericFormatter<>).MakeGenericType(type);
            var cachedInstance = genericFormatter.GetField(nameof(GenericFormatter<object>.CachedInstance), BindingFlagsEx.Static);
            
            il.Emit(OpCodes.Ldsfld, cachedInstance);
        }

        public static void EmitGetSize(ILGenerator il, Type type)
        {
            il.Emit(OpCodes.Callvirt, typeof(IFormatter<>)
                .MakeGenericType(type)
                .GetMethod(nameof(IFormatter<object>.GetSize)));
        }

        public static void EmitSerialize(ILGenerator il, Type type)
        {
            il.Emit(OpCodes.Callvirt, typeof(IFormatter<>)
                .MakeGenericType(type)
                .GetMethod(nameof(IFormatter<object>.Serialize)));
        }

        public static void EmitDeserialize(ILGenerator il, Type type)
        {
            il.Emit(OpCodes.Callvirt, typeof(IFormatter<>)
                .MakeGenericType(type)
                .GetMethod(nameof(IFormatter<object>.Deserialize)));
        }
    }
}

using System;
using System.Diagnostics;

namespace BinarySerializer.Formatters.Arrays
{
    internal class ArrayFormatter
    {
        public static IFormatter<T> Create<T>()
        {
            Debug.Assert(typeof(T).IsArray);

            var elementType = typeof(T).GetElementType();

            if (!GenericFormatter.IsSerializableType(elementType))
                return null;

            if (typeof(T).GetArrayRank() == 1 && typeof(T) == elementType.MakeArrayType())
                return CreateSZArrayFormatter<T>(elementType);

            return CreateMultidimensionalArrayFormatter<T>(elementType);
        }

        private static IFormatter<T> CreateSZArrayFormatter<T>(Type elementType)
        {
            return (IFormatter<T>)typeof(SZArrayFormatter<>)
                .MakeGenericType(elementType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);
        }

        private static IFormatter<T> CreateMultidimensionalArrayFormatter<T>(Type elementType)
        {
            return (IFormatter<T>)typeof(MultidimensionalArrayFormatter<,>)
                .MakeGenericType(typeof(T), elementType)
                .GetConstructor(new[] { typeof(int) })
                .Invoke(new object[] { typeof(T).GetArrayRank()});
        }
    }
}

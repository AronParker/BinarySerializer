using System;
using System.Runtime.Serialization;

namespace BinarySerializer.Formatters.Arrays
{
    internal class MultidimensionalArrayFormatter<TArray, TElement> : IFormatter<TArray> where TArray : class
    {
        private int _rank;

        public MultidimensionalArrayFormatter(int rank)
        {
            _rank = rank;
        }

        public int GetSize(TArray value, int maxArrayLength, int maxRecursionDepth)
        {
            if (value == null)
                return Binary.InternalGetUInt32Size(0);

            var array = (Array)(object)value;
            var totalSize = 0;

            for (var i = 0; i < _rank; i++)
            {
                var length = array.GetLength(i);

                if (length > maxArrayLength)
                    throw new ArgumentException("Failed to get the size of the array, because one or more dimensions exceeed the maximum array length.", nameof(value));

                if (i == 0)
                    length++;

                totalSize += Binary.InternalGetUInt32Size((uint)length);
            }
            
            if (array.Length > 0)
            {
                var indices = GetIndices(array);
                int indicesIndex;

                do
                {
                    totalSize += GenericFormatter<TElement>.CachedInstance.GetSize((TElement)array.GetValue(indices), maxArrayLength, maxRecursionDepth);

                    for (indicesIndex = _rank - 1; indicesIndex >= 0; indicesIndex--)
                    {
                        indices[indicesIndex]++;

                        if (indices[indicesIndex] <= array.GetUpperBound(indicesIndex))
                            break;

                        indices[indicesIndex] = array.GetLowerBound(indicesIndex);
                    }
                } while (indicesIndex >= 0);
            }

            return totalSize;
        }

        public int Serialize(TArray value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            if (value == null)
                return Binary.InternalWrite7BitEncodedUInt32(0, buffer, offset, count);

            var array = (Array)(object)value;
            var start = offset;

            for (var i = 0; i < _rank; i++)
            {
                var length = array.GetLength(i);

                if (length > maxArrayLength)
                    throw new ArgumentException("Failed to serialize the array, because one or more dimensions exceeed the maximum array length.", nameof(value));

                if (i == 0)
                    length++;

                var size = Binary.InternalWrite7BitEncodedUInt32((uint)length, buffer, offset, count);

                offset += size;
                count -= size;
            }

            if (array.Length > 0)
            {
                var indices = GetIndices(array);
                int indicesIndex;

                do
                {
                    var size = GenericFormatter<TElement>.CachedInstance.Serialize((TElement)array.GetValue(indices), buffer, offset, count, maxArrayLength, maxRecursionDepth);

                    offset += size;
                    count -= size;

                    for (indicesIndex = _rank - 1; indicesIndex >= 0; indicesIndex--)
                    {
                        indices[indicesIndex]++;

                        if (indices[indicesIndex] <= array.GetUpperBound(indicesIndex))
                            break;

                        indices[indicesIndex] = array.GetLowerBound(indicesIndex);
                    }
                } while (indicesIndex >= 0);
            }

            return offset - start;
        }

        public TArray Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            var start = offset;
            var lengths = new int[_rank];
            var totalLength = 1;

            for (var i = 0; i < lengths.Length; i++)
            {
                var length = (int)Binary.InternalReadUInt32(buffer, offset, count, out var size);

                if (i == 0)
                {
                    if (length == 0)
                    {
                        bytesRead = size;
                        return null;
                    }

                    length--;
                }

                if ((uint)length > (uint)maxArrayLength)
                    throw new SerializationException("Failed to deserialize the array, because one or more dimensions exceeed the maximum array length.");

                totalLength = checked(totalLength * length);
                lengths[i] = length;

                offset += size;
                count -= size;
            }

            var array = Array.CreateInstance(typeof(TElement), lengths);
            
            if (totalLength > 0)
            {
                var indices = GetIndices(array);
                int indicesIndex;

                do
                {
                    array.SetValue(GenericFormatter<TElement>.CachedInstance.Deserialize(buffer, offset, count, out var size, maxArrayLength, maxRecursionDepth), indices);

                    offset += size;
                    count -= size;

                    for (indicesIndex = _rank - 1; indicesIndex >= 0; indicesIndex--)
                    {
                        indices[indicesIndex]++;

                        if (indices[indicesIndex] <= array.GetUpperBound(indicesIndex))
                            break;

                        indices[indicesIndex] = array.GetLowerBound(indicesIndex);
                    }
                } while (indicesIndex >= 0);
            }

            bytesRead = offset - start;
            return (TArray)(object)array;
        }

        private int[] GetIndices(Array array)
        {
            var indices = new int[_rank];

            for (var i = 0; i < indices.Length; i++)
                indices[i] = array.GetLowerBound(i);

            return indices;
        }
    }
}

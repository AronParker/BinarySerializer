﻿using System;
using System.Runtime.Serialization;

namespace BinarySerializer.Formatters.Primitives.Arrays
{
    internal sealed class UInt32ArrayFormatter : IFormatter<uint[]>
    {
        public int GetSize(uint[] value, int maxArrayLength, int maxRecursionDepth)
        {
            if (value == null)
                return Binary.InternalGet7BitEncodedUInt32Size(0);

            if (value.Length > maxArrayLength)
                throw new ArgumentException("Failed to get the size of the array, because it exceeds the maximum array length.", nameof(value));

            var size = Binary.InternalGet7BitEncodedUInt32Size((uint)value.Length + 1);

            for (var i = 0; i < value.Length; i++)
                size += Binary.InternalGet7BitEncodedUInt32Size(value[i]);

            return size;
        }

        public int Serialize(uint[] value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            if (value == null)
                return Binary.InternalWrite7BitEncodedUInt32(0, buffer, offset, count);

            if (value.Length > maxArrayLength)
                throw new ArgumentException("Failed to serialize the array, because it exceeds the maximum array length.", nameof(value));

            var start = offset;
            var size = Binary.InternalWrite7BitEncodedUInt32((uint)value.Length + 1, buffer, offset, count);

            offset += size;
            count -= size;

            for (var i = 0; i < value.Length; i++)
            {
                size = Binary.InternalWrite7BitEncodedUInt32(value[i], buffer, offset, count);

                offset += size;
                count -= size;
            }

            return offset - start;
        }

        public uint[] Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            var start = offset;
            var length = (int)Binary.InternalRead7BitEncodedUInt32(buffer, offset, count, out var size) - 1;

            if (length == -1)
            {
                bytesRead = size;
                return null;
            }

            if ((uint)length > (uint)maxArrayLength)
                throw new SerializationException("Failed to deserialize the array, because it exceeds the maximum array length");

            var value = length == 0 ? Array.Empty<uint>() : new uint[length];

            offset += size;
            count -= size;

            for (var i = 0; i < value.Length; i++)
            {
                value[i] = Binary.InternalRead7BitEncodedUInt32(buffer, offset, count, out size);

                offset += size;
                count -= size;
            }

            bytesRead = offset - start;
            return value;
        }
    }
}
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using BinarySerializer.Formatters;

namespace BinarySerializer
{
    public static class BinaryFormatter
    {
        public const int DefaultMaximumArrayLength = 65536;
        public const int DefaultMaximumRecursionDepth = 64;

        public static int GetSize<T>(T value)
        {
            return InternalGetSize(value, DefaultMaximumArrayLength, DefaultMaximumRecursionDepth);
        }

        public static int GetSize<T>(T value, int maxArrayLength, int maxRecursionDepth)
        {
            if (maxArrayLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            if (maxRecursionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRecursionDepth));

            return InternalGetSize(value, maxArrayLength, maxRecursionDepth);
        }

        public static byte[] Serialize<T>(T value)
        {
            return InternalSerialize(value, DefaultMaximumArrayLength, DefaultMaximumRecursionDepth);
        }

        public static byte[] Serialize<T>(T value, int maxArrayLength, int maxRecursionDepth)
        {
            if (maxArrayLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            if (maxRecursionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRecursionDepth));

            return InternalSerialize(value, maxArrayLength, maxRecursionDepth);
        }

        public static int Serialize<T>(T value, byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return InternalSerialize(value, buffer, 0, buffer.Length, DefaultMaximumArrayLength, DefaultMaximumRecursionDepth);
        }

        public static int Serialize<T>(T value, byte[] buffer, int maxArrayLength, int maxRecursionDepth)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (maxArrayLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            if (maxRecursionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRecursionDepth));

            return InternalSerialize(value, buffer, 0, buffer.Length, maxArrayLength, maxRecursionDepth);
        }

        public static int Serialize<T>(T value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (maxArrayLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            if (maxRecursionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRecursionDepth));

            return InternalSerialize(value, buffer, offset, count, maxArrayLength, maxRecursionDepth);
        }

        public static T Deserialize<T>(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return InternalDeserialize<T>(buffer, 0, buffer.Length, DefaultMaximumArrayLength, DefaultMaximumRecursionDepth, out var _);
        }

        public static T Deserialize<T>(byte[] buffer, out int bytesWritten)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return InternalDeserialize<T>(buffer, 0, buffer.Length, DefaultMaximumArrayLength, DefaultMaximumRecursionDepth, out bytesWritten);
        }

        public static T Deserialize<T>(byte[] buffer, int maxArrayLength, int maxRecursionDepth, out int bytesWritten)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (maxArrayLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            if (maxRecursionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRecursionDepth));

            return InternalDeserialize<T>(buffer, 0, buffer.Length, maxArrayLength, maxRecursionDepth, out bytesWritten);
        }

        public static T Deserialize<T>(byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth, out int bytesWritten)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (maxArrayLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            if (maxRecursionDepth < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRecursionDepth));

            return InternalDeserialize<T>(buffer, offset, count, maxArrayLength, maxRecursionDepth, out bytesWritten);
        }

        private static int InternalGetSize<T>(T value, int maxArrayLength, int maxRecursionDepth)
        {
            return InternalGetFormatter<T>().GetSize(value, maxArrayLength, maxRecursionDepth);
        }

        private static byte[] InternalSerialize<T>(T value, int maxArrayLength, int maxRecursionDepth)
        {
            var size = InternalGetFormatter<T>().GetSize(value, maxArrayLength, maxRecursionDepth);
            var result = new byte[size];
            var bytesWritten = InternalGetFormatter<T>().Serialize(value, result, 0, result.Length, maxArrayLength, maxRecursionDepth);

            Debug.Assert(size == bytesWritten);

            return result;
        }

        private static int InternalSerialize<T>(T value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return InternalGetFormatter<T>().Serialize(value, buffer, offset, count, maxArrayLength, maxRecursionDepth);
        }

        private static T InternalDeserialize<T>(byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth, out int bytesWritten)
        {
            return InternalGetFormatter<T>().Deserialize(buffer, offset, count, out bytesWritten, maxArrayLength, maxRecursionDepth);
        }

        private static IFormatter<T> InternalGetFormatter<T>()
        {
            var formatter = GenericFormatter<T>.CachedInstance;

            if (formatter == null)
                throw new SerializationException($"Type '{typeof(T).FullName}' in Assembly '{typeof(T).Assembly.FullName}' is not serializable.");

            return formatter;
        }
    }
}

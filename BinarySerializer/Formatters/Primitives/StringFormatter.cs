using System;
using System.IO;
using System.Runtime.Serialization;

namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class StringFormatter : IFormatter<string>
    {
        public int GetSize(string value, int maxArrayLength, int maxRecursionDepth)
        {
            if (value == null)
                return Binary.InternalGet7BitEncodedUInt32Size(0);

            var utf8Length = Binary.InternalGetStringSize(value);

            if (utf8Length > maxArrayLength)
                throw new ArgumentException("String length must not be greater than maxArrayLength.", nameof(value));

            return Binary.InternalGet7BitEncodedUInt32Size((uint)utf8Length + 1) + utf8Length;
        }

        public int Serialize(string value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            if (value == null)
                return Binary.InternalWrite7BitEncodedUInt32(0, buffer, offset, count);

            var utf8Length = Binary.InternalGetStringSize(value);

            if (utf8Length > maxArrayLength)
                throw new ArgumentException("String length must not be greater than maxArrayLength.", nameof(value));

            var lengthBytesWritten = Binary.InternalWrite7BitEncodedUInt32((uint)utf8Length + 1, buffer, offset, count);

            offset += lengthBytesWritten;
            count -= lengthBytesWritten;

            var valueBytesWritten = Binary.InternalWriteString(value, buffer, offset, count);

            return lengthBytesWritten + valueBytesWritten;
        }

        public string Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            var utf8Length = (int)Binary.InternalRead7BitEncodedUInt32(buffer, offset, count, out var lengthBytesRead) - 1;

            if (utf8Length == -1)
            {
                bytesRead = lengthBytesRead;
                return null;
            }

            if ((uint)utf8Length > (uint)count - (uint)lengthBytesRead)
                throw new EndOfStreamException();

            if ((uint)utf8Length > (uint)maxArrayLength)
                throw new SerializationException("String length must not be greater than maxArrayLength.");

            var value = Binary.InternalReadString(buffer, offset + lengthBytesRead, utf8Length, out var valueBytesRead);
            bytesRead = lengthBytesRead + valueBytesRead;
            return value;
        }
    }
}
namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class ByteFormatter : IFormatter<byte>
    {
        public int GetSize(byte value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetByteSize(value);
        }

        public int Serialize(byte value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteByte(value, buffer, offset, count);
        }

        public byte Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadByte(buffer, offset, count, out bytesRead);
        }
    }
}

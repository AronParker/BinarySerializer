namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class Int16Formatter : IFormatter<short>
    {
        public int GetSize(short value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetInt16Size(value);
        }

        public int Serialize(short value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteInt16(value, buffer, offset, count);
        }

        public short Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadInt16(buffer, offset, count, out bytesRead);
        }
    }
}

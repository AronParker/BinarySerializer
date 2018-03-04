namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class UInt16Formatter : IFormatter<ushort>
    {
        public int GetSize(ushort value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetUInt16Size(value);
        }

        public int Serialize(ushort value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteUInt16(value, buffer, offset, count);
        }

        public ushort Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadUInt16(buffer, offset, count, out bytesRead);
        }
    }
}

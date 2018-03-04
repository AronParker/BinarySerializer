namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class SByteFormatter : IFormatter<sbyte>
    {
        public int GetSize(sbyte value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetSByteSize(value);
        }

        public int Serialize(sbyte value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteSByte(value, buffer, offset, count);
        }

        public sbyte Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadSByte(buffer, offset, count, out bytesRead);
        }
    }
}

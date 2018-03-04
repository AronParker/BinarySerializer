namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class Int64Formatter : IFormatter<long>
    {
        public int GetSize(long value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGet7BitEncodedInt64Size(value);
        }

        public int Serialize(long value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWrite7BitEncodedInt64(value, buffer, offset, count);
        }

        public long Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalRead7BitEncodedInt64(buffer, offset, count, out bytesRead);
        }
    }
}

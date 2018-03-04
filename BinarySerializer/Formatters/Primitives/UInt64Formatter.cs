namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class UInt64Formatter : IFormatter<ulong>
    {
        public int GetSize(ulong value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGet7BitEncodedUInt64Size(value);
        }

        public int Serialize(ulong value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWrite7BitEncodedUInt64(value, buffer, offset, count);
        }

        public ulong Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalRead7BitEncodedUInt64(buffer, offset, count, out bytesRead);
        }
    }
}

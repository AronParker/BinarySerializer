namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class UInt32Formatter : IFormatter<uint>
    {
        public int GetSize(uint value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGet7BitEncodedUInt32Size(value);
        }

        public int Serialize(uint value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWrite7BitEncodedUInt32(value, buffer, offset, count);
        }

        public uint Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalRead7BitEncodedUInt32(buffer, offset, count, out bytesRead);
        }
    }
}

namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class Int32Formatter : IFormatter<int>
    {
        public int GetSize(int value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGet7BitEncodedInt32Size(value);
        }

        public int Serialize(int value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWrite7BitEncodedInt32(value, buffer, offset, count);
        }

        public int Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalRead7BitEncodedInt32(buffer, offset, count, out bytesRead);
        }
    }
}

namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class BooleanFormatter : IFormatter<bool>
    {
        public int GetSize(bool value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetBooleanSize(value);
        }

        public int Serialize(bool value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteBoolean(value, buffer, offset, count);
        }

        public bool Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadBoolean(buffer, offset, count, out bytesRead);
        }
    }
}

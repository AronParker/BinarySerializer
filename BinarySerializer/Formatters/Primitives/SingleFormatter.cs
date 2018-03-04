namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class SingleFormatter : IFormatter<float>
    {
        public int GetSize(float value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetSingleSize(value);
        }

        public int Serialize(float value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteSingle(value, buffer, offset, count);
        }

        public float Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadSingle(buffer, offset, count, out bytesRead);
        }
    }
}

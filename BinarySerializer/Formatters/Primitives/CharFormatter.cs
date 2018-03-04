namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class CharFormatter : IFormatter<char>
    {
        public int GetSize(char value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetCharSize(value);
        }

        public int Serialize(char value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteChar(value, buffer, offset, count);
        }

        public char Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadChar(buffer, offset, count, out bytesRead);
        }
    }
}

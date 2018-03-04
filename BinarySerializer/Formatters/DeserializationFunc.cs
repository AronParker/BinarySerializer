namespace BinarySerializer.Formatters
{
    public delegate T DeserializationFunc<out T>(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth);
}

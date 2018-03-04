namespace BinarySerializer.Formatters
{
    public delegate int SerializationFunc<in T>(T value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth);
}
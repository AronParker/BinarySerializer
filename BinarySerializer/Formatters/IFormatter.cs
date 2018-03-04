namespace BinarySerializer.Formatters
{
    public interface IFormatter<T>
    {
        int GetSize(T value, int maxArrayLength, int maxRecursionDepth);
        int Serialize(T value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth);
        T Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth);
    }
}

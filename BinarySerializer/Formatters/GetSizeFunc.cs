namespace BinarySerializer.Formatters
{
    public delegate int GetSizeFunc<in T>(T value, int maxArrayLength, int maxRecursionDepth);
}
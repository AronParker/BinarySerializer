namespace BinarySerializer.Formatters.Primitives
{
    internal sealed class DoubleFormatter : IFormatter<double>
    {
        public int GetSize(double value, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalGetDoubleSize(value);
        }

        public int Serialize(double value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalWriteDouble(value, buffer, offset, count);
        }

        public double Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return Binary.InternalReadDouble(buffer, offset, count, out bytesRead);
        }
    }
}
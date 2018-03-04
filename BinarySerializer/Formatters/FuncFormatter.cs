namespace BinarySerializer.Formatters
{
    internal sealed class FuncFormatter<T> : IFormatter<T>
    {
        private GetSizeFunc<T> _getSizeFunc;
        private SerializationFunc<T> _serializationFunc;
        private DeserializationFunc<T> _deserializationFunc;

        public FuncFormatter(GetSizeFunc<T> getSizeFunc, SerializationFunc<T> serializationFunc, DeserializationFunc<T> deserializationFunc)
        {
            _getSizeFunc = getSizeFunc;
            _serializationFunc = serializationFunc;
            _deserializationFunc = deserializationFunc;
        }

        public int GetSize(T value, int maxArrayLength, int maxRecursionDepth)
        {
            return _getSizeFunc(value, maxArrayLength, maxRecursionDepth);
        }

        public int Serialize(T value, byte[] buffer, int offset, int count, int maxArrayLength, int maxRecursionDepth)
        {
            return _serializationFunc(value, buffer, offset, count, maxArrayLength, maxRecursionDepth);
        }

        public T Deserialize(byte[] buffer, int offset, int count, out int bytesRead, int maxArrayLength, int maxRecursionDepth)
        {
            return _deserializationFunc(buffer, offset, count, out bytesRead, maxArrayLength, maxRecursionDepth);
        }
    }
}

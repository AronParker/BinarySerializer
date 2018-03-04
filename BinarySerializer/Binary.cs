using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

namespace BinarySerializer
{
    public static class Binary
    {
        public static int GetSByteSize(sbyte value)
        {
            return InternalGetSByteSize(value);
        }

        public static int GetByteSize(byte value)
        {
            return InternalGetByteSize(value);
        }

        public static int GetInt16Size(short value)
        {
            return InternalGetInt16Size(value);
        }

        public static int GetUInt16Size(ushort value)
        {
            return InternalGetUInt16Size(value);
        }

        public static int GetInt32Size(int value)
        {
            return InternalGet7BitEncodedInt32Size(value);
        }

        public static int GetUInt32Size(uint value)
        {
            return InternalGet7BitEncodedUInt32Size(value);
        }

        public static int GetInt64Size(long value)
        {
            return InternalGet7BitEncodedInt64Size(value);
        }

        public static int GetUInt64Size(ulong value)
        {
            return InternalGet7BitEncodedUInt64Size(value);
        }

        public static int GetBooleanSize(bool value)
        {
            return InternalGetBooleanSize(value);
        }

        public static int GetCharSize(char value)
        {
            return InternalGetCharSize(value);
        }

        public static int GetSingleSize(float value)
        {
            return InternalGetSingleSize(value);
        }

        public static int GetDoubleSize(double value)
        {
            return InternalGetDoubleSize(value);
        }
        
        public static int GetStringSize(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            return InternalGetStringSize(value);
        }

        public static int GetStringSize(string value, int charOffset, int charCount)
        {
            if (value == null)
                throw new ArgumentOutOfRangeException(nameof(value));
            if ((uint)charOffset > (uint)value.Length)
                throw new ArgumentOutOfRangeException(nameof(charOffset));
            if ((uint)charCount > (uint)value.Length - (uint)charOffset)
                throw new ArgumentOutOfRangeException(nameof(charCount));

            return InternalGetStringSize(value, charOffset, charCount);
        }

        public static sbyte ReadSByte(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadSByte(buffer, offset, count, out bytesRead);
        }

        public static byte ReadByte(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadByte(buffer, offset, count, out bytesRead);
        }

        public static short ReadInt16(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadInt16(buffer, offset, count, out bytesRead);
        }

        public static ushort ReadUInt16(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadUInt16(buffer, offset, count, out bytesRead);
        }

        public static int ReadInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadInt32(buffer, offset, count, out bytesRead);
        }

        public static uint ReadUInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadUInt32(buffer, offset, count, out bytesRead);
        }

        public static long ReadInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadInt64(buffer, offset, count, out bytesRead);
        }

        public static ulong ReadUInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadUInt64(buffer, offset, count, out bytesRead);
        }

        public static int Read7BitEncodedInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalRead7BitEncodedInt32(buffer, offset, count, out bytesRead);
        }

        public static uint Read7BitEncodedUInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalRead7BitEncodedUInt32(buffer, offset, count, out bytesRead);
        }

        public static long Read7BitEncodedInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalRead7BitEncodedInt64(buffer, offset, count, out bytesRead);
        }

        public static ulong Read7BitEncodedUInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalRead7BitEncodedUInt64(buffer, offset, count, out bytesRead);
        }

        public static bool ReadBoolean(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadBoolean(buffer, offset, count, out bytesRead);
        }

        public static char ReadChar(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadChar(buffer, offset, count, out bytesRead);
        }

        public static float ReadSingle(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadSingle(buffer, offset, count, out bytesRead);
        }

        public static double ReadDouble(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadDouble(buffer, offset, count, out bytesRead);
        }

        public static string ReadString(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalReadString(buffer, offset, count, out bytesRead);
        }

        public static int WriteSByte(sbyte value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteSByte(value, buffer, offset, count);
        }

        public static int WriteByte(byte value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteByte(value, buffer, offset, count);
        }

        public static int WriteInt16(short value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteInt16(value, buffer, offset, count);
        }

        public static int WriteUInt16(ushort value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteUInt16(value, buffer, offset, count);
        }

        public static int WriteInt32(int value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWrite7BitEncodedInt32(value, buffer, offset, count);
        }

        public static int WriteUInt32(uint value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWrite7BitEncodedUInt32(value, buffer, offset, count);
        }

        public static int WriteInt64(long value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWrite7BitEncodedInt64(value, buffer, offset, count);
        }

        public static int WriteUInt64(ulong value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWrite7BitEncodedUInt64(value, buffer, offset, count);
        }

        public static int WriteBoolean(bool value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteBoolean(value, buffer, offset, count);
        }

        public static int WriteChar(char value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteChar(value, buffer, offset, count);
        }

        public static int WriteSingle(float value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteSingle(value, buffer, offset, count);
        }

        public static int WriteDouble(double value, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteDouble(value, buffer, offset, count);
        }

        public static int WriteString(string value, byte[] buffer, int offset, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteString(value, buffer, offset, count);
        }

        public static int WriteString(string value, int charOffset, int charCount, byte[] buffer, int offset, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if ((uint)charOffset > (uint)value.Length)
                throw new ArgumentOutOfRangeException(nameof(charOffset));
            if ((uint)charCount > (uint)value.Length - (uint)charOffset)
                throw new ArgumentOutOfRangeException(nameof(charCount));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if ((uint)offset > (uint)buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((uint)count > (uint)buffer.Length - (uint)offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            return InternalWriteString(value, charOffset, charCount, buffer, offset, count);
        }

        internal static int InternalGetSByteSize(sbyte value)
        {
            return sizeof(sbyte);
        }

        internal static int InternalGetByteSize(byte value)
        {
            return sizeof(byte);
        }

        internal static int InternalGetInt16Size(short value)
        {
            return sizeof(short);
        }

        internal static int InternalGetUInt16Size(ushort value)
        {
            return sizeof(ushort);
        }

        internal static int InternalGetInt32Size(int value)
        {
            return sizeof(int);
        }

        internal static int InternalGetUInt32Size(uint value)
        {
            return sizeof(uint);
        }

        internal static int InternalGetInt64Size(long value)
        {
            return sizeof(long);
        }

        internal static int InternalGetUInt64Size(ulong value)
        {
            return sizeof(ulong);
        }

        internal static int InternalGet7BitEncodedInt32Size(int value)
        {
            return InternalGet7BitEncodedUInt32Size(ZigZagEncodeInt32(value));
        }

        internal static int InternalGet7BitEncodedUInt32Size(uint value)
        {
            var size = 1;

            while (value >= 0x80)
            {
                size++;
                value >>= 7;
            }

            return size;
        }

        internal static int InternalGet7BitEncodedInt64Size(long value)
        {
            return InternalGet7BitEncodedUInt64Size(ZigZagEncodeInt64(value));
        }

        internal static int InternalGet7BitEncodedUInt64Size(ulong value)
        {
            var size = 1;

            while (value >= 0x80)
            {
                size++;
                value >>= 7;
            }

            return size;
        }

        internal static int InternalGetBooleanSize(bool value)
        {
            return sizeof(bool);
        }

        internal static int InternalGetCharSize(char value)
        {
            return sizeof(char);
        }

        internal static int InternalGetSingleSize(float value)
        {
            return sizeof(float);
        }

        internal static int InternalGetDoubleSize(double value)
        {
            return sizeof(double);
        }

        internal static int InternalGetStringSize(string value)
        {
            if (value.Length == 0)
                return 0;

            return Encoding.UTF8.GetByteCount(value);
        }

        internal static unsafe int InternalGetStringSize(string value, int charOffset, int charCount)
        {
            if (charCount == 0)
                return 0;

            fixed (char* valuePtr = value)
                return Encoding.UTF8.GetByteCount(&valuePtr[charOffset], charCount);
        }

        internal static sbyte InternalReadSByte(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return (sbyte)InternalReadByte(buffer, offset, count, out bytesRead);
        }

        internal static byte InternalReadByte(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (count <= 0)
                throw new EndOfStreamException();

            bytesRead = sizeof(byte);
            return buffer[offset];
        }

        internal static short InternalReadInt16(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return (short)InternalReadUInt16(buffer, offset, count, out bytesRead);
        }

        internal static ushort InternalReadUInt16(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (count < sizeof(ushort))
                throw new EndOfStreamException();

            bytesRead = sizeof(ushort);
            return (ushort)(buffer[offset+0] << 0 |
                            buffer[offset+1] << 8);
        }

        internal static int InternalReadInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return (int)InternalReadUInt32(buffer, offset, count, out bytesRead);
        }

        internal static uint InternalReadUInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (count < sizeof(uint))
                throw new EndOfStreamException();

            bytesRead = sizeof(uint);
            return (uint)(buffer[offset + 0] << 0 |
                          buffer[offset + 1] << 8 |
                          buffer[offset + 2] << 16 |
                          buffer[offset + 3] << 24);
        }

        internal static long InternalReadInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return (long)InternalReadUInt64(buffer, offset, count, out bytesRead);
        }

        internal static ulong InternalReadUInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (count < sizeof(ulong))
                throw new EndOfStreamException();

            var lo = (uint)(buffer[offset + 0] << 0 |
                            buffer[offset + 1] << 8 |
                            buffer[offset + 2] << 16 |
                            buffer[offset + 3] << 24);

            var hi = (uint)(buffer[offset + 4] << 0 |
                            buffer[offset + 5] << 8 |
                            buffer[offset + 6] << 16 |
                            buffer[offset + 7] << 24);

            bytesRead = sizeof(ulong);
            return lo << 0 | (ulong)hi << 32;
        }

        internal static int InternalRead7BitEncodedInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return ZigZagDecodeUInt32(InternalRead7BitEncodedUInt32(buffer, offset, count, out bytesRead));
        }

        internal static uint InternalRead7BitEncodedUInt32(byte[] buffer, int offset, int count, out int bytesRead)
        {
            var limit = offset + count;

            if (offset == limit)
                throw new EndOfStreamException();

            var num = (uint)buffer[offset++];

            if (num < 0x80)
            {
                bytesRead = 1;
                return num;
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num &= 0x7F;
            var cur = (uint)buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 2;
                return num | (cur << (1 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (1 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 3;
                return num | (cur << (2 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (2 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 4;
                return num | (cur << (3 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();
            
            num |= (cur & 0x7F) << (3 * 7);
            cur = buffer[offset];

            if (cur < 0x80)
            {
                bytesRead = 5;
                return num | (cur << (4 * 7));
            }

            throw new SerializationException("Invalid 7-bit encoded 32-bit integer.");
        }

        internal static long InternalRead7BitEncodedInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return ZigZagDecodeUInt64(InternalRead7BitEncodedUInt64(buffer, offset, count, out bytesRead));
        }

        internal static ulong InternalRead7BitEncodedUInt64(byte[] buffer, int offset, int count, out int bytesRead)
        {
            var limit = offset + count;

            if (offset == limit)
                throw new EndOfStreamException();

            var num = (ulong)buffer[offset++];

            if (num < 0x80)
            {
                bytesRead = 1;
                return num;
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num &= 0x7F;
            var cur = (ulong)buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 2;
                return num | (cur << (1 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (1 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 3;
                return num | (cur << (2 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (2 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 4;
                return num | (cur << (3 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (3 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 5;
                return num | (cur << (4 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (4 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 6;
                return num | (cur << (5 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (5 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 7;
                return num | (cur << (6 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (6 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 8;
                return num | (cur << (7 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (7 * 7);
            cur = buffer[offset++];

            if (cur < 0x80)
            {
                bytesRead = 9;
                return num | (cur << (8 * 7));
            }

            if (offset == limit)
                throw new EndOfStreamException();

            num |= (cur & 0x7F) << (8 * 7);
            cur = buffer[offset];

            if (cur < 0x80)
            {
                bytesRead = 10;
                return num | (cur << (9 * 7));
            }

            throw new SerializationException("Invalid 7-bit encoded 64-bit integer.");
        }

        internal static bool InternalReadBoolean(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return InternalReadByte(buffer, offset, count, out bytesRead) != 0;
        }

        internal static char InternalReadChar(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return (char)InternalReadUInt16(buffer, offset, count, out bytesRead);
        }

        internal static unsafe float InternalReadSingle(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return *(float*)InternalReadUInt32(buffer, offset, count, out bytesRead);
        }

        internal static unsafe double InternalReadDouble(byte[] buffer, int offset, int count, out int bytesRead)
        {
            return *(double*)InternalReadUInt64(buffer, offset, count, out bytesRead);
        }

        internal static string InternalReadString(byte[] buffer, int offset, int count, out int bytesRead)
        {
            if (count == 0)
            {
                bytesRead = 0;
                return string.Empty;
            }

            bytesRead = count;
            return Encoding.UTF8.GetString(buffer, offset, count);
        }

        internal static int InternalWriteSByte(sbyte value, byte[] buffer, int offset, int count)
        {
            return InternalWriteByte((byte)value, buffer, offset, count);
        }

        internal static int InternalWriteByte(byte value, byte[] buffer, int offset, int count)
        {
            if (count < sizeof(byte))
                throw new EndOfStreamException();

            buffer[offset] = value;
            return sizeof(byte);
        }

        internal static int InternalWriteInt16(short value, byte[] buffer, int offset, int count)
        {
            return InternalWriteUInt16((ushort)value, buffer, offset, count);
        }

        internal static int InternalWriteUInt16(ushort value, byte[] buffer, int offset, int count)
        {
            if (count < sizeof(ushort))
                throw new EndOfStreamException();

            buffer[offset + 0] = (byte)(value >> 0);
            buffer[offset + 1] = (byte)(value >> 8);
            return sizeof(ushort);
        }

        internal static int InternalWriteInt32(int value, byte[] buffer, int offset, int count)
        {
            return InternalWriteUInt32((uint)value, buffer, offset, count);
        }

        internal static int InternalWriteUInt32(uint value, byte[] buffer, int offset, int count)
        {
            if (count < sizeof(uint))
                throw new EndOfStreamException();

            buffer[offset + 0] = (byte)(value >> 0);
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
            return sizeof(uint);
        }

        internal static int InternalWriteInt64(long value, byte[] buffer, int offset, int count)
        {
            return InternalWriteUInt64((ulong)value, buffer, offset, count);
        }

        internal static int InternalWriteUInt64(ulong value, byte[] buffer, int offset, int count)
        {
            if (count < sizeof(ulong))
                throw new EndOfStreamException();

            var lo = (uint)(value >> 0);
            var hi = (uint)(value >> 32);

            buffer[offset+0] = (byte)(lo >> 0);
            buffer[offset+1] = (byte)(lo >> 8);
            buffer[offset+2] = (byte)(lo >> 16);
            buffer[offset+3] = (byte)(lo >> 24);
            buffer[offset+4] = (byte)(hi >> 0);
            buffer[offset+5] = (byte)(hi >> 8);
            buffer[offset+6] = (byte)(hi >> 16);
            buffer[offset+7] = (byte)(hi >> 24);
            return sizeof(ulong);
        }

        internal static int InternalWrite7BitEncodedInt32(int value, byte[] buffer, int offset, int count)
        {
            return InternalWrite7BitEncodedUInt32(ZigZagEncodeInt32(value), buffer, offset, count);
        }

        internal static int InternalWrite7BitEncodedUInt32(uint value, byte[] buffer, int offset, int count)
        {
            if (value < (1U << (1 * 7)))
            {
                if (count < 1)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)(value >> (0 * 7));
                return 1;
            }

            if (value < (1U << (2 * 7)))
            {
                if (count < 2)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)(value >> (1 * 7));
                return 2;
            }

            if (value < (1 << (3 * 7)))
            {
                if (count < 3)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)(value >> (2 * 7));
                return 3;
            }

            if (value < (1 << (4 * 7)))
            {
                if (count < 4)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset + 3] = (byte)(value >> (3 * 7));
                return 4;
            }

            if (count < 5)
                throw new EndOfStreamException();

            buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
            buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
            buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
            buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
            buffer[offset + 4] = (byte)(value >> (4 * 7));
            return 5;
        }

        internal static int InternalWrite7BitEncodedInt64(long value, byte[] buffer, int offset, int count)
        {
            return InternalWrite7BitEncodedUInt64(ZigZagEncodeInt64(value), buffer, offset, count);
        }

        internal static int InternalWrite7BitEncodedUInt64(ulong value, byte[] buffer, int offset, int count)
        {
            if (value < (1UL << (1 * 7)))
            {
                if (count < 1)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)(value >> (0 * 7));
                return 1;
            }

            if (value < (1UL << (2 * 7)))
            {
                if (count < 2)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)(value >> (1 * 7));
                return 2;
            }

            if (value < (1UL << (3 * 7)))
            {
                if (count < 3)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)(value >> (2 * 7));
                return 3;
            }

            if (value < (1UL << (4 * 7)))
            {
                if (count < 4)
                    throw new EndOfStreamException();

                buffer[offset+0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset+1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset+2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset+3] = (byte)(value >> (3 * 7));
                return 4;
            }

            if (value < (1UL << (5 * 7)))
            {
                if (count < 5)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
                buffer[offset + 4] = (byte)(value >> (4 * 7));
                return 5;
            }

            if (value < (1UL << (6 * 7)))
            {
                if (count < 6)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
                buffer[offset + 4] = (byte)((value >> (4 * 7)) | 0x80);
                buffer[offset + 5] = (byte)(value >> (5 * 7));
                return 6;
            }

            if (value < (1UL << (7 * 7)))
            {
                if (count < 7)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
                buffer[offset + 4] = (byte)((value >> (4 * 7)) | 0x80);
                buffer[offset + 5] = (byte)((value >> (5 * 7)) | 0x80);
                buffer[offset+6] = (byte)(value >> (6 * 7));
                return 7;
            }

            if (value < (1UL << (8 * 7)))
            {
                if (count < 8)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
                buffer[offset + 4] = (byte)((value >> (4 * 7)) | 0x80);
                buffer[offset + 5] = (byte)((value >> (5 * 7)) | 0x80);
                buffer[offset + 6] = (byte)((value >> (6 * 7)) | 0x80);
                buffer[offset + 7] = (byte)(value >> (7 * 7));
                return 8;
            }

            if (value < (1UL << (9 * 7)))
            {
                if (count < 9)
                    throw new EndOfStreamException();

                buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
                buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
                buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
                buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
                buffer[offset + 4] = (byte)((value >> (4 * 7)) | 0x80);
                buffer[offset + 5] = (byte)((value >> (5 * 7)) | 0x80);
                buffer[offset + 6] = (byte)((value >> (6 * 7)) | 0x80);
                buffer[offset + 7] = (byte)((value >> (7 * 7)) | 0x80);
                buffer[offset + 8] = (byte)(value >> (8 * 7));
                return 9;
            }

            if (count < 10)
                throw new EndOfStreamException();

            buffer[offset + 0] = (byte)((value >> (0 * 7)) | 0x80);
            buffer[offset + 1] = (byte)((value >> (1 * 7)) | 0x80);
            buffer[offset + 2] = (byte)((value >> (2 * 7)) | 0x80);
            buffer[offset + 3] = (byte)((value >> (3 * 7)) | 0x80);
            buffer[offset + 4] = (byte)((value >> (4 * 7)) | 0x80);
            buffer[offset + 5] = (byte)((value >> (5 * 7)) | 0x80);
            buffer[offset + 6] = (byte)((value >> (6 * 7)) | 0x80);
            buffer[offset + 7] = (byte)((value >> (7 * 7)) | 0x80);
            buffer[offset + 8] = (byte)((value >> (8 * 7)) | 0x80);
            buffer[offset+ 9] = (byte)(value >> (9 * 7));
            return 10;
        }

        internal static int InternalWriteBoolean(bool value, byte[] buffer, int offset, int count)
        {
            return InternalWriteByte(value ? (byte)1 : (byte)0, buffer, offset, count);
        }

        internal static int InternalWriteChar(char value, byte[] buffer, int offset, int count)
        {
            return InternalWriteUInt16(value, buffer, offset, count);
        }

        internal static unsafe int InternalWriteSingle(float value, byte[] buffer, int offset, int count)
        {
            return InternalWriteUInt32(*(uint*)&value, buffer, offset, count);
        }

        internal static unsafe int InternalWriteDouble(double value, byte[] buffer, int offset, int count)
        {
            return InternalWriteUInt64(*(ulong*)&value, buffer, offset, count);
        }

        internal static unsafe int InternalWriteString(string value, byte[] buffer, int offset, int count)
        {
#warning add length
            if (value.Length == 0)
                return 0;

            fixed (char* chars = value)
            {
                var byteCount = Encoding.UTF8.GetByteCount(chars, value.Length);

                if (byteCount > count)
                    throw new EndOfStreamException();
                
                if (byteCount == value.Length)
                {
                    for (var i = 0; i < value.Length; i++)
                        buffer[offset++] = (byte)value[i];

                    return byteCount;
                }
                
                fixed (byte* bufferPtr = buffer)
                    return Encoding.UTF8.GetBytes(chars, value.Length, &bufferPtr[offset], count);
            }
        }

        internal static unsafe int InternalWriteString(string value, int charOffset, int charCount, byte[] buffer, int offset, int count)
        {
            if (charCount == 0)
                return 0;

            fixed (char* chars = value)
            {
                var byteCount = Encoding.UTF8.GetByteCount(chars, charCount);

                if (byteCount > count)
                    throw new EndOfStreamException();

                if (byteCount == charCount)
                {
                    var charLimit = charOffset + charCount;

                    while (charOffset < charLimit)
                        buffer[offset++] = (byte)value[charOffset++];

                    return byteCount;
                }

                fixed (byte* bufferPtr = buffer)
                    return Encoding.UTF8.GetBytes(&chars[charOffset], charCount, &bufferPtr[offset], count);
            }
        }

        private static uint ZigZagEncodeInt32(int value)
        {
            return (uint)((value << 1) ^ (value >> (8 * sizeof(int) - 1)));
        }

        private static ulong ZigZagEncodeInt64(long value)
        {
            return (ulong)((value << 1) ^ (value >> (8 * sizeof(long) - 1)));
        }

        private static int ZigZagDecodeUInt32(uint value)
        {
            return (int)(value >> 1) ^ -(int)(value & 1);
        }

        private static long ZigZagDecodeUInt64(ulong value)
        {
            return (long)(value >> 1) ^ -(long)(value & 1);
        }
    }
}

#if (!UNITY_WEBGL || UNITY_EDITOR) && !BESTHTTP_DISABLE_ALTERNATE_SSL

using System;

using Best.HTTP.Shared.PlatformSupport.Memory;

namespace Best.HTTP.Hosts.Connections.HTTP2
{
    public static class BufferHelper
    {
        public static void SetUInt16(byte[] buffer, int offset, UInt16 value)
        {
            buffer[offset + 1] = (byte)(value);
            buffer[offset + 0] = (byte)(value >> 8);
        }

        public static void SetUInt24(byte[] buffer, int offset, UInt32 value)
        {
            buffer[offset + 2] = (byte)(value);
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 0] = (byte)(value >> 16);
        }

        public static void SetUInt31(byte[] buffer, int offset, UInt32 value)
        {
            buffer[offset + 3] = (byte)(value);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 0] = (byte)((value >> 24) & 0x7F);
        }

        public static void SetUInt32(byte[] buffer, int offset, UInt32 value)
        {
            buffer[offset + 3] = (byte)(value);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 0] = (byte)(value >> 24);
        }

        public static void SetLong(byte[] buffer, int offset, long value)
        {
            buffer[offset + 7] = (byte)(value);
            buffer[offset + 6] = (byte)(value >> 8);
            buffer[offset + 5] = (byte)(value >> 16);
            buffer[offset + 4] = (byte)(value >> 24);
            buffer[offset + 3] = (byte)(value >> 32);
            buffer[offset + 2] = (byte)(value >> 40);
            buffer[offset + 1] = (byte)(value >> 48);
            buffer[offset + 0] = (byte)(value >> 56);
        }

        public static byte SetBit(byte value, byte bitIdx, bool bitValue)
        {
            // bitIdx: 01234567
            return SetBit(value, bitIdx, Convert.ToByte(bitValue));
        }

        public static byte SetBit(byte value, byte bitIdx, byte bitValue)
        {
            // bitIdx: 01234567

            return (byte)((value ^ (value & (0x80 >> bitIdx))) | bitValue << (7 - bitIdx));
        }

        public static byte ReadBit(byte value, byte bitIdx)
        {
            // bitIdx: 01234567
            byte mask = (byte)(0x80 >> bitIdx);

            return (byte)((value & mask) >> (7 - bitIdx));
        }

        public static byte ReadValue(byte value, byte fromBit, byte toBit)
        {
            // bitIdx: 01234567

            byte result = 0;
            short idx = toBit;

            while (idx >= fromBit)
            {
                result += (byte)(ReadBit(value, (byte)idx) << (toBit - idx));
                idx--;
            }

            return result;
        }

        public static UInt16 ReadUInt16(byte[] buffer, int offset)
        {
            return (UInt16)(buffer[offset + 1] | buffer[offset] << 8);
        }

        public static UInt32 ReadUInt24(byte[] buffer, int offset)
        {
            return (UInt32)(buffer[offset + 2] |
                            buffer[offset + 1] << 8 |
                            buffer[offset + 0] << 16
                            );
        }

        public static UInt32 ReadUInt31(byte[] buffer, int offset)
        {
            return (UInt32)(buffer[offset + 3] |
                            buffer[offset + 2] << 8 |
                            buffer[offset + 1] << 16 |
                            (buffer[offset] & 0x7F) << 24
                           );
        }

        public static UInt32 ReadUInt31(BufferSegment buffer, int offset)
        {
            return (UInt32)( buffer.Data[offset + 3] |
                             buffer.Data[offset + 2] << 8 |
                             buffer.Data[offset + 1] << 16 |
                            (buffer.Data[offset] & 0x7F) << 24
                           );
        }

        public static UInt32 ReadUInt32(byte[] buffer, int offset)
        {
            return (UInt32)(buffer[offset + 3] |
                            buffer[offset + 2] << 8 |
                            buffer[offset + 1] << 16 |
                            buffer[offset + 0] << 24
                            );
        }

        public static UInt32 ReadUInt32(BufferSegment buffer, int offset)
        {
            return (UInt32)(buffer.Data[offset + 3] |
                            buffer.Data[offset + 2] << 8 |
                            buffer.Data[offset + 1] << 16 |
                            buffer.Data[offset + 0] << 24
                            );
        }

        public static long ReadLong(BufferSegment buffer, int offset)
        {
            return (long)buffer.Data[offset + 7] |
                   (long)buffer.Data[offset + 6] << 8 |
                   (long)buffer.Data[offset + 5] << 16 |
                   (long)buffer.Data[offset + 4] << 24 |
                   (long)buffer.Data[offset + 3] << 32 |
                   (long)buffer.Data[offset + 2] << 40 |
                   (long)buffer.Data[offset + 1] << 48 |
                   (long)buffer.Data[offset + 0] << 56;
        }
    }
}

#endif

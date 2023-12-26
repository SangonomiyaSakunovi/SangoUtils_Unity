using System;
using System.Collections.Generic;

public static class IOCPUtils
{
    public static byte[] SplitLogicBytes(ref List<byte> bytesList)
    {
        if (bytesList.Count > 4)
        {
            byte[] data = bytesList.ToArray();
            int length = BitConverter.ToInt32(data, 0);
            if (bytesList.Count >= length + 4)
            {
                byte[] buff = new byte[length];
                Buffer.BlockCopy(data, 4, buff, 0, length);
                bytesList.RemoveRange(0, length + 4);
                return buff;
            }
        }
        return null;
    }

    public static byte[] PackMessageLengthInfo(byte[] body)
    {
        int length = body.Length;
        byte[] package = new byte[length + 4];
        byte[] head = BitConverter.GetBytes(length);
        head.CopyTo(package, 0);
        body.CopyTo(package, 4);
        return package;
    }
}

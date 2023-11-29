using System;
using System.Text;

public abstract class TimeCryptoUtils
{
    public static string EncryptTimestamp(long timestamp)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(timestamp.ToString());
        return Convert.ToBase64String(bytes);
    }

    public static string EncryptTimestamp(string timestamp)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(timestamp);
        return Convert.ToBase64String(bytes);
    }

    public static string DecryptTimestamp(string base64Data)
    {
        byte[] bytes = Convert.FromBase64String(base64Data);
        return Encoding.UTF8.GetString(bytes);
    }
}

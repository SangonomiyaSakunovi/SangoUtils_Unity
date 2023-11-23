using System;
using System.Security.Cryptography;
using System.Text;

public static class Md5SignatureUtils
{
    public static string GenerateMd5SignData(string rawData, string timestamp, string apiKey, string apiSecret)
    {
        string signData = apiKey + apiSecret + timestamp;
        string signPatameterMd5 = rawData + signData;
        string md5Str = GetMd5Str(signPatameterMd5);
        return md5Str;
    }

    public static bool CheckMd5SignDataValid(string rawData, string md5Data, string timestamp, string apiKey, string apiSecret)
    {
        string signData = apiKey + apiSecret + timestamp;
        string signParameterMd5 = rawData + signData;
        string md5Str = GetMd5Str(signParameterMd5);
        if (md5Str == md5Data)
        {
            return true;
        }
        return false;
    }

    private static string GetMd5Str(string str)
    {
        MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
        return Convert.ToBase64String(bytes);
    }
}

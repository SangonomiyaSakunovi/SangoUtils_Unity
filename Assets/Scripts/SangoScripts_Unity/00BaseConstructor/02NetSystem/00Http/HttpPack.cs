using SangoUtils_Extensions_Universal.Utils;
using SangoUtils_Logger;
using System;
using UnityEngine.Networking;

namespace SangoUtils_Unity_Scripts.Net
{
    public enum HttpType
    {
        Get,
        Post,
        Put
    }

    public abstract class HttpPack
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public HttpType HttpType { get; set; }
        public Type DataType { get; set; }
        public string Parame { get; set; }
        public int TryCount { get; set; }
        public UnityWebRequest WebRequest { get; set; }

        public abstract void OnDataReceived(string dataStr, int code, int messageId);
    }

    public class HttpPack<T> : HttpPack where T : class
    {
        public override void OnDataReceived(string dataStr, int code, int messageId)
        {
            SangoLogger.Log("HttpMessageId:[" + messageId + "], ReceivedStr: " + dataStr);
            HttpDataInfo dataInfo = JsonUtils.FromJson<HttpDataInfo>(dataStr);
            if (dataInfo != null)
            {
                if (dataInfo.res == 0)
                {
                    if (!string.IsNullOrEmpty(dataInfo.data))
                    {
                        T data;
                        if (typeof(T).Name == "String")
                        {
                            data = dataInfo.data as T;
                        }
                        else
                        {
                            data = JsonUtils.FromJson<T>(dataInfo.data);
                        }
                        if (data != null)
                        {
                            HttpService.Instance?.HttpBroadcast<T>(data, messageId, dataInfo.res);
                        }
                    }
                    else
                    {
                        HttpService.Instance?.HttpBroadcast<T>(null, messageId, dataInfo.res);
                    }
                }
                else
                {
                    HttpService.Instance?.HttpBroadcast<T>(null, messageId, dataInfo.res);
                }
            }

        }
    }

    public class HttpDataInfo
    {
        public int res { get; set; } = 0;
        public string data { get; set; } = "";
    }
}


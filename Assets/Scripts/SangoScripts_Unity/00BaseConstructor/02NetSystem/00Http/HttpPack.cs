using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Networking;

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
        JObject recvObj = JObject.Parse(dataStr);
        int resCode = recvObj["res"].Value<int>();
        if (resCode == 0)
        {
            string recievedData = recvObj["data"].ToString();
            if (!string.IsNullOrEmpty(recievedData))
            {
                T data;
                if (typeof(T).Name == "String")
                {
                    data = recievedData as T;
                }
                else
                {
                    data = JsonUtils.DeJsonString<T>(recievedData.ToString());
                }
                if (data != null)
                {
                    HttpService.Instance?.HttpBroadcast<T>(data, messageId, resCode);
                }
            }
            else
            {
                HttpService.Instance?.HttpBroadcast<T>(null, messageId, resCode);
            }
        }
        else
        {
            HttpService.Instance?.HttpBroadcast<T>(null, messageId, resCode);
        }
    }
}

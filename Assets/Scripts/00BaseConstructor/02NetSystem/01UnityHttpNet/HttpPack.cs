using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.Networking;

public enum HttpType
{
    Get,
    Post,
    Put
}

public abstract class HttpPack
{
    public int id;
    public string url;
    public HttpType httpType;
    public Type dataType;
    public string parame;
    public int tryCount;
    public UnityWebRequest webRequest;

    public abstract void OnDataReceived(string dataStr, int code, int messageId);
}

public class HttpPack<T> : HttpPack where T : class
{
    public override void OnDataReceived(string dataStr, int code, int messageId)
    {
        Debug.Log("HttpMessageId:[" + messageId + "], ReceivedStr: " + dataStr);
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

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient
{
    private const int _webReqTimeout = 15;
    private const int _webReqTryCount = 3;
    private const string _serviceHost = "localhost";

    private List<HttpPack> sendList = new List<HttpPack>(10);
    private List<HttpPack> receiveList = new List<HttpPack>(10);

    public void Init()
    {
    }
    public void Clear()
    {
    }

    public void UpdateHttpRequest()
    {
        HandleSend();
        HandleRecive();
    }

    public void SendHttpRequest<T>(int httpId, HttpType httpType, string parame) where T : class
    {
        AddHttpRequest<T>(httpId, httpType, parame, typeof(T));
    }

    private void AddHttpRequest<T>(int httpId, HttpType httpType, string parame, Type dataType) where T : class
    {
        HttpPack<T> pack = new HttpPack<T>();
        pack.id = httpId;
        string apiKey = HttpId.GetHttpApi(httpId);
        string serviceHost = _serviceHost;

        pack.url = serviceHost + apiKey;
        pack.httpType = httpType;
        pack.dataType = dataType;
        pack.parame = parame;
        pack.tryCount = _webReqTryCount;

        sendList.Add(pack);
    }

    private void HandleSend()
    {
        if (sendList.Count == 0)
            return;

        for (int i = 0; i < sendList.Count; i++)
        {
            HttpPack pack = sendList[i];
            switch (pack.httpType)
            {
                case HttpType.Get:
                    {
                        if (!string.IsNullOrEmpty(pack.parame))
                        {
                            pack.url = string.Format("{0}?{1}", pack.url, pack.parame);
                        }
                        pack.webRequest = new UnityWebRequest(pack.url, UnityWebRequest.kHttpVerbGET);
                        pack.webRequest.downloadHandler = new DownloadHandlerBuffer();
                        pack.webRequest.timeout = _webReqTimeout;
                        pack.webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                        pack.webRequest.SetRequestHeader("token", "");
                        pack.webRequest.SendWebRequest();
                    }
                    break;
                case HttpType.Post:
                    {
                        pack.webRequest = new UnityWebRequest(pack.url, UnityWebRequest.kHttpVerbPOST);
                        if (!string.IsNullOrEmpty(pack.parame))
                        {
                            byte[] databyte = Encoding.UTF8.GetBytes(pack.parame);
                            pack.webRequest.uploadHandler = new UploadHandlerRaw(databyte);
                        }
                        pack.webRequest.downloadHandler = new DownloadHandlerBuffer();
                        pack.webRequest.timeout = _webReqTimeout;
                        pack.webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                        pack.webRequest.SetRequestHeader("token", "");                       
                        pack.webRequest.SendWebRequest();
                    }
                    break;
                case HttpType.Put:
                    {
                        pack.webRequest = new UnityWebRequest(pack.url, UnityWebRequest.kHttpVerbPUT);
                        if (!string.IsNullOrEmpty(pack.parame))
                        {
                            byte[] databyte = Encoding.UTF8.GetBytes(pack.parame);
                            pack.webRequest.uploadHandler = new UploadHandlerRaw(databyte);
                        }
                        pack.webRequest.downloadHandler = new DownloadHandlerBuffer();
                        pack.webRequest.timeout = _webReqTimeout;
                        pack.webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                        pack.webRequest.SetRequestHeader("token", "");
                        pack.webRequest.SendWebRequest();
                    }
                    break;
            }
            receiveList.Add(pack);
        }
        sendList.Clear();
    }

    private void HandleRecive()
    {
        if (receiveList.Count == 0)
            return;

        for (int i = receiveList.Count - 1; i >= 0; i--)
        {
            HttpPack pack = receiveList[i];
            if (pack.webRequest == null)
            {
                receiveList.Remove(pack);
                continue;
            }

            if (pack.webRequest.isDone)
            {
            }
            else if (pack.webRequest.isHttpError || pack.webRequest.isNetworkError)
            {
                Debug.LogError(pack.webRequest.error);
            }
            else
            {
                continue;
            }

            int responseCode = (int)pack.webRequest.responseCode;
            string responseJson = pack.webRequest.downloadHandler.text;

            pack.webRequest.Abort();
            pack.webRequest.Dispose();
            pack.webRequest = null;
            receiveList.Remove(pack);

            if (responseCode != 200 && --pack.tryCount > 0)
            {
                Debug.Log("Try reconnect Id: [" +pack.id + " ], try times: " + pack.tryCount);

                sendList.Add(pack);
                continue;
            }

            CheckResponseCode(pack.id, responseCode, responseJson);

            pack.OnData(responseJson, responseCode, pack.id);
        }
    }

    private void CheckResponseCode(int httpId, int responseCode, string responseJson)
    {
        if (responseCode == 200)
            return;

        int codeType = responseCode / 100;
        string apiKey = HttpId.GetHttpApi(httpId);

        switch (codeType)
        {
            case 4:
                Debug.LogWarning(string.Format("{0} : {1} : ClientError", responseCode, apiKey));
                break;
            case 5:
            case 6:
                Debug.LogWarning(string.Format("{0} : {1} : ServerError", responseCode, apiKey));
                break;
            default:
                Debug.LogWarning(string.Format("{0} : {1} : UnknownError", responseCode, apiKey));
                break;
        }
    }

}
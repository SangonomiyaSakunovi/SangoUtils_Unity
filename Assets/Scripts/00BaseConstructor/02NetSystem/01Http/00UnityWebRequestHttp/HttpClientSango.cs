using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientSango
{
    private const int _webReqTimeout = 15;
    private const int _webReqTryCount = 3;
    private const string _serviceHost = "localhost";

    private List<HttpPack> _sendHttpPacks = new List<HttpPack>(10);
    private List<HttpPack> _receivedHttpPacks = new List<HttpPack>(10);

    private List<HttpBaseResourcePack> _sendHttpResourcePacks = new List<HttpBaseResourcePack>(10);
    private List<HttpBaseResourcePack> _receivedHttpResourcePacks = new List<HttpBaseResourcePack>(10);

    public void Init()
    {
    }
    public void Clear()
    {
    }

    public void UpdateHttpRequest()
    {
        HandleRequestSend();
        HandleRequestResponsed();
        HandleResourceRequestSend();
        HandleResourceRequestResponsed();
    }

    public void SendHttpRequest<T>(int httpId, HttpType httpType, string parame) where T : class
    {
        AddHttpRequest<T>(httpId, httpType, parame, typeof(T));
    }

    public void SendHttpResourceRequest(HttpBaseResourcePack httpBaseResourcePack)
    {
        AddHttpResourceRequest(httpBaseResourcePack);
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

        _sendHttpPacks.Add(pack);
    }

    private void HandleRequestSend()
    {
        if (_sendHttpPacks.Count == 0) return;

        for (int i = 0; i < _sendHttpPacks.Count; i++)
        {
            HttpPack pack = _sendHttpPacks[i];
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
            _receivedHttpPacks.Add(pack);
        }
        _sendHttpPacks.Clear();
    }

    private void HandleRequestResponsed()
    {
        if (_receivedHttpPacks.Count == 0)
            return;

        for (int i = _receivedHttpPacks.Count - 1; i >= 0; i--)
        {
            HttpPack pack = _receivedHttpPacks[i];
            if (pack.webRequest == null)
            {
                _receivedHttpPacks.Remove(pack);
                continue;
            }

            if (pack.webRequest.isDone)
            {
                int responseCode = (int)pack.webRequest.responseCode;
                string responseJson = pack.webRequest.downloadHandler.text;
                if (responseCode != 200 && --pack.tryCount > 0)
                {
                    Debug.Log("Try reconnect Id: [" + pack.id + " ], try times: " + pack.tryCount);

                    _sendHttpPacks.Add(pack);
                    continue;
                }
                CheckResponseCode(pack.id, responseCode, responseJson);
                pack.OnDataReceived(responseJson, responseCode, pack.id);
            }
            else if (pack.webRequest.isHttpError || pack.webRequest.isNetworkError)
            {

            }
            else
            {
                continue;
            }
            pack.webRequest.Abort();
            pack.webRequest.Dispose();
            pack.webRequest = null;
            _receivedHttpPacks.Remove(pack);
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

    private void AddHttpResourceRequest(HttpBaseResourcePack httpBaseResourcePack)
    {
        _sendHttpResourcePacks.Add(httpBaseResourcePack);
    }

    private void HandleResourceRequestSend()
    {
        if (_sendHttpResourcePacks.Count == 0) return;
        for (int i = 0; i < _sendHttpResourcePacks.Count; i++)
        {
            HttpBaseResourcePack pack = _sendHttpResourcePacks[i];
            pack.tryCount = _webReqTryCount;
            pack.OnRequest();
            _receivedHttpResourcePacks.Add(pack);
        }
        _sendHttpResourcePacks.Clear();
    }

    private void HandleResourceRequestResponsed()
    {
        if (_receivedHttpResourcePacks.Count == 0) return;

        for (int i = _receivedHttpResourcePacks.Count - 1; i >= 0; i--)
        {
            HttpBaseResourcePack pack = _receivedHttpResourcePacks[i];
            if (pack.webRequest == null)
            {
                _receivedHttpResourcePacks.Remove(pack);
                continue;
            }

            if (pack.webRequest.isDone)
            {
                int responseCode = (int)pack.webRequest.responseCode;
                if (responseCode != 200 && --pack.tryCount > 0)
                {
                    _sendHttpResourcePacks.Add(pack);
                    continue;
                }               
                if (pack.webRequest.downloadHandler.isDone)
                {
                    pack.OnResponsed();
                }
                else
                {
                    pack.OnErrored();
                }
            }

            else if (pack.webRequest.isHttpError || pack.webRequest.isNetworkError)
            {
                pack.OnErrored();
            }
            else
            {
                continue;
            }
            _receivedHttpResourcePacks.Remove(pack);
        }
    }

}
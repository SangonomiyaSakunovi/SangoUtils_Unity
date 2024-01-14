using SangoUtils_Logger;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace SangoScripts_Unity.Net
{
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
            pack.Id = httpId;
            string apiKey = HttpId.GetHttpApi(httpId);
            string serviceHost = _serviceHost;

            pack.Url = serviceHost + apiKey;
            pack.HttpType = httpType;
            pack.DataType = dataType;
            pack.Parame = parame;
            pack.TryCount = _webReqTryCount;

            _sendHttpPacks.Add(pack);
        }

        private void HandleRequestSend()
        {
            if (_sendHttpPacks.Count == 0) return;

            for (int i = 0; i < _sendHttpPacks.Count; i++)
            {
                HttpPack pack = _sendHttpPacks[i];
                switch (pack.HttpType)
                {
                    case HttpType.Get:
                        {
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                pack.Url = string.Format("{0}?{1}", pack.Url, pack.Parame);
                            }
                            pack.WebRequest = new UnityWebRequest(pack.Url, UnityWebRequest.kHttpVerbGET);
                            pack.WebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.WebRequest.timeout = _webReqTimeout;
                            pack.WebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                            pack.WebRequest.SetRequestHeader("token", "");
                            pack.WebRequest.SendWebRequest();
                        }
                        break;
                    case HttpType.Post:
                        {
                            pack.WebRequest = new UnityWebRequest(pack.Url, UnityWebRequest.kHttpVerbPOST);
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                byte[] databyte = Encoding.UTF8.GetBytes(pack.Parame);
                                pack.WebRequest.uploadHandler = new UploadHandlerRaw(databyte);
                            }
                            pack.WebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.WebRequest.timeout = _webReqTimeout;
                            pack.WebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                            pack.WebRequest.SetRequestHeader("token", "");
                            pack.WebRequest.SendWebRequest();
                        }
                        break;
                    case HttpType.Put:
                        {
                            pack.WebRequest = new UnityWebRequest(pack.Url, UnityWebRequest.kHttpVerbPUT);
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                byte[] databyte = Encoding.UTF8.GetBytes(pack.Parame);
                                pack.WebRequest.uploadHandler = new UploadHandlerRaw(databyte);
                            }
                            pack.WebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.WebRequest.timeout = _webReqTimeout;
                            pack.WebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                            pack.WebRequest.SetRequestHeader("token", "");
                            pack.WebRequest.SendWebRequest();
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
                if (pack.WebRequest == null)
                {
                    _receivedHttpPacks.Remove(pack);
                    continue;
                }

                if (pack.WebRequest.isDone)
                {
                    int responseCode = (int)pack.WebRequest.responseCode;
                    string responseJson = pack.WebRequest.downloadHandler.text;
                    if (responseCode != 200 && --pack.TryCount > 0)
                    {
                        SangoLogger.Log("Try reconnect Id: [" + pack.Id + " ], try times: " + pack.TryCount);

                        _sendHttpPacks.Add(pack);
                        continue;
                    }
                    CheckResponseCode(pack.Id, responseCode, responseJson);
                    pack.OnDataReceived(responseJson, responseCode, pack.Id);
                }
                else if (pack.WebRequest.result == UnityWebRequest.Result.ProtocolError || pack.WebRequest.result == UnityWebRequest.Result.ConnectionError)
                {

                }
                else
                {
                    continue;
                }
                pack.WebRequest.Abort();
                pack.WebRequest.Dispose();
                pack.WebRequest = null;
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
                    SangoLogger.Warning(string.Format("{0} : {1} : ClientError", responseCode, apiKey));
                    break;
                case 5:
                case 6:
                    SangoLogger.Warning(string.Format("{0} : {1} : ServerError", responseCode, apiKey));
                    break;
                default:
                    SangoLogger.Warning(string.Format("{0} : {1} : UnknownError", responseCode, apiKey));
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
                pack.TryCount = _webReqTryCount;
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
                if (pack.WebRequest == null)
                {
                    _receivedHttpResourcePacks.Remove(pack);
                    continue;
                }

                if (pack.WebRequest.isDone)
                {
                    int responseCode = (int)pack.WebRequest.responseCode;
                    if (responseCode != 200 && --pack.TryCount > 0)
                    {
                        _sendHttpResourcePacks.Add(pack);
                        continue;
                    }
                    if (pack.WebRequest.downloadHandler.isDone)
                    {
                        pack.OnResponsed();
                    }
                    else
                    {
                        pack.OnErrored();
                    }
                }

                else if (pack.WebRequest.result == UnityWebRequest.Result.ProtocolError || pack.WebRequest.result == UnityWebRequest.Result.ConnectionError)
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
}
using System;
using System.Collections.Generic;
using System.Text;

public class HttpService : BaseService<HttpService>
{
    private HttpClient _httpClient;
    private Dictionary<int, BaseRequest> _requestDict = new Dictionary<int, BaseRequest>();

    public override void OnInit()
    {
        base.OnInit();
        _httpClient = new HttpClient();
        _httpClient.Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _httpClient.UpdateHttpRequest();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        _httpClient.Clear();
    }

    public void HttpGet<T>(int httpId, Dictionary<string, string> getParameDic) where T : class
    {
        string getParameStr;
        if (getParameDic == null || getParameDic.Count == 0)
            getParameStr = null;
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool isFirst = true;
            foreach (var item in getParameDic)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    stringBuilder.Append('&');
                }
                stringBuilder.Append(Uri.EscapeDataString(item.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(Uri.EscapeDataString(item.Value));
            }
            getParameStr = stringBuilder.ToString();
        }
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Get, getParameStr);
    }

    public void HttpGet<T>(int httpId, string getParameStr) where T : class
    {
        //Protocol: getParameStr = url?parame1Name=parame1Value&parame2Name=parame2Value
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Get, getParameStr);
    }

    public void HttpGet<T>(int httpId) where T : class
    {
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Get, null);
    }

    public void HttpPost<T>(int httpId, Dictionary<string, string> postParameDic) where T : class
    {
        string postParameStr;
        if (postParameDic == null || postParameDic.Count == 0)
            postParameStr = null;
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool isFirst = true;
            foreach (var item in postParameDic)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    stringBuilder.Append('&');
                }
                stringBuilder.Append(Uri.EscapeDataString(item.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(Uri.EscapeDataString(item.Value));
            }
            postParameStr = stringBuilder.ToString();
        }
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Post, postParameStr);
    }

    public void HttpPost<T>(int httpId, string postParameStr) where T : class
    {
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Post, postParameStr);
    }

    public void HttpPost<T>(int httpId, object postParame) where T : class
    {
        string postParameStr = JsonUtils.SetJsonString(postParame);
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Post, postParameStr);
    }

    public void HttpPost<T>(int httpId) where T : class
    {
        _httpClient.SendHttpRequest<T>(httpId, HttpType.Post, null);
    }

    public void HttpPut(int httpId, string putParameStr)
    {
        _httpClient.SendHttpRequest<string>(httpId, HttpType.Put, putParameStr);
    }

    public void HttpPut(int httpId, object putParame)
    {
        string putParameStr = JsonUtils.SetJsonString(putParame);
        _httpClient.SendHttpRequest<string>(httpId, HttpType.Put, putParameStr);
    }

    public void HttpBroadcast<T>(T data, int id) where T : class
    {
        _requestDict.TryGetValue(id, out BaseRequest request);
        if (request != null)
        {
            request.OnOperationResponsed<T>(data);
        }
    }

    public void AddRequest(BaseRequest req)
    {
        _requestDict.Add(req.HttpId, req);
    }

    public T GetRequest<T>(int httpId) where T : BaseRequest, new()
    {
        if (_requestDict.ContainsKey(httpId))
        {
            return (T)_requestDict[httpId];
        }
        else
        {
            T request = new T();
            request.OnInit(httpId);
            return request;
        }
    }

    public void RemoveRequest(BaseRequest req)
    {
        _requestDict.Remove(req.HttpId);
    }
}

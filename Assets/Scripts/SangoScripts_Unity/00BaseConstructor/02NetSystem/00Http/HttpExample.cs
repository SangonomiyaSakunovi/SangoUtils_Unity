using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

public class HttpExample : MonoBehaviour
{
    private SangoHttpExampleRequest _sangoHttpExampleRequest = null;
    private List<SangoHttpExampleInfo> _sangoHttpExampleInfos = new();

    public static HttpExample Instance;

    public void InitService()
    {
        Instance = this;
        HttpService.Instance.OnInit();
    }

    private void InitRequest()
    {
        _sangoHttpExampleRequest = HttpService.Instance.GetRequest<SangoHttpExampleRequest>(HttpId.loginId);
    }

    private void SendRequestAsync()
    {
        _sangoHttpExampleRequest.SetReuqestInfo(0);
        _sangoHttpExampleRequest.DefaultRequest<List<SangoHttpExampleData>>();
    }

    public void OnSendRequestResponsed(List<SangoHttpExampleData> data)
    {
        _sangoHttpExampleInfos.Clear();
        for (int i = 0; i < data.Count; i++)
        {
            SangoHttpExampleData item = data[i];
            SangoHttpExampleInfo info = new SangoHttpExampleInfo(item.id, item.discription);
            _sangoHttpExampleInfos.Add(info);
        }
    }
}

public class SangoHttpExampleRequest : BaseHttpRequest
{
    public override void OnInit(int httpId)
    {
        base.OnInit(httpId);
    }

    public override void OnOperationResponsed<T>(T data, int resCode)
    {
        List<SangoHttpExampleData> value = data as List<SangoHttpExampleData>;
        if (value != null)
        {
            HttpExample.Instance?.OnSendRequestResponsed(value);
        }
    }

    public void SetReuqestInfo(int id)
    {
        _contentDict.Clear();
        _contentDict.Add("id", id.ToString());
    }

    public override void DefaultRequest<T>()
    {
        base.SendRequest<T>(_contentDict);
    }
}

[Serializable]
public class SangoHttpExampleData
{
    [JsonConverter(typeof(NullableInt32Converter))]
    public int id { get; set; }
    [JsonConverter(typeof(NullableStringConverter))]
    public string discription { get; set; }
}

public class SangoHttpExampleInfo
{
    public int id { get; private set; }
    public string discription { get; private set; }

    public SangoHttpExampleInfo(int id, string discription)
    {
        this.id = id;
        this.discription = discription;
    }
}

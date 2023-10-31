using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Application;

public enum HttpResourceType
{
    RawImage,
    Audio
}

public abstract class HttpBaseResourcePack
{
    public string url;
    public int tryCount;
    public HttpResourceType resourceType;
    public UnityWebRequest webRequest;
    public Action<object[]> onCompleteCallBack;

    public abstract void OnRequest();
    public abstract void OnResponsed();
}

public class HttpRawImageResourcePack : HttpBaseResourcePack
{
    public DownloadHandlerTexture downloadHandlerTexture;
    public RawImage targetRawImage;
    public Action<string, Texture> onLoadCallBack;

    public override void OnRequest()
    {
        webRequest = UnityWebRequest.Get(url);
        downloadHandlerTexture = new DownloadHandlerTexture();
        webRequest.downloadHandler = downloadHandlerTexture;
        webRequest.SendWebRequest();
    }

    public override void OnResponsed()
    {
        Texture downloadedTexture = downloadHandlerTexture.texture;
        if (targetRawImage != null)
        {
            targetRawImage.texture = downloadedTexture;
        }        
        if (onLoadCallBack != null)
        {
            onLoadCallBack(url,downloadedTexture);
        }
        webRequest.Abort();
        webRequest.Dispose();
        webRequest = null;
    }
}
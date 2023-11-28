using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum HttpResourceType
{
    RawImage,
    Audio
}

public abstract class HttpBaseResourcePack
{
    public uint packId;

    public string url;
    public int tryCount;
    public HttpResourceType resourceType;
    public UnityWebRequest webRequest;
    public Action<object[]> onCompleteCallBack;
    public Action<object[]> onCanceledCallBack;
    public Action<object[]> onErroredCallBack;

    public Func<uint, bool> onCompleteInvokePackRemoveCallBack;

    public abstract void OnRequest();
    public abstract void OnResponsed();
    public abstract void OnCanceled();
    public abstract void OnErrored();
    protected abstract void OnDisposed();
}

public class HttpRawImageResourcePack : HttpBaseResourcePack
{
    public DownloadHandlerTexture downloadHandlerTexture;
    public RawImage targetRawImage;
    public Action<string, Texture> onLoadCallBack;

    public override void OnCanceled()
    {
        OnDisposed();
        onCanceledCallBack?.Invoke(null);
    }

    protected override void OnDisposed()
    {
        if (webRequest == null) return;
        webRequest.Abort();
        webRequest.Dispose();
        webRequest = null;
    }

    public override void OnErrored()
    {
        OnDisposed();
        onErroredCallBack?.Invoke(null);
        onCompleteInvokePackRemoveCallBack?.Invoke(packId);
    }

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
            onLoadCallBack(url, downloadedTexture);
        }
        OnDisposed();
        onCompleteCallBack?.Invoke(null);
        onCompleteInvokePackRemoveCallBack?.Invoke(packId);
    }
}
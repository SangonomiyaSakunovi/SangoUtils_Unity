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
    public uint PackId { get; set; }

    public string Url { get; set; }
    public int TryCount { get; set; }
    public HttpResourceType ResourceType { get; set; }
    public UnityWebRequest WebRequest { get; set; }

    public Action<object[]> OnCompleteCallBack { get; set; }
    public Action<object[]> OnCanceledCallBack { get; set; }
    public Action<object[]> OnErroredCallBack { get; set; }

    public Func<uint, bool> OnCompleteInvokePackRemoveCallBack { get; set; }

    public abstract void OnRequest();
    public abstract void OnResponsed();
    public abstract void OnCanceled();
    public abstract void OnErrored();
    protected abstract void OnDisposed();
}

public class HttpRawImageResourcePack : HttpBaseResourcePack
{
    public DownloadHandlerTexture DownloadHandlerTexture { get; set; }
    public RawImage TargetRawImage { get; set; }
    public Action<string, Texture> OnLoadCallBack { get; set; }

    public override void OnCanceled()
    {
        OnDisposed();
        OnCanceledCallBack?.Invoke(null);
    }

    protected override void OnDisposed()
    {
        if (WebRequest == null) return;
        WebRequest.Abort();
        WebRequest.Dispose();
        WebRequest = null;
    }

    public override void OnErrored()
    {
        OnDisposed();
        OnErroredCallBack?.Invoke(null);
        OnCompleteInvokePackRemoveCallBack?.Invoke(PackId);
    }

    public override void OnRequest()
    {
        WebRequest = UnityWebRequest.Get(Url);
        DownloadHandlerTexture = new DownloadHandlerTexture();
        WebRequest.downloadHandler = DownloadHandlerTexture;
        WebRequest.SendWebRequest();
    }

    public override void OnResponsed()
    {
        Texture downloadedTexture = DownloadHandlerTexture.texture;
        if (TargetRawImage != null)
        {
            TargetRawImage.texture = downloadedTexture;
        }
        if (OnLoadCallBack != null)
        {
            OnLoadCallBack(Url, downloadedTexture);
        }
        OnDisposed();
        OnCompleteCallBack?.Invoke(null);
        OnCompleteInvokePackRemoveCallBack?.Invoke(PackId);
    }
}
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
    public string url;
    public bool isCache;
    public HttpResourceType resourceType;
    public UnityWebRequest webRequest;

    public abstract void OnRequest();
    public abstract void OnResponsed();
}

public class HttpRawImageResourcePack : HttpBaseResourcePack
{
    public DownloadHandlerTexture downloadHandlerTexture;
    public RawImage targetRawImage;

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
        if (isCache)
        {
            ResourceService.Instance.AddRawImageTextureCache(url, downloadedTexture);
        }
        webRequest.Abort();
        webRequest.Dispose();
        webRequest = null;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceService : BaseService<ResourceService>
{
    private ResourceRawImageLoader _resourceRawImageLoader = new ResourceRawImageLoader();

    private Dictionary<string, AudioClip> _audioClipDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();
    private Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();

    private Dictionary<string, Texture> _rawImageTextureDict = new Dictionary<string, Texture>();

    public override void OnInit()
    {
        base.OnInit();
    }

    public AudioClip LoadAudioClip(string audioPath, bool isCache)
    {
        _audioClipDict.TryGetValue(audioPath, out AudioClip audioClip);
        if (audioClip == null)
        {
            audioClip = Resources.Load<AudioClip>(audioPath);
            if (isCache)
            {
                if (!_audioClipDict.ContainsKey(audioPath))
                {
                    _audioClipDict.Add(audioPath, audioClip);
                }
            }
        }
        return audioClip;
    }

    public Sprite LoadSprite(string spritePath, bool isCache)
    {
        _spriteDict.TryGetValue(spritePath, out Sprite sprite);
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>(spritePath);
            if (isCache)
            {
                if (!_spriteDict.ContainsKey(spritePath))
                {
                    _spriteDict.Add(spritePath, sprite);
                }
            }
        }
        return sprite;
    }

    public GameObject LoadPrefab(string prefabPath, bool isCache)
    {
        _prefabDict.TryGetValue(prefabPath, out GameObject prefab);
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>(prefabPath);
            if (isCache)
            {
                if (!_prefabDict.ContainsKey(prefabPath))
                {
                    _prefabDict.Add(prefabPath, prefab);
                }
            }
        }
        return prefab;
    }

    public uint LoadAndSetRawImageOnlineAsync(RawImage targetRawImage, string urlPath, bool isCahce, Action<object[]> completeCallBack, Action<object[]> canceledCallBack, Action<object[]> erroredCallBack)
    {
        uint packId = 0;
        _rawImageTextureDict.TryGetValue(urlPath, out Texture texture);
        {
            if (texture == null)
            {
                packId = _resourceRawImageLoader.AddPack(targetRawImage, urlPath, isCahce, AddRawImageTextureCacheCB, completeCallBack, canceledCallBack, erroredCallBack);                
            }
            else
            {
                if (targetRawImage != null)
                {
                    targetRawImage.texture = texture;
                    completeCallBack?.Invoke(null);
                }
            }
        }
        return packId;
    }

    public bool RemoveRawImageOnlineAsyncPack(uint packId)
    {
        return _resourceRawImageLoader.RemovePack(packId);
    }

    private void AddRawImageTextureCacheCB(string urlPath, Texture texture)
    {
        if (!_rawImageTextureDict.ContainsKey(urlPath))
        {
            _rawImageTextureDict.Add(urlPath, texture);
        }
    }

    public void LoadAndSetGLTFModelOnlineAsync(GameObject parentObject, string urlPath)
    {
        ResourcePerticularService.Instance?.LoadAndSetGLTFModelOnlineAsync(parentObject, urlPath);
    }
}

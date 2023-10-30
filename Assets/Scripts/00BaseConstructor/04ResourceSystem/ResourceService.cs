using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceService : BaseService<ResourceService>
{
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

    public void LoadAndSetRawImageOnlineAsync(RawImage targetRawImage, string urlPath, bool isCahce)
    {
        _rawImageTextureDict.TryGetValue(urlPath,out Texture texture);
        {
            if (texture == null)
            {
                HttpRawImageResourcePack pack = new HttpRawImageResourcePack();
                pack.url = urlPath;
                pack.isCache = isCahce;
                pack.targetRawImage = targetRawImage;
                pack.resourceType = HttpResourceType.RawImage;
                HttpService.Instance?.HttpResource(pack);
            }
            else
            {
                if (targetRawImage != null)
                {
                    targetRawImage.texture = texture;
                }
            }
        }
    }

    public void AddRawImageTextureCache(string urlPath, Texture texture)
    {
        _rawImageTextureDict.Add(urlPath, texture);
    }
}

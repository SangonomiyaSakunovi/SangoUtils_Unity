using System.Collections.Generic;
using UnityEngine;

public class ResourceService : BaseService
{
    public static ResourceService Instance = null;

    private Dictionary<string, AudioClip> _audioClipDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();

    public override void InitService()
    {
        base.InitService();
        Instance = this;
    }

    public AudioClip LoadAudioClip(string audioPath, bool isCache)
    {
        _audioClipDict.TryGetValue(audioPath, out AudioClip audioClip);
        if (audioClip == null)
        {
            audioClip = Resources.Load<AudioClip>(audioPath);
            if (isCache)
            {
                _audioClipDict.Add(audioPath, audioClip);
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
                _spriteDict.Add(spritePath, sprite);
            }
        }
        return sprite;
    }
}

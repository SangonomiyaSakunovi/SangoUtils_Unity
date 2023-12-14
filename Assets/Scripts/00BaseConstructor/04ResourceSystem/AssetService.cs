using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class AssetService : BaseService<AssetService>
{
    private List<AssetHandle> _assetHandles;

    public override void OnInit()
    {
        base.OnInit();
        _assetHandles = new List<AssetHandle>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_assetHandles.Count > 0)
        {
            for (int i = _assetHandles.Count; i > -1; i--)
            {
                AssetHandle handle = _assetHandles[i];
                handle.Release();
                handle = null;
                _assetHandles.RemoveAt(i);
            }
            _assetHandles.Clear();
        }
    }

    public AudioClip LoadAudioClip(string path)
    {
        AssetHandle handle = YooAssets.LoadAssetSync<AudioClip>(path);
        AudioClip clip = handle.AssetObject as AudioClip;
        ReleaseAssetHandle(handle);
        return clip;
    }

    public void LoadAudioClipASync(string path, Action<AudioClip> callBack)
    {
        AssetHandle handle = YooAssets.LoadAssetAsync<AudioClip>(path);
        handle.Completed += (operation) =>
        {
            AudioClip clip = handle.AssetObject as AudioClip;
            callBack(clip);
            ReleaseAssetHandle(handle);
        };
    }

    public void ReleaseAssetHandle(AssetHandle handle)
    {
        _assetHandles.Add(handle);
    }
}

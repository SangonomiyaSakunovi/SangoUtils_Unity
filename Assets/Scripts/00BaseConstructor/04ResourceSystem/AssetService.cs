using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class AssetService : BaseService<AssetService>
{
    private List<AssetHandle> _cacheAssetHandles;

    private Dictionary<string, AssetHandle> _audioClipHandleDict;
    private Dictionary<string, AssetHandle> _spriteHandleDict;
    private Dictionary<string, AssetHandle> _prefabHandleDict;

    public override void OnInit()
    {
        base.OnInit();
        _cacheAssetHandles = new List<AssetHandle>();

        _audioClipHandleDict = new Dictionary<string, AssetHandle>();
        _spriteHandleDict = new Dictionary<string, AssetHandle>();
        _prefabHandleDict = new Dictionary<string, AssetHandle>();
    }

    public AudioClip LoadAudioClip(string audioClipPath, bool isCache)
    {
        AudioClip audioClip = null;
        _audioClipHandleDict.TryGetValue(audioClipPath, out AssetHandle handle);
        if (handle == null)
        {
            handle = YooAssets.LoadAssetSync<AudioClip>(audioClipPath);
            audioClip = handle.AssetObject as AudioClip;
            if (isCache)
            {
                if (!_audioClipHandleDict.ContainsKey(audioClipPath))
                {
                    _audioClipHandleDict.Add(audioClipPath, handle);
                }
            }
            else
            {
                GCAssetHandleTODO(handle);
            }
        }
        else
        {
            audioClip = handle.AssetObject as AudioClip;
        }
        return audioClip;
    }

    public void LoadAudioClipASync(string audioClipPath, Action<AudioClip> assetLoadedCallBack, bool isCache)
    {
        AudioClip audioClip = null;
        _audioClipHandleDict.TryGetValue(audioClipPath, out AssetHandle handle);
        if (handle == null)
        {
            handle = YooAssets.LoadAssetAsync<AudioClip>(audioClipPath);
            handle.Completed += (AssetHandle handle) =>
            {
                audioClip = handle.AssetObject as AudioClip;
                assetLoadedCallBack?.Invoke(audioClip);
                if (isCache)
                {
                    if (!_audioClipHandleDict.ContainsKey(audioClipPath))
                    {
                        _audioClipHandleDict.Add(audioClipPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            };
        }
        else
        {
            audioClip = handle.AssetObject as AudioClip;
            assetLoadedCallBack?.Invoke(audioClip);
        }
    }

    public GameObject LoadPrefab(string prefabPath, bool isCache)
    {
        GameObject prefab = null;
        _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
        if (handle == null)
        {
            handle = YooAssets.LoadAssetSync<GameObject>(prefabPath);
            prefab = handle.AssetObject as GameObject;
            if (isCache)
            {
                if (!_prefabHandleDict.ContainsKey(prefabPath))
                {
                    _prefabHandleDict.Add(prefabPath, handle);
                }
            }
            else
            {
                GCAssetHandleTODO(handle);
            }
        }
        else
        {
            prefab = handle.AssetObject as GameObject;
        }
        return prefab;
    }

    public void LoadPrefabASync(string prefabPath, Action<GameObject> assetLoadedCallBack, bool isCache)
    {
        GameObject prefab = null;
        _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
        if (handle == null)
        {
            handle = YooAssets.LoadAssetAsync<GameObject>(prefabPath);
            handle.Completed += (AssetHandle handle) =>
            {
                prefab = handle.AssetObject as GameObject;
                assetLoadedCallBack?.Invoke(prefab);
                if (isCache)
                {
                    if (!_prefabHandleDict.ContainsKey(prefabPath))
                    {
                        _prefabHandleDict.Add(prefabPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            };
        }
        else
        {
            prefab = handle.AssetObject as GameObject;
            assetLoadedCallBack?.Invoke(prefab);
        }
    }

    public GameObject InstantiatePrefab(Transform parentTrans, string prefabPath, bool isCache)
    {
        GameObject prefab = LoadPrefab(prefabPath, isCache);
        GameObject instantiatedPrefab = Instantiate(prefab, parentTrans);
        return instantiatedPrefab;
    }

    public void InstantiatePrefabASync(Transform parentTrans ,string prefabPath, Action<GameObject> assetLoadedCallBack, bool isCache)
    {
        GameObject prefab = null;
        _prefabHandleDict.TryGetValue(prefabPath, out AssetHandle handle);
        if (handle == null)
        {
            handle = YooAssets.LoadAssetAsync<GameObject>(prefabPath);
            handle.Completed += (AssetHandle handle) =>
            {
                prefab = handle.InstantiateSync(parentTrans);
                assetLoadedCallBack?.Invoke(prefab);
                if (isCache)
                {
                    if (!_prefabHandleDict.ContainsKey(prefabPath))
                    {
                        _prefabHandleDict.Add(prefabPath, handle);
                    }
                }
                else
                {
                    GCAssetHandleTODO(handle);
                }
            };
        }
        else
        {
            prefab = handle.AssetObject as GameObject;
            assetLoadedCallBack?.Invoke(prefab);
        }
    }

    private void GCAssetHandleTODO(AssetHandle assetHandle)
    {
        _cacheAssetHandles.Add(assetHandle);
    }

    public void ReleaseAssetHandles()
    {
        for (int i = 0; i < _cacheAssetHandles.Count; i++)
        {
            _cacheAssetHandles[i].Release();
        }
        _cacheAssetHandles.Clear();
    }
}

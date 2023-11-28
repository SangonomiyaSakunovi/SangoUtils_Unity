using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceRawImageLoader : BaseASyncPackManager
{
    private readonly Dictionary<uint, HttpRawImageResourcePack> _packDict;
    private const string _packIdLock = "ResourceRawImageLoader_Lock";

    public ResourceRawImageLoader()
    {
        _packDict = new Dictionary<uint, HttpRawImageResourcePack>();
    }

    public uint AddPack(RawImage targetRawImage, string urlPath, bool isCahce, Action<string, Texture> onLoadCallBack, Action<object[]> completeCallBack, Action<object[]> canceledCallBack, Action<object[]> erroredCallBack)
    {
        uint tempPackId = GeneratePackId();
        HttpRawImageResourcePack pack = new HttpRawImageResourcePack()
        {
            packId = tempPackId,
            url = urlPath,
            targetRawImage = targetRawImage,
            resourceType = HttpResourceType.RawImage,
            onCompleteCallBack = completeCallBack,
            onCanceledCallBack = canceledCallBack,
            onErroredCallBack = erroredCallBack,
            onCompleteInvokePackRemoveCallBack = RemovePackCallBack
        };
        if (isCahce)
        {
            pack.onLoadCallBack = onLoadCallBack;
        }
        _packDict.Add(tempPackId, pack);
        HttpService.Instance?.HttpResource(pack);
        return tempPackId;
    }

    public override bool RemovePack(uint packId)
    {
        if (_packDict.TryGetValue(packId, out var value))
        {
            value.OnCanceled();
            _packDict.Remove(packId);
            return true;
        }
        return false;
    }

    protected override uint GeneratePackId()
    {
        lock (_packIdLock)
        {
            while (true)
            {
                ++_packId;
                if (_packId == uint.MaxValue)
                {
                    _packId = 1;
                }
                if (!_packDict.ContainsKey(_packId))
                {
                    return _packId;
                }
            }
        }
    }

    public override bool RemovePackCallBack(uint packId)
    {
        Debug.Log("调用一次回调删除Pack操作");
        if (_packDict.TryGetValue(packId, out var value))
        {
            _packDict.Remove(packId);
            return true;
        }
        return false;
    }
}

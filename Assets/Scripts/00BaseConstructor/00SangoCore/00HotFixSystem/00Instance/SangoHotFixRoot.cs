using Unity.VisualScripting;
using UnityEngine;
using System;

public class SangoHotFixRoot : BaseRoot<SangoSecurityCheckRoot>
{
    private SangoHotFixWnd _sangoHotFixWnd;
    private EventCache _eventCache;

    private void Start()
    {
        EventService.Instance.OnInit();
        OnInit();
        HotFixService.Instance.OnInit();
    }

    public override void OnInit()
    {
        _sangoHotFixWnd = transform.Find("SangoHotFixWnd").GetOrAddComponent<SangoHotFixWnd>();
        base.OnInit();
        AddEventCache();
        _sangoHotFixWnd.SetRoot(this);
        _sangoHotFixWnd.SetWindowState();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        _eventCache.RemoveAllListeners();
    }

    private void AddEventCache()
    {
        _eventCache = new EventCache();
        _eventCache.AddEventListener<HotFixEventMessage.InitializeFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixEventMessage.PatchStatesChange>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixEventMessage.FoundUpdateFiles>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixEventMessage.DownloadProgressUpdate>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixEventMessage.PackageVersionUpdateFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixEventMessage.PatchManifestUpdateFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixEventMessage.WebFileDownloadFailed>(OnHandleEventMessage);
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        if (message is HotFixEventMessage.InitializeFailed)
        {
            Action callback = () =>
            {
                HotFixUserEventMessage.UserTryInitialize.SendEventMessage();
            };
            _sangoHotFixWnd.ShowMessageBox($"Failed to initialize package !", callback);
        }
        else if (message is HotFixEventMessage.PatchStatesChange)
        {
            var msg = message as HotFixEventMessage.PatchStatesChange;
            _sangoHotFixWnd.UpdateTips(msg.Tips);
        }
        else if (message is HotFixEventMessage.FoundUpdateFiles)
        {
            var msg = message as HotFixEventMessage.FoundUpdateFiles;
            Action callback = () =>
            {
                HotFixUserEventMessage.UserBeginDownloadWebFiles.SendEventMessage();
            };
            float sizeMB = msg.TotalSizeBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            _sangoHotFixWnd.ShowMessageBox($"Found update patch files, Total count {msg.TotalCount} Total szie {totalSizeMB}MB", callback);
        }
        else if (message is HotFixEventMessage.DownloadProgressUpdate)
        {
            var msg = message as HotFixEventMessage.DownloadProgressUpdate;
            float sliderValue = (float)msg.CurrentDownloadCount / msg.TotalDownloadCount;
            _sangoHotFixWnd.UpdateSliderValue(sliderValue);
            string currentSizeMB = (msg.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
            string totalSizeMB = (msg.TotalDownloadSizeBytes / 1048576f).ToString("f1");
            string tips = $"{msg.CurrentDownloadCount}/{msg.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
            _sangoHotFixWnd.UpdateTips(tips);
        }
        else if (message is HotFixEventMessage.PackageVersionUpdateFailed)
        {
            Action callback = () =>
            {
                HotFixUserEventMessage.UserTryUpdatePackageVersion.SendEventMessage();
            };
            _sangoHotFixWnd.ShowMessageBox($"Failed to update static version, please check the network status.", callback);
        }
        else if (message is HotFixEventMessage.PatchManifestUpdateFailed)
        {
            Action callback = () =>
            {
                HotFixUserEventMessage.UserTryUpdatePatchManifest.SendEventMessage();
            };
            _sangoHotFixWnd.ShowMessageBox($"Failed to update patch manifest, please check the network status.", callback);
        }
        else if (message is HotFixEventMessage.WebFileDownloadFailed)
        {
            var msg = message as HotFixEventMessage.WebFileDownloadFailed;
            Action callback = () =>
            {
                HotFixUserEventMessage.UserTryDownloadWebFiles.SendEventMessage();
            };
            _sangoHotFixWnd.ShowMessageBox($"Failed to download file : {msg.FileName}", callback);
        }
        else
        {
            throw new NotImplementedException($"{message.GetType()}");
        }
    }
}

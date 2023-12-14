using Unity.VisualScripting;
using UnityEngine;
using System;

public class SangoPatchRoot : BaseRoot<SangoSecurityCheckRoot>
{
    private SangoPatchWnd _sangoPatchWnd;
    private EventCache _eventCache;

    public override void OnInit()
    {
        _sangoPatchWnd = transform.Find("SangoPatchWnd").GetOrAddComponent<SangoPatchWnd>();
        base.OnInit();
        PatchService.Instance.OnInit();
        AddEventCache();
        _sangoPatchWnd.SetRoot(this);
        _sangoPatchWnd.SetWindowState();        
    }

    public override void OnDispose()
    {
        base.OnDispose();
        _eventCache.RemoveAllListeners();
    }

    private void AddEventCache()
    {
        _eventCache = new EventCache();
        _eventCache.AddEventListener<PatchSystemEventMessage.InitializeFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.PatchStatesChange>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.FoundUpdateFiles>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.DownloadProgressUpdate>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.PackageVersionUpdateFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.PatchManifestUpdateFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.WebFileDownloadFailed>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.ClosePatchWindow>(OnHandleEventMessage);
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        if (message is PatchSystemEventMessage.InitializeFailed)
        {
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryInitialize.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to initialize package !", callback);
        }
        else if (message is PatchSystemEventMessage.PatchStatesChange)
        {
            var msg = message as PatchSystemEventMessage.PatchStatesChange;
            _sangoPatchWnd.UpdateTips(msg.Tips);
        }
        else if (message is PatchSystemEventMessage.FoundUpdateFiles)
        {
            var msg = message as PatchSystemEventMessage.FoundUpdateFiles;
            Action callback = () =>
            {
                PatchUserEventMessage.UserBeginDownloadWebFiles.SendEventMessage();
            };
            float sizeMB = msg.TotalSizeBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            _sangoPatchWnd.ShowMessageBox($"Found update patch files, Total count {msg.TotalCount} Total szie {totalSizeMB}MB", callback);
        }
        else if (message is PatchSystemEventMessage.DownloadProgressUpdate)
        {
            var msg = message as PatchSystemEventMessage.DownloadProgressUpdate;
            float sliderValue = (float)msg.CurrentDownloadCount / msg.TotalDownloadCount;
            _sangoPatchWnd.UpdateSliderValue(sliderValue);
            string currentSizeMB = (msg.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
            string totalSizeMB = (msg.TotalDownloadSizeBytes / 1048576f).ToString("f1");
            string tips = $"{msg.CurrentDownloadCount}/{msg.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
            _sangoPatchWnd.UpdateTips(tips);
        }
        else if (message is PatchSystemEventMessage.PackageVersionUpdateFailed)
        {
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryUpdatePackageVersion.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to update static version, please check the network status.", callback);
        }
        else if (message is PatchSystemEventMessage.PatchManifestUpdateFailed)
        {
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryUpdatePatchManifest.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to update patch manifest, please check the network status.", callback);
        }
        else if (message is PatchSystemEventMessage.WebFileDownloadFailed)
        {
            var msg = message as PatchSystemEventMessage.WebFileDownloadFailed;
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryDownloadWebFiles.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to download file : {msg.FileName}", callback);
        }
        else if (message is PatchSystemEventMessage.ClosePatchWindow)
        {
            _sangoPatchWnd.SetWindowState(false);
        }
        else
        {
            throw new NotImplementedException($"{message.GetType()}");
        }
    }
}

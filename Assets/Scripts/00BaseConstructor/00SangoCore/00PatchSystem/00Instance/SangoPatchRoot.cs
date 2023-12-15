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
        _eventCache.AddEventListener<PatchSystemEventMessage.InitializeFailed_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.FoundUpdateFiles_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.DownloadProgressUpdate_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.PackageVersionUpdateFailed_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.PatchManifestUpdateFailed_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.WebFileDownloadFailed_PatchSystemEventMessage>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchSystemEventMessage.ClosePatchWindow_PatchSystemEventMessage>(OnHandleEventMessage);
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        if (message is PatchSystemEventMessage.InitializeFailed_PatchSystemEventMessage)
        {
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryInitialize_PatchUserEventMessage.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to initialize package !", callback);
        }
        else if (message is PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage)
        {
            var msg = message as PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage;
            _sangoPatchWnd.UpdateTips(msg.Tips);
        }
        else if (message is PatchSystemEventMessage.FoundUpdateFiles_PatchSystemEventMessage)
        {
            var msg = message as PatchSystemEventMessage.FoundUpdateFiles_PatchSystemEventMessage;
            Action callback = () =>
            {
                PatchUserEventMessage.UserBeginDownloadWebFiles_PatchUserEventMessage.SendEventMessage();
            };
            float sizeMB = msg.TotalSizeBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            _sangoPatchWnd.ShowMessageBox($"Found update patch files, Total count {msg.TotalCount} Total szie {totalSizeMB}MB", callback);
        }
        else if (message is PatchSystemEventMessage.DownloadProgressUpdate_PatchSystemEventMessage)
        {
            var msg = message as PatchSystemEventMessage.DownloadProgressUpdate_PatchSystemEventMessage;
            float sliderValue = (float)msg.CurrentDownloadCount / msg.TotalDownloadCount;
            _sangoPatchWnd.UpdateSliderValue(sliderValue);
            string currentSizeMB = (msg.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
            string totalSizeMB = (msg.TotalDownloadSizeBytes / 1048576f).ToString("f1");
            string tips = $"{msg.CurrentDownloadCount}/{msg.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
            _sangoPatchWnd.UpdateTips(tips);
        }
        else if (message is PatchSystemEventMessage.PackageVersionUpdateFailed_PatchSystemEventMessage)
        {
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryUpdatePackageVersion_PatchUserEventMessage.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to update static version, please check the network status.", callback);
        }
        else if (message is PatchSystemEventMessage.PatchManifestUpdateFailed_PatchSystemEventMessage)
        {
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryUpdatePatchManifest_PatchUserEventMessage.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to update patch manifest, please check the network status.", callback);
        }
        else if (message is PatchSystemEventMessage.WebFileDownloadFailed_PatchSystemEventMessage)
        {
            var msg = message as PatchSystemEventMessage.WebFileDownloadFailed_PatchSystemEventMessage;
            Action callback = () =>
            {
                PatchUserEventMessage.UserTryDownloadWebFiles_PatchUserEventMessage.SendEventMessage();
            };
            _sangoPatchWnd.ShowMessageBox($"Failed to download file : {msg.FileName}", callback);
        }
        else if (message is PatchSystemEventMessage.ClosePatchWindow_PatchSystemEventMessage)
        {
            _sangoPatchWnd.SetWindowState(false);
        }
        else
        {
            throw new NotImplementedException($"{message.GetType()}");
        }
    }
}

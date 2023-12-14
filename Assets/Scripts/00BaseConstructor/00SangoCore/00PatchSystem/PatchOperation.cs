using System;
using YooAsset;

public class PatchOperation : GameAsyncOperation
{
    private PatchConfig _currentPatchConfig;

    private EventCache _eventCache;
    private FSMLinkedStater _fsmLinkedStater;

    private ESteps _steps = ESteps.None;

    public PatchOperation(PatchConfig patchConfig)
    {
        _currentPatchConfig = patchConfig;
        _eventCache = new EventCache();
        AddEventListener();
        AddFSMStater();
    }

    protected override void OnAbort() { }

    protected override void OnStart()
    {
        _steps = ESteps.Update;
        _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_InitializePackage>(true);
    }

    protected override void OnUpdate()
    {
        if (_steps == ESteps.None || _steps == ESteps.Done)
            return;

        if (_steps == ESteps.Update)
        {
            _fsmLinkedStater.UpdateCurrentStaterItem();
            if (_fsmLinkedStater.CurrentStaterName == typeof(PatchLinkedFSM_UpdaterDone).FullName)
            {
                _eventCache.RemoveAllListeners();
                Status = EOperationStatus.Succeed;
                _steps = ESteps.Done;
            }
        }
    }

    private void AddEventListener()
    {
        _eventCache.AddEventListener<PatchUserEventMessage.UserTryInitialize>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchUserEventMessage.UserBeginDownloadWebFiles>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchUserEventMessage.UserTryUpdatePackageVersion>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchUserEventMessage.UserTryUpdatePatchManifest>(OnHandleEventMessage);
        _eventCache.AddEventListener<PatchUserEventMessage.UserTryDownloadWebFiles>(OnHandleEventMessage);
    }

    private void AddFSMStater()
    {
        _fsmLinkedStater = new FSMLinkedStater(this);
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_InitializePackage>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_UpdatePackageVersion>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_UpdatePackageManifest>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_DownloadPackageFiles>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_DownloadPackageOver>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_ClearPackageCache>();
        _fsmLinkedStater.AddStaterItem<PatchLinkedFSM_UpdaterDone>();

        _fsmLinkedStater.SetBlackboardValue("PackageName", _currentPatchConfig.packageName);
        _fsmLinkedStater.SetBlackboardValue("PlayMode", _currentPatchConfig.playMode);
        _fsmLinkedStater.SetBlackboardValue("BuildPipeline", _currentPatchConfig.buildPipeline.ToString());
        _fsmLinkedStater.SetBlackboardValue("HostServerIP", _currentPatchConfig.hostServerIP);
        _fsmLinkedStater.SetBlackboardValue("AppId", _currentPatchConfig.appId);
        _fsmLinkedStater.SetBlackboardValue("AppVersion", _currentPatchConfig.appVersion);
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        if (message is PatchUserEventMessage.UserTryInitialize)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_InitializePackage>();
        }
        else if (message is PatchUserEventMessage.UserBeginDownloadWebFiles)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageFiles>();
        }
        else if (message is PatchUserEventMessage.UserTryUpdatePackageVersion)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageVersion>();
        }
        else if (message is PatchUserEventMessage.UserTryUpdatePatchManifest)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageManifest>();
        }
        else if (message is PatchUserEventMessage.UserTryDownloadWebFiles)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
        }
        else
        {
            throw new NotImplementedException($"{message.GetType()}");
        }
    }

    private enum ESteps
    {
        None,
        Update,
        Done,
    }
}

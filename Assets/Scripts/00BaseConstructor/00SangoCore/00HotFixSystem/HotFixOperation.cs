using System;
using YooAsset;

public class HotFixOperation : GameAsyncOperation
{
    private HotFixConfig _currentHotFixConfig;

    private EventCache _eventCache;
    private FSMLinkedStater _fsmLinkedStater;

    private ESteps _steps = ESteps.None;

    public HotFixOperation(HotFixConfig hotFixConfig)
    {
        _currentHotFixConfig = hotFixConfig;
        _eventCache = new EventCache();
        AddEventListener();
        AddFSMStater();
    }

    protected override void OnAbort() { }

    protected override void OnStart()
    {
        _steps = ESteps.Update;
        _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_InitializePackage>(true);
    }

    protected override void OnUpdate()
    {
        if (_steps == ESteps.None || _steps == ESteps.Done)
            return;

        if (_steps == ESteps.Update)
        {
            _fsmLinkedStater.UpdateCurrentStaterItem();
            if (_fsmLinkedStater.CurrentStaterName == typeof(HotFixLinkedFSM_UpdaterDone).FullName)
            {
                _eventCache.RemoveAllListeners();
                Status = EOperationStatus.Succeed;
                _steps = ESteps.Done;
            }
        }
    }

    private void AddEventListener()
    {
        _eventCache.AddEventListener<HotFixUserEventMessage.UserTryInitialize>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixUserEventMessage.UserBeginDownloadWebFiles>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixUserEventMessage.UserTryUpdatePackageVersion>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixUserEventMessage.UserTryUpdatePatchManifest>(OnHandleEventMessage);
        _eventCache.AddEventListener<HotFixUserEventMessage.UserTryDownloadWebFiles>(OnHandleEventMessage);
    }

    private void AddFSMStater()
    {
        _fsmLinkedStater = new FSMLinkedStater(this);
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_InitializePackage>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_UpdatePackageVersion>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_UpdatePackageManifest>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_CreatePackageDownloader>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_DownloadPackageFiles>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_DownloadPackageOver>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_ClearPackageCache>();
        _fsmLinkedStater.AddStaterItem<HotFixLinkedFSM_UpdaterDone>();

        _fsmLinkedStater.SetBlackboardValue("PackageName", _currentHotFixConfig.packageName);
        _fsmLinkedStater.SetBlackboardValue("PlayMode", _currentHotFixConfig.playMode);
        _fsmLinkedStater.SetBlackboardValue("BuildPipeline", _currentHotFixConfig.buildPipeline.ToString());
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        if (message is HotFixUserEventMessage.UserTryInitialize)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_InitializePackage>();
        }
        else if (message is HotFixUserEventMessage.UserBeginDownloadWebFiles)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_DownloadPackageFiles>();
        }
        else if (message is HotFixUserEventMessage.UserTryUpdatePackageVersion)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_UpdatePackageVersion>();
        }
        else if (message is HotFixUserEventMessage.UserTryUpdatePatchManifest)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_UpdatePackageManifest>();
        }
        else if (message is HotFixUserEventMessage.UserTryDownloadWebFiles)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_CreatePackageDownloader>();
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

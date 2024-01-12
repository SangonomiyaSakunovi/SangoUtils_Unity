using System;
using YooAsset;

namespace SangoScripts_Unity.Patch
{
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
            _eventCache.AddEventListener<PatchUserEventMessage.UserTryInitialize_PatchUserEventMessage>(OnHandleEventMessage);
            _eventCache.AddEventListener<PatchUserEventMessage.UserBeginDownloadWebFiles_PatchUserEventMessage>(OnHandleEventMessage);
            _eventCache.AddEventListener<PatchUserEventMessage.UserTryUpdatePackageVersion_PatchUserEventMessage>(OnHandleEventMessage);
            _eventCache.AddEventListener<PatchUserEventMessage.UserTryUpdatePatchManifest_PatchUserEventMessage>(OnHandleEventMessage);
            _eventCache.AddEventListener<PatchUserEventMessage.UserTryDownloadWebFiles_PatchUserEventMessage>(OnHandleEventMessage);
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

            _fsmLinkedStater.SetBlackboardValue("PackageName", _currentPatchConfig.PackageName);
            _fsmLinkedStater.SetBlackboardValue("PlayMode", _currentPatchConfig.PlayMode);
            _fsmLinkedStater.SetBlackboardValue("BuildPipeline", _currentPatchConfig.BuildPipeline.ToString());
            _fsmLinkedStater.SetBlackboardValue("HostServerIP", _currentPatchConfig.HostServerIP);
            _fsmLinkedStater.SetBlackboardValue("AppId", _currentPatchConfig.AppId);
            _fsmLinkedStater.SetBlackboardValue("AppVersion", _currentPatchConfig.AppVersion);
        }

        private void OnHandleEventMessage(IEventMessageBase message)
        {
            if (message is PatchUserEventMessage.UserTryInitialize_PatchUserEventMessage)
            {
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_InitializePackage>();
            }
            else if (message is PatchUserEventMessage.UserBeginDownloadWebFiles_PatchUserEventMessage)
            {
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageFiles>();
            }
            else if (message is PatchUserEventMessage.UserTryUpdatePackageVersion_PatchUserEventMessage)
            {
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageVersion>();
            }
            else if (message is PatchUserEventMessage.UserTryUpdatePatchManifest_PatchUserEventMessage)
            {
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdatePackageManifest>();
            }
            else if (message is PatchUserEventMessage.UserTryDownloadWebFiles_PatchUserEventMessage)
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
}
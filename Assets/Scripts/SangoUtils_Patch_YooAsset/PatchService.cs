using SangoUtils_Bases_UnityEngine;
using SangoUtils_Bases_Universal;
using SangoUtils_Extensions_UnityEngine.Core;
using System;
using System.Collections;
using YooAsset;

namespace SangoUtils_Patch_YooAsset
{
    public class PatchService : BaseService<PatchService> 
    {
        private PatchConfig _currentPatchConfig;

        private CoroutineHandler coroutine = null;

        public Action<bool> OnPatchCompleted { get; set; }

        public override void OnInit()
        {
            base.OnInit();
            coroutine = StartOperation().Start();
        }

        public void SetConfig(PatchConfig patchConfig)
        {
            _currentPatchConfig = patchConfig;
        }

        private IEnumerator StartOperation()
        {
            YooAssets.Initialize();

            PatchOperation hotFixOperation = new PatchOperation(_currentPatchConfig);
            YooAssets.StartOperation(hotFixOperation);
            yield return hotFixOperation;

            ResourcePackage assetPackage = YooAssets.GetPackage(_currentPatchConfig.PackageName);
            YooAssets.SetDefaultPackage(assetPackage);

            PatchSystemEventMessage.ClosePatchWindow_PatchSystemEventMessage.SendEventMessage();
            OnPatchCompleted?.Invoke(true);
        }
    }

    public class PatchConfig : BaseConfig
    {
        public string HostServerIP { get; set; }
        public string AppId { get; set; }
        public string AppVersion { get; set; }

        public string PackageName { get; set; }
        public EPlayMode PlayMode { get; set; }
        public EDefaultBuildPipeline BuildPipeline { get; set; }
    }
}
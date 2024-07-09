using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_UpdatePackageVersion : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.UpdatePackageVersion;

        internal override void OnEvent()
        {
            EventBus_Patchs.CallPatchSystemEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "获取最新的资源版本!"));
            _UpdatePackageVersionASync().Start();
        }

        private IEnumerator _UpdatePackageVersionASync()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = EventBus_Patchs.PatchConfig.PackageName;
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageVersionAsync();
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                EventBus_Patchs.CallPatchSystemEvent(this, new PatchSystemEventArgs(PatchSystemEventCode.PackageVersionUpdateFailed));
            }
            else
            {
                EventBus_Patchs.PatchConfig.PackageVersion = operation.PackageVersion;
                EventBus_Patchs.CallPatchOperationEvent(this, new PatchOperationEventArgs(PatchOperationEventCode.UpdatePackageManifest));
            }
        }
    }
}
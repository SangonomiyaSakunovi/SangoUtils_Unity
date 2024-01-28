using SangoUtils_Extensions_UnityEngine.Core;
using SangoUtils_FSM;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils_Patch_YooAsset
{
    public class PatchLinkedFSM_UpdatePackageManifest : FSMLinkedStaterItemBase
    {
        private CoroutineHandler coroutine = null;

        public override void OnEnter()
        {
            PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage.SendEventMessage("更新资源清单！");
            coroutine = UpdateManifest().Start();
        }

        private IEnumerator UpdateManifest()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var packageVersion = (string)_fsmLinkedStater.GetBlackboardValue("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            bool savePackageVersion = true;
            var operation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                PatchSystemEventMessage.PatchManifestUpdateFailed_PatchSystemEventMessage.SendEventMessage();
                yield break;
            }
            else
            {
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
            }
        }
    }
}
using System.Collections;
using UnityEngine;
using YooAsset;

public class PatchLinkedFSM_UpdatePackageManifest : FSMLinkedStaterItemBase
{
    private CoroutineHandler coroutine = null;

    public override void OnEnter()
    {
        PatchSystemEventMessage.PatchStatesChange.SendEventMessage("更新资源清单！");
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
            PatchSystemEventMessage.PatchManifestUpdateFailed.SendEventMessage();
            yield break;
        }
        else
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_CreatePackageDownloader>();
        }
    }
}


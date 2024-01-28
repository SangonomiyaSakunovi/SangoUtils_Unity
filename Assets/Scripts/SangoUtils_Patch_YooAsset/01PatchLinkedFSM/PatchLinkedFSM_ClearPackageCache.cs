using SangoUtils_FSM;
using YooAsset;

namespace SangoUtils_Patch_YooAsset
{
    public class PatchLinkedFSM_ClearPackageCache : FSMLinkedStaterItemBase
    {
        public override void OnEnter()
        {
            PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage.SendEventMessage("清理未使用的缓存文件！");
            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearUnusedCacheFilesAsync();
            operation.Completed += Operation_Completed;
        }

        private void Operation_Completed(AsyncOperationBase obj)
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdaterDone>();
        }
    }
}
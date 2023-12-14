using YooAsset;

public class PatchLinkedFSM_ClearPackageCache : FSMLinkedStaterItemBase
{
    public override void OnEnter()
    {
        PatchSystemEventMessage.PatchStatesChange.SendEventMessage("����δʹ�õĻ����ļ���");
        var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
        var package = YooAssets.GetPackage(packageName);
        var operation = package.ClearUnusedCacheFilesAsync();
        operation.Completed += Operation_Completed;
    }

    private void Operation_Completed(YooAsset.AsyncOperationBase obj)
    {
        _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdaterDone>();
    }
}

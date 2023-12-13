public class HotFixLinkedFSM_DownloadPackageOver : FSMLinkedStaterItemBase
{
    public override void OnEnter()
    {
        _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_ClearPackageCache>();
    }
}

namespace SangoScripts_Unity.Patch
{
    public class PatchLinkedFSM_DownloadPackageOver : FSMLinkedStaterItemBase
    {
        public override void OnEnter()
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_ClearPackageCache>();
        }
    }
}
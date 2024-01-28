using SangoUtils_FSM;

namespace SangoUtils_Patch_YooAsset
{
    public class PatchLinkedFSM_DownloadPackageOver : FSMLinkedStaterItemBase
    {
        public override void OnEnter()
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_ClearPackageCache>();
        }
    }
}
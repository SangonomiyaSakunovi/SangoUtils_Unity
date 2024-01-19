using SangoUtils_FSM;

namespace SangoUtils_Unity_Scripts.Patch
{
    public class PatchLinkedFSM_DownloadPackageOver : FSMLinkedStaterItemBase
    {
        public override void OnEnter()
        {
            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_ClearPackageCache>();
        }
    }
}
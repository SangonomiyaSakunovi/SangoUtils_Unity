using SangoUtils_Extensions_UnityEngine.Core;
using SangoUtils_FSM;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils_Patch_YooAsset
{
    public class PatchLinkedFSM_CreatePackageDownloader : FSMLinkedStaterItemBase
    {
        private CoroutineHandler coroutine = null;

        public override void OnEnter()
        {
            PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage.SendEventMessage("����������������");
            coroutine = CreateDownloader().Start();
        }

        private IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = (string)_fsmLinkedStater.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            _fsmLinkedStater.SetBlackboardValue("Downloader", downloader);

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("Not found any download files !");
                _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_UpdaterDone>();
            }
            else
            {
                // �����¸����ļ��󣬹�������ϵͳ
                // ע�⣺��������Ҫ������ǰ�����̿ռ䲻��
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                PatchSystemEventMessage.FoundUpdateFiles_PatchSystemEventMessage.SendEventMessage(totalDownloadCount, totalDownloadBytes);
            }
        }
    }
}
using SangoUtils.Patchs_YooAsset.Utils;
using System.Collections;
using UnityEngine;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchOperationOP_CreatePackageDownloader : PatchOperationOP_Base
    {
        internal override PatchOperationEventCode PatchOperationEventCode => PatchOperationEventCode.CreatePackageDownloader;

        internal override void OnEvent()
        {
            EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.PatchStatesChange, "����������������"));
            CreateDownloader().Start();
        }

        private IEnumerator CreateDownloader()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var packageName = EventBus_Patchs.PatchOperation.PatchOperationData.PackageName;
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

            EventBus_Patchs.PatchOperation.PatchOperationData.ResourceDownloaderOperation = downloader;

            if (downloader.TotalDownloadCount == 0)
            {
                Debug.Log("Not found any download files !");
                EventBus_Patchs.PatchOperation.SendMessage(this, new PatchOperationEventArgs(PatchOperationEventCode.UpdaterDone));
            }
            else
            {
                // �����¸����ļ��󣬹�������ϵͳ
                // ע�⣺��������Ҫ������ǰ�����̿ռ䲻��
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                EventBus_Patchs.SangoPatchRoot.SendMessage(this, new PatchSystemEventArgs(PatchSystemEventCode.FoundUpdateFiles, totalDownloadCount, totalDownloadBytes));
            }
        }
    }
}
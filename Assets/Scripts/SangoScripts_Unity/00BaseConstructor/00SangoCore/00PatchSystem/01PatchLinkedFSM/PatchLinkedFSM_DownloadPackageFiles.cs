using SangoUtils_Extensions_UnityEngine.Core;
using SangoUtils_FSM;
using System.Collections;
using YooAsset;

namespace SangoScripts_Unity.Patch
{
    public class PatchLinkedFSM_DownloadPackageFiles : FSMLinkedStaterItemBase
    {
        private CoroutineHandler coroutine = null;

        public override void OnEnter()
        {
            PatchSystemEventMessage.PatchStatesChange_PatchSystemEventMessage.SendEventMessage("开始下载补丁文件！");
            coroutine = BeginDownload().Start();
        }

        private IEnumerator BeginDownload()
        {
            var downloader = (ResourceDownloaderOperation)_fsmLinkedStater.GetBlackboardValue("Downloader");
            downloader.OnDownloadErrorCallback = PatchSystemEventMessage.WebFileDownloadFailed_PatchSystemEventMessage.SendEventMessage;
            downloader.OnDownloadProgressCallback = PatchSystemEventMessage.DownloadProgressUpdate_PatchSystemEventMessage.SendEventMessage;
            downloader.BeginDownload();
            yield return downloader;

            if (downloader.Status != EOperationStatus.Succeed)
                yield break;

            _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageOver>();
        }
    }
}
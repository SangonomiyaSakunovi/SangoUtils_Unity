using System.Collections;
using YooAsset;

public class PatchLinkedFSM_DownloadPackageFiles : FSMLinkedStaterItemBase
{
    private CoroutineHandler coroutine = null;

    public override void OnEnter()
    {
        PatchSystemEventMessage.PatchStatesChange.SendEventMessage("开始下载补丁文件！");
        coroutine = BeginDownload().Start();
    }

    private IEnumerator BeginDownload()
    {
        var downloader = (ResourceDownloaderOperation)_fsmLinkedStater.GetBlackboardValue("Downloader");
        downloader.OnDownloadErrorCallback = PatchSystemEventMessage.WebFileDownloadFailed.SendEventMessage;
        downloader.OnDownloadProgressCallback = PatchSystemEventMessage.DownloadProgressUpdate.SendEventMessage;
        downloader.BeginDownload();
        yield return downloader;

        // 检测下载结果
        if (downloader.Status != EOperationStatus.Succeed)
            yield break;

        _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageOver>();
    }
}

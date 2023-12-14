using System.Collections;
using YooAsset;

public class PatchLinkedFSM_DownloadPackageFiles : FSMLinkedStaterItemBase
{
    private CoroutineHandler coroutine = null;

    public override void OnEnter()
    {
        PatchSystemEventMessage.PatchStatesChange.SendEventMessage("��ʼ���ز����ļ���");
        coroutine = BeginDownload().Start();
    }

    private IEnumerator BeginDownload()
    {
        var downloader = (ResourceDownloaderOperation)_fsmLinkedStater.GetBlackboardValue("Downloader");
        downloader.OnDownloadErrorCallback = PatchSystemEventMessage.WebFileDownloadFailed.SendEventMessage;
        downloader.OnDownloadProgressCallback = PatchSystemEventMessage.DownloadProgressUpdate.SendEventMessage;
        downloader.BeginDownload();
        yield return downloader;

        // ������ؽ��
        if (downloader.Status != EOperationStatus.Succeed)
            yield break;

        _fsmLinkedStater.InvokeTargetStaterItem<PatchLinkedFSM_DownloadPackageOver>();
    }
}

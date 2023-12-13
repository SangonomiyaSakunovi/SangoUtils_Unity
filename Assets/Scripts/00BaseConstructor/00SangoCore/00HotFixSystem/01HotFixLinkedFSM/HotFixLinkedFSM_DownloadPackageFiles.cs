using System.Collections;
using YooAsset;

public class HotFixLinkedFSM_DownloadPackageFiles : FSMLinkedStaterItemBase
{
    private CoroutineHandler coroutine = null;

    public override void OnEnter()
    {
        HotFixEventMessage.PatchStatesChange.SendEventMessage("��ʼ���ز����ļ���");
        coroutine = BeginDownload().Start();
    }

    private IEnumerator BeginDownload()
    {
        var downloader = (ResourceDownloaderOperation)_fsmLinkedStater.GetBlackboardValue("Downloader");
        downloader.OnDownloadErrorCallback = HotFixEventMessage.WebFileDownloadFailed.SendEventMessage;
        downloader.OnDownloadProgressCallback = HotFixEventMessage.DownloadProgressUpdate.SendEventMessage;
        downloader.BeginDownload();
        yield return downloader;

        // ������ؽ��
        if (downloader.Status != EOperationStatus.Succeed)
            yield break;

        _fsmLinkedStater.InvokeTargetStaterItem<HotFixLinkedFSM_DownloadPackageOver>();
    }
}

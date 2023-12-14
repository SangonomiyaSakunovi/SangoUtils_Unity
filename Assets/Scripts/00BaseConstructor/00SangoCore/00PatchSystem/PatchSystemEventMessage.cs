public class PatchSystemEventMessage
{
    /// <summary>
	/// ��������ʼ��ʧ��
	/// </summary>
	public class InitializeFailed : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new InitializeFailed();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �������̲���ı�
    /// </summary>
    public class PatchStatesChange : IEventMessageBase
    {
        public string Tips;

        public static void SendEventMessage(string tips)
        {
            var msg = new PatchStatesChange();
            msg.Tips = tips;
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// ���ָ����ļ�
    /// </summary>
    public class FoundUpdateFiles : IEventMessageBase
    {
        public int TotalCount;
        public long TotalSizeBytes;

        public static void SendEventMessage(int totalCount, long totalSizeBytes)
        {
            var msg = new FoundUpdateFiles();
            msg.TotalCount = totalCount;
            msg.TotalSizeBytes = totalSizeBytes;
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// ���ؽ��ȸ���
    /// </summary>
    public class DownloadProgressUpdate : IEventMessageBase
    {
        public int TotalDownloadCount;
        public int CurrentDownloadCount;
        public long TotalDownloadSizeBytes;
        public long CurrentDownloadSizeBytes;

        public static void SendEventMessage(int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeBytes, long currentDownloadSizeBytes)
        {
            var msg = new DownloadProgressUpdate();
            msg.TotalDownloadCount = totalDownloadCount;
            msg.CurrentDownloadCount = currentDownloadCount;
            msg.TotalDownloadSizeBytes = totalDownloadSizeBytes;
            msg.CurrentDownloadSizeBytes = currentDownloadSizeBytes;
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// ��Դ�汾�Ÿ���ʧ��
    /// </summary>
    public class PackageVersionUpdateFailed : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new PackageVersionUpdateFailed();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �����嵥����ʧ��
    /// </summary>
    public class PatchManifestUpdateFailed : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new PatchManifestUpdateFailed();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �����ļ�����ʧ��
    /// </summary>
    public class WebFileDownloadFailed : IEventMessageBase
    {
        public string FileName;
        public string Error;

        public static void SendEventMessage(string fileName, string error)
        {
            var msg = new WebFileDownloadFailed();
            msg.FileName = fileName;
            msg.Error = error;
            EventService.Instance.SendEventMessage(msg);
        }
    }

    public class ClosePatchWindow : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new ClosePatchWindow();
            EventService.Instance.SendEventMessage(msg);
        }
    }
}

public class PatchUserEventMessage
{
    /// <summary>
	/// �û������ٴγ�ʼ����Դ��
	/// </summary>
	public class UserTryInitialize : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryInitialize();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �û���ʼ���������ļ�
    /// </summary>
    public class UserBeginDownloadWebFiles : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new UserBeginDownloadWebFiles();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �û������ٴθ��¾�̬�汾
    /// </summary>
    public class UserTryUpdatePackageVersion : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryUpdatePackageVersion();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �û������ٴθ��²����嵥
    /// </summary>
    public class UserTryUpdatePatchManifest : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryUpdatePatchManifest();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    /// <summary>
    /// �û������ٴ����������ļ�
    /// </summary>
    public class UserTryDownloadWebFiles : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryDownloadWebFiles();
            EventService.Instance.SendEventMessage(msg);
        }
    }
}

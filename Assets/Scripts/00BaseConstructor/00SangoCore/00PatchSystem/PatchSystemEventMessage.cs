public class PatchSystemEventMessage
{
    /// <summary>
	/// 补丁包初始化失败
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
    /// 补丁流程步骤改变
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
    /// 发现更新文件
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
    /// 下载进度更新
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
    /// 资源版本号更新失败
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
    /// 补丁清单更新失败
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
    /// 网络文件下载失败
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
	/// 用户尝试再次初始化资源包
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
    /// 用户开始下载网络文件
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
    /// 用户尝试再次更新静态版本
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
    /// 用户尝试再次更新补丁清单
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
    /// 用户尝试再次下载网络文件
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

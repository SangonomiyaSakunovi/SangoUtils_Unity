using System;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchSystemEventArgs : EventArgs
    {
        internal PatchSystemEventArgs(PatchSystemEventCode patchSystemEventCode, params object[] data)
        {
            PatchSystemEventCode = patchSystemEventCode;
            ExtensionData = data;
        }

        internal PatchSystemEventCode PatchSystemEventCode { get; }
        internal object[] ExtensionData { get; }
    }

    internal enum PatchSystemEventCode
    {
        InitializeFailed,
        PatchStatesChange,
        FoundUpdateFiles,
        PackageVersionUpdateFailed,
        PatchManifestUpdateFailed,
        WebFileDownloadFailed,
        ClosePatchWindow
    }

    internal class PatchSystem_DownloadProgressUpdateEventArgs : EventArgs
    {
        public PatchSystem_DownloadProgressUpdateEventArgs(int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeBytes, long currentDownloadSizeBytes)
        {
            TotalDownloadCount = totalDownloadCount;
            CurrentDownloadCount = currentDownloadCount;
            TotalDownloadSizeBytes = totalDownloadSizeBytes;
            CurrentDownloadSizeBytes = currentDownloadSizeBytes;
        }

        internal int TotalDownloadCount { get; }
        internal int CurrentDownloadCount { get; }
        internal long TotalDownloadSizeBytes { get; }
        internal long CurrentDownloadSizeBytes { get; }
    }

    internal class PatchUserEventArgs : EventArgs
    {
        internal PatchUserEventArgs(PatchUserEventCode patchUserEventCode)
        {
            PatchUserEventCode = patchUserEventCode;
        }

        internal PatchUserEventCode PatchUserEventCode { get; }
    }

    internal enum PatchUserEventCode
    {
        UserTryInitialize,
        UserBeginDownloadWebFiles,
        UserTryUpdatePackageVersion,
        UserTryUpdatePatchManifest,
        UserTryDownloadWebFiles
    }

    internal class PatchOperationEventArgs : EventArgs
    {
        internal PatchOperationEventArgs(PatchOperationEventCode patchOperationEventCode)
        {
            PatchOperationEventCode = patchOperationEventCode;
        }

        internal PatchOperationEventCode PatchOperationEventCode { get; }
    }

    internal enum PatchOperationEventCode
    {
        InitializePackage,
        UpdatePackageVersion,
        UpdatePackageManifest,
        CreatePackageDownloader,
        DownloadPackageFiles,
        DownloadPackageOver,
        ClearPackageCache,
        UpdaterDone
    }

    internal abstract class PatchOperationOP_Base
    {
        internal abstract PatchOperationEventCode PatchOperationEventCode { get; }

        internal abstract void OnEvent();
    }

    internal class PatchOperationData
    {
        internal PatchOperationData(string packageName, EPlayMode playMode, string buildPipline, string hostServerIP, string appID, string appVersion, Action onUpdaterDone)
        {
            PackageName = packageName;
            PlayMode = playMode;
            BuildPipline = buildPipline;
            HostServerIP = hostServerIP;
            AppID = appID;
            AppVersion = appVersion;
            OnUpdaterDone = onUpdaterDone;
        }

        internal string PackageName { get; }
        internal EPlayMode PlayMode { get; }
        internal string BuildPipline { get; }
        internal string HostServerIP { get; }
        internal string AppID { get; }
        internal string AppVersion { get; }
        internal Action OnUpdaterDone { get; }

        internal string PackageVersion { get; set; }
        internal ResourceDownloaderOperation ResourceDownloaderOperation { get; set; }
    }
}
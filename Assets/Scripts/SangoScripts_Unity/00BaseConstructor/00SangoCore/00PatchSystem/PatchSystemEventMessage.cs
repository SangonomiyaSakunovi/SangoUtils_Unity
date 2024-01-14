using SangoUtils_Event;

namespace SangoScripts_Unity.Patch
{
    public class PatchSystemEventMessage
    {
        public class InitializeFailed_PatchSystemEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new InitializeFailed_PatchSystemEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class PatchStatesChange_PatchSystemEventMessage : IEventMessageBase
        {
            public string Tips;

            public static void SendEventMessage(string tips)
            {
                var msg = new PatchStatesChange_PatchSystemEventMessage();
                msg.Tips = tips;
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class FoundUpdateFiles_PatchSystemEventMessage : IEventMessageBase
        {
            public int TotalCount;
            public long TotalSizeBytes;

            public static void SendEventMessage(int totalCount, long totalSizeBytes)
            {
                var msg = new FoundUpdateFiles_PatchSystemEventMessage();
                msg.TotalCount = totalCount;
                msg.TotalSizeBytes = totalSizeBytes;
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class DownloadProgressUpdate_PatchSystemEventMessage : IEventMessageBase
        {
            public int TotalDownloadCount;
            public int CurrentDownloadCount;
            public long TotalDownloadSizeBytes;
            public long CurrentDownloadSizeBytes;

            public static void SendEventMessage(int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeBytes, long currentDownloadSizeBytes)
            {
                var msg = new DownloadProgressUpdate_PatchSystemEventMessage();
                msg.TotalDownloadCount = totalDownloadCount;
                msg.CurrentDownloadCount = currentDownloadCount;
                msg.TotalDownloadSizeBytes = totalDownloadSizeBytes;
                msg.CurrentDownloadSizeBytes = currentDownloadSizeBytes;
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class PackageVersionUpdateFailed_PatchSystemEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new PackageVersionUpdateFailed_PatchSystemEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class PatchManifestUpdateFailed_PatchSystemEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new PatchManifestUpdateFailed_PatchSystemEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class WebFileDownloadFailed_PatchSystemEventMessage : IEventMessageBase
        {
            public string FileName;
            public string Error;

            public static void SendEventMessage(string fileName, string error)
            {
                var msg = new WebFileDownloadFailed_PatchSystemEventMessage();
                msg.FileName = fileName;
                msg.Error = error;
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class ClosePatchWindow_PatchSystemEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new ClosePatchWindow_PatchSystemEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }
    }

    public class PatchUserEventMessage
    {
        public class UserTryInitialize_PatchUserEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryInitialize_PatchUserEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class UserBeginDownloadWebFiles_PatchUserEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new UserBeginDownloadWebFiles_PatchUserEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class UserTryUpdatePackageVersion_PatchUserEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryUpdatePackageVersion_PatchUserEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class UserTryUpdatePatchManifest_PatchUserEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryUpdatePatchManifest_PatchUserEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }

        public class UserTryDownloadWebFiles_PatchUserEventMessage : IEventMessageBase
        {
            public static void SendEventMessage()
            {
                var msg = new UserTryDownloadWebFiles_PatchUserEventMessage();
                EventService.Instance.SendEventMessage(msg);
            }
        }
    }
}
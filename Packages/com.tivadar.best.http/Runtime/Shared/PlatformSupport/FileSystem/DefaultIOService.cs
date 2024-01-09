#define _ENABLE_LOGGING
using System;
using System.IO;

namespace Best.HTTP.Shared.PlatformSupport.FileSystem
{
    public sealed class DefaultIOService : IIOService
    {
        public Stream CreateFileStream(string path, FileStreamModes mode)
        {
#if ENABLE_LOGGING
            if (HTTPManager.Logger.IsDiagnostic)
                HTTPManager.Logger.Verbose(nameof(DefaultIOService), $"{nameof(CreateFileStream)}('{path}', {mode})");
#endif

            switch (mode)
            {
                case FileStreamModes.Create:
                    return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                case FileStreamModes.OpenRead:
                    return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read /*, 64 * 1024, FileOptions.SequentialScan*/);
                case FileStreamModes.OpenReadWrite:
                    return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                case FileStreamModes.Append:
                    return new FileStream(path, FileMode.Append);
            }

            throw new NotImplementedException($"{nameof(DefaultIOService)}.{nameof(CreateFileStream)} - '{mode}' not implemented!");
        }

        public void DirectoryCreate(string path)
        {
#if ENABLE_LOGGING
            if (HTTPManager.Logger.IsDiagnostic)
                HTTPManager.Logger.Verbose(nameof(DefaultIOService), $"{nameof(DirectoryCreate)}('{path}')");
#endif
            Directory.CreateDirectory(path);
        }

        public void DirectoryDelete(string path) => Directory.Delete(path, true);

        public bool DirectoryExists(string path)
        {
            bool exists = Directory.Exists(path);

#if ENABLE_LOGGING
            if (HTTPManager.Logger.IsDiagnostic)
                HTTPManager.Logger.Verbose(nameof(DefaultIOService), $"{nameof(DirectoryExists)}('{path}', {exists})");
#endif

            return exists;
        }

        public string[] GetFiles(string path)
        {
            var files = Directory.GetFiles(path);

#if ENABLE_LOGGING
            if (HTTPManager.Logger.IsDiagnostic)
                HTTPManager.Logger.Verbose(nameof(DefaultIOService), $"{nameof(GetFiles)}('{path}', {files?.Length})");
#endif

            return files;
        }

        public void FileDelete(string path)
        {
#if ENABLE_LOGGING
            if (HTTPManager.Logger.IsDiagnostic)
                HTTPManager.Logger.Verbose(nameof(DefaultIOService), $"{nameof(FileDelete)}('{path}')");
#endif
            File.Delete(path);
        }

        public bool FileExists(string path)
        {
            bool exists = File.Exists(path);

#if ENABLE_LOGGING
            if (HTTPManager.Logger.IsDiagnostic)
                HTTPManager.Logger.Verbose(nameof(DefaultIOService), $"{nameof(FileExists)}('{path}', {exists})");
#endif

            return exists;
        }
    }
}

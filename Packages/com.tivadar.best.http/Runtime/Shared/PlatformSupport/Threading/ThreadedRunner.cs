using System;
using System.Threading;

namespace Best.HTTP.Shared.PlatformSupport.Threading
{
    public static class ThreadedRunner
    {
        public static int ShortLivingThreads { get => _shortLivingThreads; }
        private static int _shortLivingThreads;

        public static int LongLivingThreads { get => _LongLivingThreads; }
        private static int _LongLivingThreads;

        public static void SetThreadName(string name)
        {
            try
            {
                System.Threading.Thread.CurrentThread.Name = name;
            }
            catch(Exception ex)
            {
                if (HTTPManager.Logger.IsDiagnostic)
                    HTTPManager.Logger.Exception(nameof(ThreadedRunner), nameof(SetThreadName), ex);
            }
        }

        public static void RunShortLiving<T>(Action<T> job, T param)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                using var __ = new IncDecShortLiving(true);
                job(param);
            }));
        }

        public static void RunShortLiving<T1, T2>(Action<T1, T2> job, T1 param1, T2 param2)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                using var __ = new IncDecShortLiving(true);
                job(param1, param2);
            }));
        }

        public static void RunShortLiving<T1, T2, T3>(Action<T1, T2, T3> job, T1 param1, T2 param2, T3 param3)
        {            
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                using var __ = new IncDecShortLiving(true);
                job(param1, param2, param3);
            }));
        }

        public static void RunShortLiving<T1, T2, T3, T4>(Action<T1, T2, T3, T4> job, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ =>
            {
                using var __ = new IncDecShortLiving(true);
                job(param1, param2, param3, param4);
            }));
        }

        public static void RunShortLiving(Action job)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((param) =>
            {
                using var __ = new IncDecShortLiving(true);
                job();
            }));
        }

        public static void RunLongLiving(Action job)
        {
            var thread = new Thread(new ParameterizedThreadStart((param) =>
            {
                using var __ = new IncDecLongLiving(true);
                job();
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        struct IncDecShortLiving : IDisposable
        {
            public IncDecShortLiving(bool dummy) => Interlocked.Increment(ref ThreadedRunner._shortLivingThreads);
            public void Dispose() => Interlocked.Decrement(ref ThreadedRunner._shortLivingThreads);
        }

        struct IncDecLongLiving : IDisposable
        {
            public IncDecLongLiving(bool dummy) => Interlocked.Increment(ref ThreadedRunner._LongLivingThreads);
            public void Dispose() => Interlocked.Decrement(ref ThreadedRunner._LongLivingThreads);
        }
    }
}

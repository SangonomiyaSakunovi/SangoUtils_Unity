using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class TaskAsyncTimer : TaskBaseTimer
{
    private bool setHandle;
    private readonly ConcurrentDictionary<int, AsyncTimerTask> taskDic;
    private ConcurrentQueue<AsyncTimerTaskPack> packQue;
    private const string tidLock = "TaskAsyncTimer_Lock";

    public TaskAsyncTimer(bool setHandle)
    {
        taskDic = new ConcurrentDictionary<int, AsyncTimerTask>();
        this.setHandle = setHandle;
        if (setHandle)
        {
            packQue = new ConcurrentQueue<AsyncTimerTaskPack>();
        }
    }

    public override int AddTask(
        uint delayInvokeTaskTime,
        Action<int> doneTaskCallBack,
        Action<int> cancelTaskCallBack,
        int repeatTaskCount = 1)
    {
        int tid = GenerateTaskId();
        AsyncTimerTask task = new AsyncTimerTask(tid, delayInvokeTaskTime, repeatTaskCount, doneTaskCallBack, cancelTaskCallBack);
        RunTaskInPool(task);

        if (taskDic.TryAdd(tid, task))
        {
            return tid;
        }
        else
        {
            LogWarnningFunc?.Invoke($"key:{tid} already exist.");
            return -1;
        }
    }

    public override bool RemoveTask(int taskId)
    {
        if (taskDic.TryRemove(taskId, out AsyncTimerTask task))
        {
            LogInfoFunc?.Invoke($"Remvoe tid:{task.TaskId} task in taskDic Succ.");

            task.CancellationTokenSource.Cancel();

            if (setHandle && task.CancelCallBack != null)
            {
                packQue.Enqueue(new AsyncTimerTaskPack(task.TaskId, task.CancelCallBack));
            }
            else
            {
                task.CancelCallBack?.Invoke(task.TaskId);
            }
            return true;
        }
        else
        {
            LogErrorFunc?.Invoke($"Remove tid:{task.TaskId} task in taskDic failed.");
            return false;
        }
    }

    public override void ResetTask()
    {
        if (packQue != null && !packQue.IsEmpty)
        {
            LogWarnningFunc?.Invoke("Call Queue is not Empty.");
        }
        taskDic.Clear();
        _taskId = 0;
    }

    public void HandleTask()
    {
        while (packQue != null && packQue.Count > 0)
        {
            if (packQue.TryDequeue(out AsyncTimerTaskPack pack))
            {
                pack.TaskCallBack?.Invoke(pack.TaskId);
            }
            else
            {
                LogWarnningFunc?.Invoke($"packQue dequeue data failed.");
            }
        }
    }

    private void RunTaskInPool(AsyncTimerTask task)
    {
        Task.Run(async () =>
        {
            if (task.RepeatCount > 0)
            {
                do
                {
                    //限次数循环任务
                    --task.RepeatCount;
                    ++task.LoopIndex;
                    int delay = (int)(task.DelayInvokeTime + task.FixedDeltaTime);
                    if (delay > 0)
                    {
                        await Task.Delay(delay, task.CancellationToken);
                    }
                    TimeSpan ts = DateTime.UtcNow - task.StartTime;
                    task.FixedDeltaTime = (int)(task.DelayInvokeTime * task.LoopIndex - ts.TotalMilliseconds);
                    CallBackTaskCB(task);
                } while (task.RepeatCount > 0);
            }
            else
            {
                //永久循环任务
                while (true)
                {
                    //限次数循环任务
                    ++task.LoopIndex;
                    int delay = (int)(task.DelayInvokeTime + task.FixedDeltaTime);
                    if (delay > 0)
                    {
                        await Task.Delay(delay, task.CancellationToken);
                    }
                    TimeSpan ts = DateTime.UtcNow - task.StartTime;
                    task.FixedDeltaTime = (int)(task.DelayInvokeTime * task.LoopIndex - ts.TotalMilliseconds);
                    CallBackTaskCB(task);
                }
            }
        });
    }

    private void CallBackTaskCB(AsyncTimerTask task)
    {
        if (setHandle)
        {
            packQue.Enqueue(new AsyncTimerTaskPack(task.TaskId, task.DoneCallBack));
        }
        else
        {
            task.DoneCallBack.Invoke(task.TaskId);
        }

        if (task.RepeatCount == 0)
        {
            if (taskDic.TryRemove(task.TaskId, out AsyncTimerTask temp))
            {
                LogInfoFunc?.Invoke($"Task tid:{task.TaskId} run to completion.");
            }
            else
            {
                LogErrorFunc?.Invoke($"Remove tid:{task.TaskId} task in taskDic failed.");
            }
        }
    }

    protected override int GenerateTaskId()
    {
        lock (tidLock)
        {
            while (true)
            {
                ++_taskId;
                if (_taskId == int.MaxValue)
                {
                    _taskId = 0;
                }
                if (!taskDic.ContainsKey(_taskId))
                {
                    return _taskId;
                }
            }
        }
    }

    private class AsyncTimerTask
    {
        public int TaskId { get; private set; }
        public uint DelayInvokeTime { get; set; }
        public int RepeatCount { get; set; }
        public Action<int> DoneCallBack { get; set; }
        public Action<int> CancelCallBack { get; set; }
        public DateTime StartTime { get; set; }
        public ulong LoopIndex { get; set; }
        public int FixedDeltaTime { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public AsyncTimerTask(int taskId, uint delayInvokeTime, int repeatCount, Action<int> doneCallBack, Action<int> cancelCallBack)
        {
            TaskId = taskId;
            DelayInvokeTime = delayInvokeTime;
            RepeatCount = repeatCount;
            DoneCallBack = doneCallBack;
            CancelCallBack = cancelCallBack;
            StartTime = DateTime.UtcNow;
            LoopIndex = 0;
            FixedDeltaTime = 0;
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }
    }

    private class AsyncTimerTaskPack
    {
        public int TaskId { get; private set; }
        public Action<int> TaskCallBack { get; set; }

        public AsyncTimerTaskPack(int taskId, Action<int> taskCallBack)
        {
            TaskId = taskId;
            TaskCallBack = taskCallBack;
        }
    }
}

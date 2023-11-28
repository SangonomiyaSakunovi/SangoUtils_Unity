using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class TaskAsyncTimer : TaskBaseTimer
{
    private bool _isSetHandled;
    private readonly ConcurrentDictionary<uint, AsyncTimerTask> _taskDict;
    private ConcurrentQueue<AsyncTimerTaskPack> _taskPackQueue;
    private const string _taskIdLock = "TaskAsyncTimer_Lock";

    public TaskAsyncTimer(bool isSetHandled)
    {
        _taskDict = new ConcurrentDictionary<uint, AsyncTimerTask>();
        _isSetHandled = isSetHandled;
        if (isSetHandled)
        {
            _taskPackQueue = new ConcurrentQueue<AsyncTimerTaskPack>();
        }
    }

    public override uint AddTask(uint delayedInvokeTaskTime, Action<uint> completeTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1)
    {
        uint taskId = GenerateTaskId();
        AsyncTimerTask task = new AsyncTimerTask(taskId, delayedInvokeTaskTime, repeatTaskCount, completeTaskCallBack, cancelTaskCallBack);
        RunTaskInPool(task);

        if (_taskDict.TryAdd(taskId, task))
        {
            return taskId;
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskAsyncTimer AddTask Warnning: [ {taskId} ] already Exist.");
            return 0;
        }
    }

    public override bool RemoveTask(uint taskId)
    {
        if (_taskDict.TryRemove(taskId, out AsyncTimerTask task))
        {
            LogInfoFunc?.Invoke($"TaskAsyncTimer RemoveTask Succeed: [ {taskId} ].");

            task.CancellationTokenSource.Cancel();

            if (_isSetHandled && task.CancelCallBack != null)
            {
                _taskPackQueue.Enqueue(new AsyncTimerTaskPack(taskId, task.CancelCallBack));
            }
            else
            {
                task.CancelCallBack?.Invoke(taskId);
            }
            return true;
        }
        else
        {
            LogErrorFunc?.Invoke($"TaskAsyncTimer RemoveTask Error: Try Remove [ {task.TaskId} ] in TaskDic Failed.");
            return false;
        }
    }

    public override void ResetTask()
    {
        if (_taskPackQueue != null && !_taskPackQueue.IsEmpty)
        {
            LogWarnningFunc?.Invoke("TaskAsyncTimer ResetTask Warnning: TaskCallBack Queue is Not Empty.");
        }
        _taskDict.Clear();
        _taskId = 0;
    }

    public void HandleTask()
    {
        while (_taskPackQueue != null && _taskPackQueue.Count > 0)
        {
            if (_taskPackQueue.TryDequeue(out AsyncTimerTaskPack pack))
            {
                pack.taskCallBack?.Invoke(pack.taskId);
            }
            else
            {
                LogWarnningFunc?.Invoke($"TaskAsyncTimer HandleTask Warnning: TaskPackQueue Dequeue Failed.");
            }
        }
    }

    private void RunTaskInPool(AsyncTimerTask task)
    {
        Task.Run(async () =>
        {
            if (task.RepeatCount > 0)    //We define that token 0 is repeated forever.
            {
                do
                {
                    --task.RepeatCount;
                    ++task.LoopIndex;
                    int delay = (int)(task.DelayInvokeTime + task.FixedDeltaTime);
                    if (delay > 0)
                    {
                        await Task.Delay(delay, task.CancellationToken);
                    }
                    TimeSpan ts = DateTime.UtcNow - task.StartTime;
                    task.FixedDeltaTime = (int)(task.DelayInvokeTime * task.LoopIndex - ts.TotalMilliseconds);
                    InvokeTaskCallBack(task);
                } while (task.RepeatCount > 0);
            }
            else
            {
                while (true)
                {
                    ++task.LoopIndex;
                    int delay = (int)(task.DelayInvokeTime + task.FixedDeltaTime);
                    if (delay > 0)
                    {
                        await Task.Delay(delay, task.CancellationToken);
                    }
                    TimeSpan ts = DateTime.UtcNow - task.StartTime;
                    task.FixedDeltaTime = (int)(task.DelayInvokeTime * task.LoopIndex - ts.TotalMilliseconds);
                    InvokeTaskCallBack(task);
                }
            }
        });
    }

    private void InvokeTaskCallBack(AsyncTimerTask task)
    {
        if (_isSetHandled)
        {
            _taskPackQueue.Enqueue(new AsyncTimerTaskPack(task.TaskId, task.CompleteCallBack));
        }
        else
        {
            task.CompleteCallBack.Invoke(task.TaskId);
        }

        if (task.RepeatCount == 0)
        {
            if (_taskDict.TryRemove(task.TaskId, out AsyncTimerTask temp))
            {
                LogInfoFunc?.Invoke($"TaskAsyncTimer UpdateTask Succeed: [ {task.TaskId} ] Run to Completion.");
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskAsyncTimer UpdateTask Error: Remove [ {task.TaskId} ] in TaskDic Failed.");
            }
        }
    }

    protected override uint GenerateTaskId()
    {
        lock (_taskIdLock)
        {
            while (true)
            {
                ++_taskId;
                if (_taskId == uint.MaxValue)
                {
                    _taskId = 1;
                }
                if (!_taskDict.ContainsKey(_taskId))
                {
                    return _taskId;
                }
            }
        }
    }

    private class AsyncTimerTask
    {
        public uint TaskId { get; private set; }
        public uint DelayInvokeTime { get; set; }
        public int RepeatCount { get; set; }
        public Action<uint> CompleteCallBack { get; set; }
        public Action<uint> CancelCallBack { get; set; }
        public DateTime StartTime { get; set; }
        public ulong LoopIndex { get; set; }
        public int FixedDeltaTime { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public AsyncTimerTask(uint taskId, uint delayInvokeTime, int repeatCount, Action<uint> completeCallBack, Action<uint> cancelCallBack)
        {
            TaskId = taskId;
            DelayInvokeTime = delayInvokeTime;
            RepeatCount = repeatCount;
            CompleteCallBack = completeCallBack;
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
        public uint taskId { get; private set; }
        public Action<uint> taskCallBack { get; set; }

        public AsyncTimerTaskPack(uint taskId, Action<uint> taskCallBack)
        {
            this.taskId = taskId;
            this.taskCallBack = taskCallBack;
        }
    }
}

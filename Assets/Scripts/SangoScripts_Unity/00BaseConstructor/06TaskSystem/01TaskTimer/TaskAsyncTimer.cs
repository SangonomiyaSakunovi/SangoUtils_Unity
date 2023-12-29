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

            task.cancellationTokenSource.Cancel();

            if (_isSetHandled && task.onCanceledCallBack != null)
            {
                _taskPackQueue.Enqueue(new AsyncTimerTaskPack(taskId, task.onCanceledCallBack));
            }
            else
            {
                task.onCanceledCallBack?.Invoke(taskId);
            }
            return true;
        }
        else
        {
            LogErrorFunc?.Invoke($"TaskAsyncTimer RemoveTask Error: Try Remove [ {task.taskId} ] in TaskDic Failed.");
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
            if (task.repeatCount > 0)    //We define that token 0 is repeated forever.
            {
                do
                {
                    --task.repeatCount;
                    ++task.loopIndex;
                    int delay = (int)(task.delayInvokeTime + task.fixedDeltaTime);
                    if (delay > 0)
                    {
                        await Task.Delay(delay, task.cancellationToken);
                    }
                    TimeSpan ts = DateTime.UtcNow - task.startTime;
                    task.fixedDeltaTime = (int)(task.delayInvokeTime * task.loopIndex - ts.TotalMilliseconds);
                    InvokeTaskCallBack(task);
                } while (task.repeatCount > 0);
            }
            else
            {
                while (true)
                {
                    ++task.loopIndex;
                    int delay = (int)(task.delayInvokeTime + task.fixedDeltaTime);
                    if (delay > 0)
                    {
                        await Task.Delay(delay, task.cancellationToken);
                    }
                    TimeSpan ts = DateTime.UtcNow - task.startTime;
                    task.fixedDeltaTime = (int)(task.delayInvokeTime * task.loopIndex - ts.TotalMilliseconds);
                    InvokeTaskCallBack(task);
                }
            }
        });
    }

    private void InvokeTaskCallBack(AsyncTimerTask task)
    {
        if (_isSetHandled)
        {
            _taskPackQueue.Enqueue(new AsyncTimerTaskPack(task.taskId, task.onCompletedCallBack));
        }
        else
        {
            task.onCompletedCallBack.Invoke(task.taskId);
        }

        if (task.repeatCount == 0)
        {
            if (_taskDict.TryRemove(task.taskId, out AsyncTimerTask temp))
            {
                LogInfoFunc?.Invoke($"TaskAsyncTimer UpdateTask Succeed: [ {task.taskId} ] Run to Completion.");
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskAsyncTimer UpdateTask Error: Remove [ {task.taskId} ] in TaskDic Failed.");
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
        public uint taskId;
        public uint delayInvokeTime;
        public int repeatCount;
        public Action<uint> onCompletedCallBack;
        public Action<uint> onCanceledCallBack;
        public DateTime startTime;
        public ulong loopIndex;
        public int fixedDeltaTime;
        public CancellationTokenSource cancellationTokenSource;
        public CancellationToken cancellationToken;

        public AsyncTimerTask(uint taskId, uint delayInvokeTime, int repeatCount, Action<uint> completeCallBack, Action<uint> cancelCallBack)
        {
            this.taskId = taskId;
            this.delayInvokeTime = delayInvokeTime;
            this.repeatCount = repeatCount;
            this.onCompletedCallBack = completeCallBack;
            this.onCanceledCallBack = cancelCallBack;
            startTime = DateTime.UtcNow;
            loopIndex = 0;
            fixedDeltaTime = 0;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }
    }

    private class AsyncTimerTaskPack
    {
        public uint taskId;
        public Action<uint> taskCallBack;

        public AsyncTimerTaskPack(uint taskId, Action<uint> taskCallBack)
        {
            this.taskId = taskId;
            this.taskCallBack = taskCallBack;
        }
    }
}

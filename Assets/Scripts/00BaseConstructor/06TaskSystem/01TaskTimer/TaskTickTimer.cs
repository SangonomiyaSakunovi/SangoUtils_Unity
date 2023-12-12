using System;
using System.Collections.Concurrent;
using System.Threading;

public class TaskTickTimer : TaskBaseTimer
{
    private readonly DateTime _utcInitialDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private readonly ConcurrentDictionary<uint, TickTimerTask> _taskDict;
    private readonly bool _isSetHandled;
    private readonly ConcurrentQueue<TickTimerTaskPack> _taskPackQueue;
    private const string _taskIdLock = "TaskTickTimer_Lock";
    private readonly Thread _taskTickTimerThread;

    public TaskTickTimer(int intervalTime = 0, bool isSetHandled = true)
    {
        _taskDict = new ConcurrentDictionary<uint, TickTimerTask>();
        _isSetHandled = isSetHandled;
        if (isSetHandled)
        {
            _taskPackQueue = new ConcurrentQueue<TickTimerTaskPack>();
        }
        if (intervalTime != 0)
        {
            void StartTickTimerTaskInThread()
            {
                try
                {
                    while (true)
                    {
                        UpdateTask();
                        Thread.Sleep(intervalTime);
                    }
                }
                catch (ThreadAbortException e)
                {
                    LogWarnningFunc?.Invoke($"TaskTickTimer Thread Warinning: Thread Abort Error, Reason: {e}.");
                }
            }
            _taskTickTimerThread = new Thread(new ThreadStart(StartTickTimerTaskInThread));
            _taskTickTimerThread.Start();
        }
    }

    public override uint AddTask(uint delayedInvokeTaskTime, Action<uint> completeTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1)
    {
        uint taskId = GenerateTaskId();
        double startTime = GetUTCMilliseconds();
        double targetTime = startTime + delayedInvokeTaskTime;
        TickTimerTask tickTimerTask = new TickTimerTask(taskId, delayedInvokeTaskTime, repeatTaskCount, targetTime, completeTaskCallBack, cancelTaskCallBack, startTime);
        if (_taskDict.TryAdd(taskId, tickTimerTask))
        {
            return taskId;
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskTickTimer AddTask Warnning: [ {taskId} ] already Exist.");
            return 0;
        }
    }

    public override bool RemoveTask(uint taskId)
    {
        if (_taskDict.TryRemove(taskId, out TickTimerTask task))
        {
            if (_isSetHandled && task.onCanceledCallBack != null)
            {
                LogInfoFunc?.Invoke($"TaskTickTimer RemoveTask Succeed: [ {taskId} ].");
                _taskPackQueue.Enqueue(new TickTimerTaskPack(taskId, task.onCanceledCallBack));
            }
            else
            {
                task.onCanceledCallBack?.Invoke(taskId);
            }
            return true;
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskTickTimer RemoveTask Warnning: Remove [ {taskId} ] Failed.");
            return false;
        }
    }

    public override void ResetTask()
    {
        if (!_taskPackQueue.IsEmpty)
        {
            LogWarnningFunc?.Invoke("TaskTickTimer ResetTask Warnning: TaskCallback Queue is Not Empty.");
        }
        _taskDict.Clear();
        if (_taskTickTimerThread != null)
        {
            _taskTickTimerThread.Abort();
        }
    }

    public void UpdateTask()
    {
        double nowTime = GetUTCMilliseconds();
        foreach (TickTimerTask task in _taskDict.Values)
        {
            if (nowTime < task.targetTime)
            {
                continue;
            }

            ++task.loopIndex;
            if (task.repeatCount > 0)
            {
                --task.repeatCount;
                if (task.repeatCount == 0)
                {
                    RunTask(task.taskId);
                }
                else
                {
                    task.targetTime = task.startTime + task.delayInvokeTime * (task.loopIndex + 1);
                    InvokeTaskCallBack(task.taskId, task.onCompletedCallBack);
                }
            }
            else
            {
                task.targetTime = task.startTime + task.delayInvokeTime * (task.loopIndex + 1);
                InvokeTaskCallBack(task.taskId, task.onCompletedCallBack);
            }
        }
    }
    public void HandleTask()
    {
        while (_taskPackQueue != null && _taskPackQueue.Count > 0)
        {
            if (_taskPackQueue.TryDequeue(out TickTimerTaskPack pack))
            {
                pack.taskCallBack.Invoke(pack.taskId);
            }
            else
            {
                LogErrorFunc?.Invoke("TaskTickTimer Handle Error: TickTaskPack Queue Dequeue Failed.");
            }
        }
    }

    private void RunTask(uint taskId)
    {
        if (_taskDict.TryRemove(taskId, out TickTimerTask task))
        {
            InvokeTaskCallBack(taskId, task.onCompletedCallBack);
            task.onCompletedCallBack = null;
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskTickTimer Done Warnning: Remove [ {taskId} ] in TaskDict Failed.");
        }
    }

    private void InvokeTaskCallBack(uint taskId, Action<uint> taskCallBack)
    {
        if (_isSetHandled)
        {
            _taskPackQueue.Enqueue(new TickTimerTaskPack(taskId, taskCallBack));
        }
        else
        {
            taskCallBack.Invoke(taskId);
        }
    }

    private double GetUTCMilliseconds()
    {
        TimeSpan timeSpan = DateTime.UtcNow - _utcInitialDateTime;
        return timeSpan.TotalMilliseconds;
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

    private class TickTimerTask
    {
        public uint taskId;
        public uint delayInvokeTime;
        public int repeatCount;
        public double targetTime;
        public Action<uint> onCompletedCallBack;
        public Action<uint> onCanceledCallBack;
        public double startTime;
        public ulong loopIndex;

        public TickTimerTask(uint taskId, uint delayInvokeTime, int repeatCount, double targetTime, Action<uint> completeCallBack, Action<uint> cancelCallBack, double startTime)
        {
            this.taskId = taskId;
            this.delayInvokeTime = delayInvokeTime;
            this.repeatCount = repeatCount;
            this.targetTime = targetTime;
            this.onCompletedCallBack = completeCallBack;
            this.onCanceledCallBack = cancelCallBack;
            this.startTime = startTime;
            loopIndex = 0;
        }
    }

    private class TickTimerTaskPack
    {
        public uint taskId;
        public Action<uint> taskCallBack;
        public TickTimerTaskPack(uint taskId, Action<uint> taskCallBack)
        {
            this.taskId = taskId;
            this.taskCallBack = taskCallBack;
        }
    }
}

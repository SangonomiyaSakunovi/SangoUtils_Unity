using System;
using System.Collections.Concurrent;
using System.Threading;

public class TaskTickTimer : TaskBaseTimer
{
    private readonly DateTime _utcInitialDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private readonly ConcurrentDictionary<int, TickTimerTask> _taskDict;
    private readonly bool _isSetHandled;
    private readonly ConcurrentQueue<TickTimerTaskPack> _taskPackQueue;
    private const string _taskIdLock = "TaskTickTimer_Lock";
    private readonly Thread _taskTickTimerThread;

    public TaskTickTimer(int intervalTime = 0, bool isSetHandled = true)
    {
        _taskDict = new ConcurrentDictionary<int, TickTimerTask>();
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

    public override int AddTask(uint delayInvokeTaskTime, Action<int> doneTaskCallBack, Action<int> cancelTaskCallBack, int repeatTaskCount = 1)
    {
        int taskId = GenerateTaskId();
        double startTime = GetUTCMilliseconds();
        double targetTime = startTime + delayInvokeTaskTime;
        TickTimerTask tickTimerTask = new TickTimerTask(taskId, delayInvokeTaskTime, repeatTaskCount, targetTime, doneTaskCallBack, cancelTaskCallBack, startTime);
        if (_taskDict.TryAdd(taskId, tickTimerTask))
        {
            return taskId;
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskTickTimer AddTask Warnning: [ {taskId} ] already Exist.");
            return -1;
        }
    }

    public override bool RemoveTask(int taskId)
    {
        if (_taskDict.TryRemove(taskId, out TickTimerTask task))
        {
            if (_isSetHandled && task.CancelCallBack != null)
            {
                _taskPackQueue.Enqueue(new TickTimerTaskPack(taskId, task.CancelCallBack));
            }
            else
            {
                task.CancelCallBack?.Invoke(taskId);
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
            if (nowTime < task.TargetTime)
            {
                continue;
            }

            ++task.LoopIndex;
            if (task.RepeatCount > 0)
            {
                --task.RepeatCount;
                if (task.RepeatCount == 0)
                {
                    DoneTask(task.TaskId);
                }
                else
                {
                    task.TargetTime = task.StartTime + task.DelayInvokeTime * (task.LoopIndex + 1);
                    InvokeTaskCallBack(task.TaskId, task.DoneCallBack);
                }
            }
            else
            {
                task.TargetTime = task.StartTime + task.DelayInvokeTime * (task.LoopIndex + 1);
                InvokeTaskCallBack(task.TaskId, task.DoneCallBack);
            }
        }
    }
    public void HandleTask()
    {
        while (_taskPackQueue != null && _taskPackQueue.Count > 0)
        {
            if (_taskPackQueue.TryDequeue(out TickTimerTaskPack pack))
            {
                pack.TaskCallBack.Invoke(pack.TaskId);
            }
            else
            {
                LogErrorFunc?.Invoke("TaskTickTimer Handle Warnning: TickTaskPack Queue Dequeue Data Error.");
            }
        }
    }

    private void DoneTask(int taskId)
    {
        if (_taskDict.TryRemove(taskId, out TickTimerTask task))
        {
            InvokeTaskCallBack(taskId, task.DoneCallBack);
            task.DoneCallBack = null;
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskTickTimer Done Warnning: Remove [ {taskId} ] in TaskDict Failed.");
        }
    }

    private void InvokeTaskCallBack(int taskId, Action<int> taskCallBack)
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

    protected override int GenerateTaskId()
    {
        lock (_taskIdLock)
        {
            while (true)
            {
                ++_taskId;
                if (_taskId == int.MaxValue)
                {
                    _taskId = 0;
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
        public int TaskId { get; private set; }
        public uint DelayInvokeTime { get; set; }
        public int RepeatCount { get; set; }
        public double TargetTime { get; set; }
        public Action<int> DoneCallBack { get; set; }
        public Action<int> CancelCallBack { get; set; }
        public double StartTime { get; set; }
        public ulong LoopIndex { get; set; }

        public TickTimerTask(int taskId, uint delayInvokeTime, int repeatCount, double targetTime, Action<int> doneCallBack, Action<int> cancelCallBack, double startTime)
        {
            TaskId = taskId;
            DelayInvokeTime = delayInvokeTime;
            RepeatCount = repeatCount;
            TargetTime = targetTime;
            DoneCallBack = doneCallBack;
            CancelCallBack = cancelCallBack;
            StartTime = startTime;
            LoopIndex = 0;
        }
    }

    private class TickTimerTaskPack
    {
        public int TaskId { get; private set; }
        public Action<int> TaskCallBack { get; set; }
        public TickTimerTaskPack(int taskId, Action<int> taskCallBack)
        {
            TaskId = taskId;
            TaskCallBack = taskCallBack;
        }
    }
}

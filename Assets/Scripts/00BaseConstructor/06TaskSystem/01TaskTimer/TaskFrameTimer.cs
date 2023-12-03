using System;
using System.Collections.Generic;

public class TaskFrameTimer : TaskBaseTimer
{
    private ulong _currentFrame;
    private readonly Dictionary<uint, FrameTimerTask> _taskDict;
    private const string _taskIdLock = "TaskFrameTimer_Lock";
    private List<uint> _taskIdLts;

    public TaskFrameTimer(ulong frameId = 0)
    {
        _currentFrame = frameId;
        _taskDict = new Dictionary<uint, FrameTimerTask>();
        _taskIdLts = new List<uint>();
    }

    public override uint AddTask(uint delayedInvokeTaskTime, Action<uint> completeTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1)
    {
        uint taskId = GenerateTaskId();
        ulong destFrame = _currentFrame + delayedInvokeTaskTime;
        FrameTimerTask task = new FrameTimerTask(taskId, delayedInvokeTaskTime, repeatTaskCount, destFrame, completeTaskCallBack, cancelTaskCallBack);
        if (_taskDict.ContainsKey(taskId))
        {
            LogWarnningFunc?.Invoke($"TaskFrameTimer AddTask Warnning: [ {taskId} ] already Exist.");
            return 0;
        }
        else
        {
            _taskDict.Add(taskId, task);
            return taskId;
        }
    }

    public override bool RemoveTask(uint taskId)
    {
        if (_taskDict.TryGetValue(taskId, out FrameTimerTask task))
        {
            if (_taskDict.Remove(taskId))
            {
                LogInfoFunc?.Invoke($"TaskFrameTimer RemoveTask Succeed: [ {taskId} ].");
                task.CancelCallBack?.Invoke(taskId);
                return true;
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskFrameTimer RemoveTask Error: Try Remove [ {taskId} ] in TaskDic Failed.");
                return false;
            }
        }
        else
        {
            LogWarnningFunc?.Invoke($"TaskFrameTimer RemoveTask Warnning: [ {taskId} ] is Not Exist.");
            return false;
        }
    }

    public override void ResetTask()
    {
        _taskDict.Clear();
        _taskIdLts.Clear();
        _currentFrame = 0;
    }

    public void UpdateTask()
    {
        ++_currentFrame;
        _taskIdLts.Clear();

        foreach (FrameTimerTask task in _taskDict.Values)
        {
            if (task.TargetFrame <= _currentFrame)
            {
                task.CompleteCallBack.Invoke(task.TaskId);
                task.TargetFrame += task.DelayInvokeTime;
                --task.RepeatCount;
                if (task.RepeatCount == 0)
                {
                    _taskIdLts.Add(task.TaskId);
                }
            }
        }

        for (int i = 0; i < _taskIdLts.Count; i++)
        {
            if (_taskDict.Remove(_taskIdLts[i]))
            {
                LogInfoFunc?.Invoke($"TaskFrameTimer UpdateTask Succeed: [ {_taskIdLts[i]} ] Run to Completion.");
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskFrameTimer UpdateTask Error: Remove [ {_taskIdLts[i]} ] in TaskDic Failed.");
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

    private class FrameTimerTask
    {
        public uint TaskId { get; private set; }
        public uint DelayInvokeTime { get; set; }
        public int RepeatCount { get; set; }
        public ulong TargetFrame { get; set; }
        public Action<uint> CompleteCallBack { get; set; }
        public Action<uint> CancelCallBack { get; set; }

        public FrameTimerTask(uint taskId, uint delayInvokeTime, int repeatCount, ulong targetFrame, Action<uint> completeCallBack, Action<uint> cancelCallBack)
        {
            TaskId = taskId;
            DelayInvokeTime = delayInvokeTime;
            RepeatCount = repeatCount;
            TargetFrame = targetFrame;
            CompleteCallBack = completeCallBack;
            CancelCallBack = cancelCallBack;
        }
    }
}

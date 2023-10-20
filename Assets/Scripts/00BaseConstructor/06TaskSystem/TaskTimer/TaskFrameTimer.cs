using System;
using System.Collections.Generic;

public class TaskFrameTimer : TaskBaseTimer
{
    private ulong currentFrame;
    private readonly Dictionary<int, FrameTimerTask> taskDic;
    private const string tidLock = "TaskFrameTimer_Lock";
    private List<int> tidLst;

    public TaskFrameTimer(ulong frameID = 0)
    {
        currentFrame = frameID;
        taskDic = new Dictionary<int, FrameTimerTask>();
        tidLst = new List<int>();
    }

    public override int AddTask(
        uint delayInvokeTaskTime,
        Action<int> doneTaskCallBack,
        Action<int> cancelTaskCallBack,
        int repeatTaskCount = 1)
    {
        int tid = GenerateTaskId();
        ulong destFrame = currentFrame + delayInvokeTaskTime;
        FrameTimerTask task = new FrameTimerTask(tid, delayInvokeTaskTime, repeatTaskCount, destFrame, doneTaskCallBack, cancelTaskCallBack);
        if (taskDic.ContainsKey(tid))
        {
            LogWarnningFunc?.Invoke($"key:{tid} already exist.");
            return -1;
        }
        else
        {
            taskDic.Add(tid, task);
            return tid;
        }
    }

    public override bool RemoveTask(int taskId)
    {
        if (taskDic.TryGetValue(taskId, out FrameTimerTask task))
        {
            if (taskDic.Remove(taskId))
            {
                task.CancelCallBack?.Invoke(taskId);
                return true;
            }
            else
            {
                LogErrorFunc?.Invoke($"Remove tid:{taskId} in taskDic failed.");
                return false;
            }
        }
        else
        {
            LogWarnningFunc?.Invoke($"tid:{taskId} is not exist.");
            return false;
        }
    }

    public override void ResetTask()
    {
        taskDic.Clear();
        tidLst.Clear();
        currentFrame = 0;
    }

    public void UpdateTask()
    {
        ++currentFrame;
        tidLst.Clear();

        foreach (var item in taskDic)
        {
            FrameTimerTask task = item.Value;
            if (task.TargetFrame <= currentFrame)
            {
                task.DoneCallBack.Invoke(task.TaskId);
                task.TargetFrame += task.DelayInvokeTime;
                --task.RepeatCount;
                if (task.RepeatCount == 0)
                {
                    tidLst.Add(task.TaskId);
                }
            }
        }

        for (int i = 0; i < tidLst.Count; i++)
        {
            if (taskDic.Remove(tidLst[i]))
            {
                LogInfoFunc?.Invoke($"Task tid:{tidLst[i]} run to completion.");
            }
            else
            {
                LogErrorFunc?.Invoke($"Remove tid:{tidLst[i]} task in taskDic failed.");
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

    private class FrameTimerTask
    {
        public int TaskId { get; private set; }
        public uint DelayInvokeTime { get; set; }
        public int RepeatCount { get; set; }
        public ulong TargetFrame { get; set; }
        public Action<int> DoneCallBack { get; set; }
        public Action<int> CancelCallBack { get; set; }

        public FrameTimerTask(int taskId, uint delayInvokeTime, int repeatCount, ulong targetFrame, Action<int> doneCallBack, Action<int> cancelCallBack)
        {
            this.TaskId = taskId;
            this.DelayInvokeTime = delayInvokeTime;
            this.RepeatCount = repeatCount;
            this.TargetFrame = targetFrame;
            this.DoneCallBack = doneCallBack;
            this.CancelCallBack = cancelCallBack;
        }
    }
}

using System;

public abstract class TaskBaseTimer
{
    public Action<string> LogInfoFunc { get; set; }
    public Action<string> LogWarnningFunc { get; set; }
    public Action<string> LogErrorFunc { get; set; }

    protected uint _taskId = 1;

    protected abstract uint GenerateTaskId();

    public abstract uint AddTask(uint delayedInvokeTaskTime, Action<uint> completeTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1);

    public abstract bool RemoveTask(uint taskId);

    public abstract void ResetTask();
}

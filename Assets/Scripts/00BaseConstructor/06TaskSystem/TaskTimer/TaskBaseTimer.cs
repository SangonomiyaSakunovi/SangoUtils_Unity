using System;

public abstract class TaskBaseTimer
{
    public Action<string> LogInfoFunc;
    public Action<string> LogWarnningFunc;
    public Action<string> LogErrorFunc; 

    protected uint _taskId = 1;

    protected abstract uint GenerateTaskId();

    public abstract uint AddTask(uint delayedInvokeTaskTime, Action<uint> completeTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1);

    public abstract bool RemoveTask(uint taskId);

    public abstract void ResetTask();
}

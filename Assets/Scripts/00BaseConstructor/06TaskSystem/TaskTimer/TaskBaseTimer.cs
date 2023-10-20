using System;

public abstract class TaskBaseTimer
{
    public Action<string> LogInfoFunc;
    public Action<string> LogWarnningFunc;
    public Action<string> LogErrorFunc; 

    protected int _taskId = 0;

    protected abstract int GenerateTaskId();

    public abstract int AddTask(uint delayInvokeTaskTime, Action<int> doneTaskCallBack, Action<int> cancelTaskCallBack, int repeatTaskCount = 1);

    public abstract bool RemoveTask(int taskId);

    public abstract void ResetTask();
}

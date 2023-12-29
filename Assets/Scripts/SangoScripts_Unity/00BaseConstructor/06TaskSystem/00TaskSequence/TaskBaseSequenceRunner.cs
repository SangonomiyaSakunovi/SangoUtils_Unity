using System;

public abstract class TaskBaseSequenceRunner
{
    public Action<string> LogInfoFunc;
    public Action<string> LogWarnningFunc;
    public Action<string> LogErrorFunc;
}

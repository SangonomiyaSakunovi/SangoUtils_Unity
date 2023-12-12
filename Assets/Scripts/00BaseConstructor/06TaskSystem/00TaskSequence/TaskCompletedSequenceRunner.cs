using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

public class TaskCompletedSequenceRunner : TaskBaseSequenceRunner
{
    private readonly ConcurrentDictionary<uint, CompletedSequenceTask> _taskDict;

    private uint _taskId = 1;
    private const string _taskIdLock = "TaskCompleteSequence_Lock";


    public TaskCompletedSequenceRunner()
    {
        _taskDict = new ConcurrentDictionary<uint, CompletedSequenceTask>();
    }

    public uint AddTask(List<uint> prerequisitedTasks, Action<uint> doneTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1)
    {
        uint taskId = GenerateTaskId();
        //CompleteSequenceTask task = new CompleteSequenceTask();
        return 0;

    }

    public bool RemoveTask(uint taskId)
    {
        throw new NotImplementedException();
    }

    public void ResetTask()
    {
        throw new NotImplementedException();
    }

    protected uint GenerateTaskId()
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

    private class CompletedSequenceTask
    {
        public uint taskId;
        public List<uint> prerequisitedTasks;
        public Action<uint> completeCallBack;
        public Action<uint> cancelCallBack;

        public CancellationTokenSource cancellationTokenSource;
        public CancellationToken cancellationToken;

        public CompletedSequenceTask(uint taskId, List<uint> prerequisitedTasks)
        {
            this.taskId = taskId;
            this.prerequisitedTasks = prerequisitedTasks;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }
    }

    private class CompletedSequenceTaskPack
    {

    }
}
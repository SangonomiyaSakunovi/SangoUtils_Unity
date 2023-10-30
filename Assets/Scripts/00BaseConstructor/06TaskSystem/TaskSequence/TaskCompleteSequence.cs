using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class TaskCompleteSequence : TaskBaseSequence
{
    private readonly ConcurrentDictionary<uint, CompleteSequenceTask> _taskDict;

    private const string _taskIdLock = "TaskCompleteSequence_Lock";


    public TaskCompleteSequence()
    {
        _taskDict = new ConcurrentDictionary<uint, CompleteSequenceTask>();
    }

    public override uint AddTask(List<uint> prerequisitedTasks, Action<uint> doneTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1)
    {
        uint taskId = GenerateTaskId();
        //CompleteSequenceTask task = new CompleteSequenceTask();
        return 0;
    }

    public override bool RemoveTask(uint taskId)
    {
        throw new NotImplementedException();
    }

    public override void ResetTask()
    {
        throw new NotImplementedException();
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

    private class CompleteSequenceTask
    {
        public uint TaskId { get; private set; }
        public List<uint> PrerequisitedTasks { get; set; }
        public Action<uint> CompleteCallBack { get; set; }
        public Action<uint> CancelCallBack { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public CompleteSequenceTask(uint taskId, List<uint> prerequisitedTasks)
        {
            TaskId = taskId;
            PrerequisitedTasks = prerequisitedTasks;
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }
    }

    private class CompleteSequenceTaskPack
    {

    }
}
using System;
using System.Collections.Generic;

public class TaskLinkedSequenceRunner : TaskBaseSequenceRunner
{
    private Dictionary<string, object> _blackboard;

    private LinkedList<TaskLinkedSequencePackBase> _taskLinkedList;
    private LinkedListNode<TaskLinkedSequencePackBase> _currentNode;

    public TaskLinkedSequenceRunner(object owner)
    {
        Owner = owner;
        _blackboard = new Dictionary<string, object>();
        _taskLinkedList = new LinkedList<TaskLinkedSequencePackBase>();
        _currentNode = _taskLinkedList.First;
    }

    public object Owner { get; }

    public int CurrentTaskId
    {
        get
        {
            if (_currentNode != null)
            {
                return _currentNode.Value.GetType().GetHashCode();
            }
            return -1;
        }
    }

    public void AddTask<T>() where T : TaskLinkedSequencePackBase
    {
        Type taskType = typeof(T);
        TaskLinkedSequencePackBase instance = Activator.CreateInstance(taskType) as TaskLinkedSequencePackBase;
        AddTask(instance);
    }

    public void AddTask(TaskLinkedSequencePackBase linkedSequenceTask)
    {
        if (linkedSequenceTask == null) throw new ArgumentNullException();
        _taskLinkedList.AddLast(linkedSequenceTask);
        linkedSequenceTask.OnInit(this);
    }

    public void RunFirstTask()
    {
        _currentNode = _taskLinkedList.First;
        _currentNode.Value.OnEnter();
    }

    public void RunNextTask()
    {
        _currentNode.Value.OnExit();
        if (_currentNode.Next != null)
        {
            _currentNode = _currentNode.Next;
            _currentNode.Value.OnEnter();
        }
    }

    public void RunTargetTask<T>() where T : TaskLinkedSequencePackBase
    {
        LinkedListNode<TaskLinkedSequencePackBase> targetNode = FindTargetTaskNode<T>();
        if (targetNode != null)
        {
            _currentNode.Value.OnExit();
            _currentNode = targetNode;
            _currentNode.Value.OnEnter();
        }
    }

    public void UpdateCurrentTask()
    {
        if (_currentNode != null)
        {
            _currentNode.Value.OnUpdate();
        }
    }

    public void UpdateTargetTask<T>() where T : TaskLinkedSequencePackBase
    {
        LinkedListNode<TaskLinkedSequencePackBase> targetNode = FindTargetTaskNode<T>();
        if (targetNode != null)
        {
            targetNode.Value.OnUpdate();
        }
    }

    public void UpdateAllTask()
    {
        LinkedListNode<TaskLinkedSequencePackBase> tempNode = _taskLinkedList.First;
        while (tempNode != null)
        {
            tempNode.Value.OnUpdate();
            tempNode = tempNode.Next;
        }
    }

    public void RemoveTargetTask<T>() where T : TaskLinkedSequencePackBase
    {
        LinkedListNode<TaskLinkedSequencePackBase> targetNode = FindTargetTaskNode<T>();
        if (targetNode != null)
        {
            _taskLinkedList.Remove(targetNode);
        }
    }

    private LinkedListNode<TaskLinkedSequencePackBase> FindTargetTaskNode<T>() where T : TaskLinkedSequencePackBase
    {
        Type taskType = typeof(T);
        int taskId = taskType.GetHashCode();
        LinkedListNode<TaskLinkedSequencePackBase> targetNode = _taskLinkedList.First;
        while (targetNode != null)
        {
            Type existTaskType = targetNode.Value.GetType();
            int existTaskId = existTaskType.GetHashCode();
            if (existTaskId == taskId)
            {
                break;
            }
            else
            {
                targetNode = targetNode.Next;
            }
        }
        return targetNode;
    }

    public void SetBlackboardValue(string key, object value)
    {
        if (_blackboard.ContainsKey(key))
        {
            _blackboard[key] = value;
        }
        else
        {
            _blackboard.Add(key, value);
        }
    }

    public object GetBlackboardValue(string key)
    {
        if (_blackboard.TryGetValue(key, out object value))
        {
            return value;
        }
        return null;
    }
}

public class TaskLinkedSequencePackBase
{
    protected TaskLinkedSequenceRunner _taskLinkedSequenceRunner;

    public virtual void OnInit(TaskLinkedSequenceRunner taskLinkedSequenceRunner)
    {
        _taskLinkedSequenceRunner = taskLinkedSequenceRunner;
    }
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}

using System;
using System.Collections.Generic;

public class FSMLinkedStater : FSMStaterBase
{
    private LinkedList<FSMLinkedStaterItemBase> _staterItemLinkedList;
    private LinkedListNode<FSMLinkedStaterItemBase> _currentNode;

    public FSMLinkedStater(object owner)
    {
        Owner = owner;
        _blackboard = new Dictionary<string, object>();
        _staterItemLinkedList = new LinkedList<FSMLinkedStaterItemBase>();
        _currentNode = _staterItemLinkedList.First;
    }

    public object Owner { get; }

    public string CurrentStaterName
    {
        get
        {
            if (_currentNode != null)
            {
                return _currentNode.Value.GetType().FullName;
            }
            return string.Empty;
        }
    }

    public void AddStaterItem<T>() where T : FSMLinkedStaterItemBase
    {
        Type staterItemType = typeof(T);
        FSMLinkedStaterItemBase instance = Activator.CreateInstance(staterItemType) as FSMLinkedStaterItemBase;
        AddStaterItem(instance);
    }

    public void AddStaterItem(FSMLinkedStaterItemBase staterItem)
    {
        if (staterItem == null) throw new ArgumentNullException();
        _staterItemLinkedList.AddLast(staterItem);
        staterItem.OnInit(this);
    }

    public void InvokeFirstStaterItem()
    {
        _currentNode = _staterItemLinkedList.First;
        _currentNode.Value.OnEnter();
    }

    public void InvokeNextStaterItem()
    {
        _currentNode.Value.OnExit();
        if (_currentNode.Next != null)
        {
            _currentNode = _currentNode.Next;
            _currentNode.Value.OnEnter();
        }
    }

    public void InvokeTargetStaterItem<T>(bool isFirstInvoke = false) where T : FSMLinkedStaterItemBase
    {
        LinkedListNode<FSMLinkedStaterItemBase> targetNode = FindTargetStaterItemNode<T>();
        if (targetNode != null)
        {
            if (!isFirstInvoke)
            {
                _currentNode.Value.OnExit();
            }           
            _currentNode = targetNode;
            _currentNode.Value.OnEnter();
        }
    }

    public void UpdateCurrentStaterItem()
    {
        if (_currentNode != null)
        {
            _currentNode.Value.OnUpdate();
        }
    }

    public void UpdateTargetStaterItem<T>() where T : FSMLinkedStaterItemBase
    {
        LinkedListNode<FSMLinkedStaterItemBase> targetNode = FindTargetStaterItemNode<T>();
        if (targetNode != null)
        {
            targetNode.Value.OnUpdate();
        }
    }

    public void UpdateAllStaterItem()
    {
        LinkedListNode<FSMLinkedStaterItemBase> tempNode = _staterItemLinkedList.First;
        while (tempNode != null)
        {
            tempNode.Value.OnUpdate();
            tempNode = tempNode.Next;
        }
    }

    public void RemoveStaterItem<T>() where T : FSMLinkedStaterItemBase
    {
        LinkedListNode<FSMLinkedStaterItemBase> targetNode = FindTargetStaterItemNode<T>();
        if (targetNode != null)
        {
            _staterItemLinkedList.Remove(targetNode);
        }
    }

    private LinkedListNode<FSMLinkedStaterItemBase> FindTargetStaterItemNode<T>() where T : FSMLinkedStaterItemBase
    {
        Type staterItemType = typeof(T);
        int staterItemId = staterItemType.GetHashCode();
        LinkedListNode<FSMLinkedStaterItemBase> targetNode = _staterItemLinkedList.First;
        while (targetNode != null)
        {
            Type existStaterItemType = targetNode.Value.GetType();
            int existStaterItemId = existStaterItemType.GetHashCode();
            if (existStaterItemId == staterItemId)
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
}

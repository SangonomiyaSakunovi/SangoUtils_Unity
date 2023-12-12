using System;
using System.Collections.Generic;

public class FSMStater<T> where T : struct
{
    private Dictionary<string, object> _blackboard;

    private Action<T, T> _transCallBack;
    public T _currentState { get; private set; }

    private Dictionary<T, List<FSMStaterItem<T>>> _transToOtherStateDict;
    private List<FSMStaterItem<T>> _transToOneStateList;

    private bool _isProcessingTransition = false;
    private Queue<FSMTransCommandBase> _transCommandQueue;

    public FSMStater(object owner, Action<T, T> transCallBack = null)
    {
        Owner = owner;
        _blackboard = new Dictionary<string, object>();
        _transToOtherStateDict = new Dictionary<T, List<FSMStaterItem<T>>>();
        _transToOneStateList = new List<FSMStaterItem<T>>();
        _transCommandQueue = new Queue<FSMTransCommandBase>();
        _transCallBack = transCallBack;
    }

    public object Owner { get; }

    public void AddLocalTransition(T currentState, FSMTransCommandBase command, T targetState, Func<T, FSMTransCommandBase, T, bool> callBack)
    {
        FSMStaterItem<T> item = new FSMStaterItem<T>(command, targetState, callBack);
        _transToOtherStateDict.TryGetValue(currentState, out List<FSMStaterItem<T>> itemList);
        if (itemList == null)
        {
            itemList = new List<FSMStaterItem<T>>();
            _transToOtherStateDict.Add(currentState, itemList);
        }
        itemList.Add(item);
    }

    public void AddGlobalTransition(FSMTransCommandBase command, T targetState, Func<T, FSMTransCommandBase, T, bool> callBack)
    {
        FSMStaterItem<T> item = new FSMStaterItem<T>(command, targetState, callBack);
        _transToOneStateList.Add(item);
    }

    public void InvokeInitState(T initialState)
    {
        _currentState = initialState;
    }

    public void Invoke(FSMTransCommandBase command)
    {
        if (_isProcessingTransition)
        {
            _transCommandQueue.Enqueue(command);
            return;
        }
        _isProcessingTransition = true;

        bool result = false;

        _transToOtherStateDict.TryGetValue(_currentState, out List<FSMStaterItem<T>> itemList);
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                result = ProcessTransition(itemList[i], command);
                if (result)
                {
                    break;
                }
            }
        }

        if (!result)
        {
            for (int i = 0; i < _transToOneStateList.Count; i++)
            {
                result = ProcessTransition(_transToOneStateList[i], command);
                if (result)
                {
                    break;
                }
            }
        }

        _isProcessingTransition = false;

        if (_transCommandQueue.Count > 0)
        {
            FSMTransCommandBase item = _transCommandQueue.Dequeue();
            Invoke(item);
        }
    }

    private bool ProcessTransition(FSMStaterItem<T> item, FSMTransCommandBase command)
    {
        bool result = false;
        if (item.TransCommand.Equals(command))
        {
            if (item.TransCallBack != null)
            {
                result = item.TransCallBack(_currentState, command, item.TargetState);
            }
            else
            {
                result = true;
            }

            if (result)
            {
                T previousState = _currentState;
                _currentState = item.TargetState;
                _transCallBack?.Invoke(previousState, _currentState);
            }
        }
        return result;
    }

    public void ClearTransCommandList()
    {
        _transCommandQueue.Clear();
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

using System;
using System.Collections.Generic;

public class EventMessageMap<T>
{
    private Dictionary<T, List<Action<object[]>>> _eventMessageHandlerDict = new Dictionary<T, List<Action<object[]>>>();
    private Dictionary<object, List<T>> _eventTargetDict = new Dictionary<object, List<T>>();

    public void AddEventMessageHandler(T eventId, Action<object[]> actionCallBack)
    {
        _eventMessageHandlerDict.TryGetValue(eventId, out List<Action<object[]>> existedActionList);
        if (existedActionList == null)
        {
            existedActionList = new List<Action<object[]>>();
            _eventMessageHandlerDict.Add(eventId, existedActionList);
        }
        if (existedActionList.Find(existedAction => existedAction.Equals(actionCallBack)) != null)
        {
            return;
        }
        else
        {
            existedActionList.Add(actionCallBack);
        }
        if (actionCallBack != null && actionCallBack.Target != null)
        {
            _eventTargetDict.TryGetValue(actionCallBack.Target, out List<T> existedTargetList);
            if (existedTargetList == null)
            {
                existedTargetList = new List<T>();
            }
            existedTargetList.Add(eventId);
        }
    }

    public void RemoveEventMessageHandler(T id)
    {
        if (_eventMessageHandlerDict.ContainsKey(id))
        {
            var handlerLst = _eventMessageHandlerDict[id];
            foreach (Action<object[]> cb in handlerLst)
            {
                if (cb != null
                    && cb.Target != null
                    && _eventTargetDict.ContainsKey(cb.Target))
                {
                    var idLst = _eventTargetDict[cb.Target];
                    idLst.RemoveAll((T t) =>
                    {
                        return id.Equals(t);
                    });
                    if (idLst.Count == 0)
                    {
                        _eventTargetDict.Remove(cb.Target);
                    }
                }
            }
        }
        _eventMessageHandlerDict.Remove(id);
    }
    public void RemoveTargetHandler(object target)
    {
        if (_eventTargetDict.ContainsKey(target))
        {
            List<T> evtLst = _eventTargetDict[target];
            for (int i = evtLst.Count - 1; i >= 0; --i)
            {
                T evt = evtLst[i];
                if (_eventMessageHandlerDict.ContainsKey(evt))
                {
                    List<Action<object[]>> cbLst = _eventMessageHandlerDict[evt];
                    cbLst.RemoveAll((Action<object[]> cb) =>
                    {
                        return cb.Target == target;
                    });
                    if (cbLst.Count == 0)
                    {
                        _eventMessageHandlerDict.Remove(evt);
                    }
                }
            }
            _eventTargetDict.Remove(target);
        }
    }

    public List<Action<object[]>> GetAllEventMessageHandler(T t)
    {
        _eventMessageHandlerDict.TryGetValue(t, out List<Action<object[]>> lst);
        return lst;
    }
}

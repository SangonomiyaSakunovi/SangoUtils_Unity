using System;
using System.Collections.Generic;

public class EventMessageMap
{
    private Dictionary<int, List<Action<IEventMessageBase>>> _eventMessageHandlerDict = new Dictionary<int, List<Action<IEventMessageBase>>>();
    private Dictionary<object, List<int>> _eventTargetDict = new Dictionary<object, List<int>>();

    public void AddEventMessageHandler(int eventId, Action<IEventMessageBase> eventMessage)
    {
        _eventMessageHandlerDict.TryGetValue(eventId, out List<Action<IEventMessageBase>> existedActionList);
        if (existedActionList == null)
        {
            existedActionList = new List<Action<IEventMessageBase>>();
            _eventMessageHandlerDict.Add(eventId, existedActionList);
        }
        if (existedActionList.Find(existedAction => existedAction.Equals(eventMessage)) != null)
        {
            return;
        }
        else
        {
            existedActionList.Add(eventMessage);
        }
        if (eventMessage != null && eventMessage.Target != null)
        {
            _eventTargetDict.TryGetValue(eventMessage.Target, out List<int> existedTargetList);
            if (existedTargetList == null)
            {
                existedTargetList = new List<int>();
            }
            existedTargetList.Add(eventId);
        }
    }

    public void RemoveEventMessageHandler(int eventId)
    {
        if (_eventMessageHandlerDict.ContainsKey(eventId))
        {
            var handlerLst = _eventMessageHandlerDict[eventId];
            foreach (Action<IEventMessageBase> eventMessage in handlerLst)
            {
                if (eventMessage != null && eventMessage.Target != null && _eventTargetDict.ContainsKey(eventMessage.Target))
                {
                    var idLst = _eventTargetDict[eventMessage.Target];
                    idLst.RemoveAll((int id) =>
                    {
                        return eventId.Equals(id);
                    });
                    if (idLst.Count == 0)
                    {
                        _eventTargetDict.Remove(eventMessage.Target);
                    }
                }
            }
        }
        _eventMessageHandlerDict.Remove(eventId);
    }
    public void RemoveTargetHandler(object target)
    {
        if (_eventTargetDict.ContainsKey(target))
        {
            List<int> eventIdList = _eventTargetDict[target];
            for (int i = eventIdList.Count - 1; i >= 0; --i)
            {
                int eventId = eventIdList[i];
                if (_eventMessageHandlerDict.ContainsKey(eventId))
                {
                    List<Action<IEventMessageBase>> eventMessageList = _eventMessageHandlerDict[eventId];
                    eventMessageList.RemoveAll((Action<IEventMessageBase> eventMessage) =>
                    {
                        return eventMessage.Target == target;
                    });
                    if (eventMessageList.Count == 0)
                    {
                        _eventMessageHandlerDict.Remove(eventId);
                    }
                }
            }
            _eventTargetDict.Remove(target);
        }
    }

    public List<Action<IEventMessageBase>> GetAllEventMessageHandler(int eventId)
    {
        _eventMessageHandlerDict.TryGetValue(eventId, out List<Action<IEventMessageBase>> lst);
        return lst;
    }
}

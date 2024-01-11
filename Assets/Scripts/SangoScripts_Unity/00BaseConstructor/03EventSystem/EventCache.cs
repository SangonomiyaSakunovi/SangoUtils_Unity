using System;
using System.Collections.Generic;

public class EventCache : BaseCache
{
    private Dictionary<int, List<Action<IEventMessageBase>>> _eventCacheDict;

    public EventCache()
    {
        _eventCacheDict = new Dictionary<int, List<Action<IEventMessageBase>>>();
    }

    public void AddEventListener<T>(Action<IEventMessageBase> eventMessage) where T : IEventMessageBase
    {
        Type eventType = typeof(T);
        int eventId = eventType.GetHashCode();
        AddEventListener(eventId, eventMessage);
    }

    public void AddEventListener(int eventHash, Action<IEventMessageBase> eventMessage)
    {        
        if (!_eventCacheDict.ContainsKey(eventHash))
        {
            _eventCacheDict.Add(eventHash, new List<Action<IEventMessageBase>>());
        }
        if (!_eventCacheDict[eventHash].Contains(eventMessage))
        {
            _eventCacheDict[eventHash].Add(eventMessage);
            EventService.Instance.AddEventListener(eventHash, eventMessage);
        }
        else
        {
            SangoLogger.Warning($"Event listener is exist : {eventHash}");
        }
    }

    public void RemoveAllListeners()
    {
        foreach (var eventId in _eventCacheDict.Keys)
        {
            EventService.Instance.RemoveEventListener(eventId);
        }
    }
}

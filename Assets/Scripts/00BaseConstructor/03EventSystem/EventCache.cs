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
        if (!_eventCacheDict.ContainsKey(eventId))
        {
            _eventCacheDict.Add(eventId, new List<Action<IEventMessageBase>>());
        }
        if (!_eventCacheDict[eventId].Contains(eventMessage))
        {
            _eventCacheDict[eventId].Add(eventMessage);
            EventService.Instance.AddEventListener(eventId, eventMessage);
        }
        else
        {
            SangoLogger.Warning($"Event listener is exist : {eventType}");
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

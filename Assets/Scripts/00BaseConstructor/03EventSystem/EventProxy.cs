using System;
using System.Collections.Generic;

public class EventProxy<T>
{
    private static string _lock = "_eventMessageLock";
    private Queue<EventMessagePararms> _eventMessageQueue = new Queue<EventMessagePararms>();
    private EventMessageMap<T> _eventMessageMap = new EventMessageMap<T>();

    public void Init()
    {
        _eventMessageQueue.Clear();
    }
    public void UpdateEventProxyQueue()
    {
        lock (_lock)
        {
            while (_eventMessageQueue.Count > 0)
            {
                EventMessagePararms data = _eventMessageQueue.Dequeue();
                TriggeEventMessageHandler(data.GetEventId(), data.GetParams());
            }
        }
    }
    public void Clear() { }

    public void AddEventMessageHandler(T evt, Action<object[]> cb)
    {
        lock (_lock)
        {
            _eventMessageMap.AddEventMessageHandler(evt, cb);
        }
    }
    public void RemoveEventMessageHandlerByEventId(T id)
    {
        lock (_lock)
        {
            _eventMessageMap.RemoveEventMessageHandler(id);
        }
    }
    public void RemoveEventMessageHandlerByTarget(object target)
    {
        lock (_lock)
        {
            _eventMessageMap.RemoveTargetHandler(target);
        }
    }

    public void InvokeEventMessageHandler(T evt, object[] paramLists = null)
    {
        lock (_lock)
        {
            _eventMessageQueue.Enqueue(new EventMessagePararms(evt, paramLists));
        }
    }

    public void InvokeEventMessageHandlerImmediately(T evt, object[] paramLists = null)
    {
        TriggeEventMessageHandler(evt, paramLists);
    }

    private void TriggeEventMessageHandler(T t, object[] paramLists)
    {
        List<Action<object[]>> lst = _eventMessageMap.GetAllEventMessageHandler(t);
        if (lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                lst[i](paramLists);
            }
        }
    }

    private class EventMessagePararms
    {
        private T _eventId = default(T);
        private object[] _paramLists = null;
        public EventMessagePararms(T t, object[] paramLists)
        {
            _eventId = t;
            _paramLists = paramLists;
        }

        public T GetEventId()
        {
            return _eventId;
        }
        public object[] GetParams() { return _paramLists; }
    }
}

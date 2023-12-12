using System;
using System.Collections.Generic;

public class EventProxy
{
    private static string _lock = "_eventMessageLock";
    private Queue<IEventMessageBase> _eventMessageQueue = new Queue<IEventMessageBase>();
    private EventMessageMap _eventMessageMap = new EventMessageMap();

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
                IEventMessageBase eventMessage = _eventMessageQueue.Dequeue();
                Type eventType = eventMessage.GetType();
                int eventId = eventType.GetHashCode();
                TriggeEventMessageHandler(eventId, eventMessage);
            }
        }
    }
    public void Clear() { }

    public void AddEventMessageHandler(int eventId, Action<IEventMessageBase> eventMessage)
    {
        lock (_lock)
        {
            _eventMessageMap.AddEventMessageHandler(eventId, eventMessage);
        }
    }
    public void RemoveEventMessageHandlerByEventId(int eventId)
    {
        lock (_lock)
        {
            _eventMessageMap.RemoveEventMessageHandler(eventId);
        }
    }
    public void RemoveEventMessageHandlerByTarget(object target)
    {
        lock (_lock)
        {
            _eventMessageMap.RemoveTargetHandler(target);
        }
    }

    public void InvokeEventMessageProxyMode(IEventMessageBase eventMessage)
    {
        lock (_lock)
        {
            _eventMessageQueue.Enqueue(eventMessage);
        }
    }

    public void InvokeEventMessageImmediately(IEventMessageBase eventMessage)
    {
        Type eventType = eventMessage.GetType();
        int eventId = eventType.GetHashCode();
        TriggeEventMessageHandler(eventId, eventMessage);
    }

    private void TriggeEventMessageHandler(int eventId, IEventMessageBase eventMessage)
    {
        List<Action<IEventMessageBase>> eventMessageList = _eventMessageMap.GetAllEventMessageHandler(eventId);
        if (eventMessageList != null)
        {
            for (int i = 0; i < eventMessageList.Count; i++)
            {
                eventMessageList[i].Invoke(eventMessage);
            }
        }
    }
}

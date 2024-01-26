using SangoUtils_Bases_UnityEngine;
using SangoUtils_Event;
using System;

public class EventService : BaseService<EventService>
{
    private EventListenerHandler _eventHandler;

    public override void OnInit()
    {
        base.OnInit(); 
        _eventHandler = new();
        _eventHandler.Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _eventHandler.UpdateEventListenerQueue();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        _eventHandler.Clear();
    }

    public void AddEventListener<T>(Action<IEventMessageBase> eventMessage) where T : IEventMessageBase
    {
        Type eventType = typeof(T);
        int eventId = eventType.GetHashCode();
        AddEventListener(eventId, eventMessage);
    }

    public void AddEventListener(int eventId, Action<IEventMessageBase> eventMessage)
    {
        _eventHandler.AddEventMessageListener(eventId, eventMessage);
    }

    public void RemoveEventListener<T>() where T : IEventMessageBase
    {
        Type eventType = typeof(T);
        int eventId = eventType.GetHashCode();
        RemoveEventListener(eventId);
    }

    public void RemoveEventListener(int eventId)
    {
        _eventHandler.RemoveEventMessageListenerByEventId(eventId);
    }

    public void RemoveTargetListener(object target)
    {
        _eventHandler.RemoveEventMessageListenerByTarget(target);
    }

    public void SendEventMessage(IEventMessageBase eventMessage)
    {
        _eventHandler.SendEventMessage(eventMessage);
    }

    public void PostEventMessage(IEventMessageBase eventMessage)
    {
        _eventHandler.SendEventMessageAsync(eventMessage);
    }
}
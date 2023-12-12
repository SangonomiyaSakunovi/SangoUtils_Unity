using System;

public class EventService : BaseService<EventService>
{
    private EventProxy _eventProxy;

    public override void OnInit()
    {
        base.OnInit();
        _eventProxy = new EventProxy();
        _eventProxy.Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _eventProxy.UpdateEventProxyQueue();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        _eventProxy.Clear();
    }

    public void AddEventListener<T>(Action<IEventMessageBase> eventMessage) where T : IEventMessageBase
    {
        Type eventType = typeof(T);
        int eventId = eventType.GetHashCode();
        _eventProxy.AddEventMessageHandler(eventId, eventMessage);
    }

    public void RemoveEventListener<T>() where T : IEventMessageBase
    {
        Type eventType = typeof(T);
        int eventId = eventType.GetHashCode();
        _eventProxy.RemoveEventMessageHandlerByEventId(eventId);
    }

    public void RemoveTargetListener(object target)
    {
        _eventProxy.RemoveEventMessageHandlerByTarget(target);
    }

    public void SendEventMessage(IEventMessageBase eventMessage)
    {
        _eventProxy.InvokeEventMessageImmediately(eventMessage);
    }

    public void PostEventMessage(IEventMessageBase eventMessage)
    {
        _eventProxy.InvokeEventMessageProxyMode(eventMessage);
    }
}

public interface IEventMessageBase
{

}
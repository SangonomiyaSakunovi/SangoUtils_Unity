using System;

public class EventService : BaseService<EventService>
{
    private EventProxy<EventId> _eventProxy;

    public override void OnInit()
    {
        base.OnInit();
        _eventProxy = new EventProxy<EventId>();
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

    public void AddEventListener(EventId evt, Action<object[]> cb)
    {
        _eventProxy.AddEventMessageHandler(evt, cb);
    }

    public void RemoveEventListener(EventId evt)
    {
        _eventProxy.RemoveEventMessageHandlerByEventId(evt);
    }

    public void RemoveTargetListener(object target)
    {
        _eventProxy.RemoveEventMessageHandlerByTarget(target);
    }

    public void InvokeEventListener(EventId evt, object[] paramList = null)
    {
        _eventProxy.InvokeEventMessageHandler(evt, paramList);
    }

    public void InvokeEventListenerImmediately(EventId evt, object[] paramList = null)
    {
        _eventProxy.InvokeEventMessageHandlerImmediately(evt, paramList);
    }
}

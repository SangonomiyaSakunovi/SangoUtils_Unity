using SangoScripts_Unity.Net;

public abstract class BaseUdpEvent
{
    public int UdpEventPortId { get; private set; }

    public virtual void OnEventDataReceived<T>(T data) where T : class
    {

    }

    public virtual void OnInit(int eventPortId)
    {
        UdpEventPortId = eventPortId;
        UdpEventService.Instance.AddUdpEvent(this);
    }

    public virtual void OnDispose()
    {
        UdpEventService.Instance.RemoveUdpEvent(this);
    }
}

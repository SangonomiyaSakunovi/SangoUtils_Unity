using System.Collections.Generic;

public class UdpEventService : BaseService<UdpEventService>
{
    private Dictionary<int, UdpClientSango> _udpListenerClients;
    private Dictionary<int, BaseUdpEvent> _eventDict;

    private List<UdpData> _udpDatas = new List<UdpData>(10);

    public override void OnInit()
    {
        base.OnInit();
        _eventDict = new Dictionary<int, BaseUdpEvent>();
        _udpListenerClients = new Dictionary<int, UdpClientSango>();
        InitUdpListenerClients();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        HandleEventReceivedData();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private void InitUdpListenerClients()
    {
        UdpClientSango typeInClient = new UdpClientSango<string>(UdpEventListenPortId.typeInPort);
        _udpListenerClients.Add(typeInClient._udpListenerPortId, typeInClient);
    }

    private void HandleEventReceivedData()
    {
        if (_udpDatas.Count == 0) return;
        for (int i = 0; i < _udpDatas.Count; i++)
        {
            UdpData data = _udpDatas[i];
            data.OnDataSync();
        }
        _udpDatas.Clear();
    }

    public void AddUpdEventReceivedData(UdpData udpData)
    {
        lock ("locker_AddUpdEventReceivedData")
        {
            _udpDatas.Add(udpData);
        }
    }

    public void UdpEventBroadcast<T>(T data, int eventId) where T : class
    {
        if (_eventDict.TryGetValue(eventId, out BaseUdpEvent netEvent))
        {
            netEvent.OnEventDataReceived<T>(data);
        }
    }

    public void AddUdpEvent(BaseUdpEvent evt)
    {
        _eventDict.Add(evt.UdpEventPortId, evt);
    }

    public T GetUdpEvent<T>(int udpEventPortId) where T : BaseUdpEvent, new()
    {
        if (_eventDict.ContainsKey(udpEventPortId))
        {
            return (T)_eventDict[udpEventPortId];
        }
        else
        {
            T request = new T();
            request.OnInit(udpEventPortId);
            return request;
        }
    }

    public void RemoveUdpEvent(BaseUdpEvent evt)
    {
        _eventDict.Remove(evt.UdpEventPortId);
    }
}

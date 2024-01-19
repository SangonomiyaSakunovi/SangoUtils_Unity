using SangoUtils_UDP;

namespace SangoUtils_Unity_Scripts.Net
{
    public class UDPEventService : BaseService<UDPEventService>
    {
        private NetUdpEventHandler _eventHandler;

        public override void OnInit()
        {
            base.OnInit();
            _eventHandler = new NetUdpEventHandler();
            UdpClientPeer<string> peer = new UdpClientPeer<string>(UdpEventListenPortID.TestPort, _eventHandler);
            peer.Open();
            _eventHandler.AddUdpClientPeer(peer);
        }

        public T GetUdpEvent<T>(int udpEventPortId) where T : BaseUdpEvent, new()
        {
            return _eventHandler.GetUdpEvent<T>(udpEventPortId);
        }

        protected override void OnUpdate()
        {
            _eventHandler.OnUpdate();
        }
    }
}

public class UdpEventListenPortID : UdpEventBaseListenPortID<UdpEventListenPortID>
{
    [UdpEventPortApiKey("TestPort")]
    public const int TestPort = 10010;
}

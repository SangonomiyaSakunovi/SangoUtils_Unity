using SangoNetProtol;
using SangoUtils_IOCP;
using System.Collections.Generic;

namespace SangoScripts_Unity.Net
{
    public class IOCPService : BaseNetService<IOCPService>
    {
        private IOCPPeer<IOCPClientPeer> _clientPeerInstance;

        private Queue<SangoNetMessage> _netMessageProxyQueue = new();

        private NetEnvironmentConfig _currentNetEnvironmentConfig;

        public override void OnInit()
        {
            base.OnInit();

            string ipAddress = _currentNetEnvironmentConfig.ServerAddress;
            int port = _currentNetEnvironmentConfig.ServerPort;
            InitClientInstance(ipAddress, port);

            DefaultIOCPRequest defaultNetRequest = GetNetRequest<DefaultIOCPRequest>(NetOperationCode.Default);
            DefaultIOCPEvent defaultNetEvent = GetNetEvent<DefaultIOCPEvent>(NetOperationCode.Default);
        }

        public override void SetConfig(NetEnvironmentConfig netEnvironmentConfig)
        {
            _currentNetEnvironmentConfig = netEnvironmentConfig;
        }

        protected override void OnUpdate()
        {
            if (_netMessageProxyQueue.Count > 0)
            {
                SangoNetMessage sangoNetMessage = _netMessageProxyQueue.Dequeue();
                OnMessageReceivedInMainThread(sangoNetMessage);
            }
        }

        public override void SendOperationRequest(NetOperationCode operationCode, string messageStr)
        {
            _clientPeerInstance.ClientPeer.SendOperationRequest(operationCode, messageStr);
        }

        public override void SendOperationBroadcast(NetOperationCode operationCode, string messageStr)
        {
            throw new System.NotImplementedException();
        }

        private void AddNetMessageProxy(SangoNetMessage sangoNetMessage)
        {
            _netMessageProxyQueue.Enqueue(sangoNetMessage);
        }

        public override void OnMessageReceived(SangoNetMessage sangoNetMessage)
        {
            AddNetMessageProxy(sangoNetMessage);
        }

        private void OnMessageReceivedInMainThread(SangoNetMessage sangoNetMessage)
        {
            NetMessageCommandBroadcast(sangoNetMessage);
        }

        private void InitClientInstance(string ipAddress, int port)
        {
            _clientPeerInstance = new IOCPPeer<IOCPClientPeer>();
            _clientPeerInstance.OpenAsUnityClient(ipAddress, port);
        }

        public void CloseClientInstance()
        {
            _clientPeerInstance.CloseAsClient();
        }
    }

    public class NetEnvironmentConfig : BaseConfig
    {
        public NetEnvMode NetEnvMode { get; set; } = NetEnvMode.Offline;
        public string ServerAddress { get; set; } = "127.0.0.1";
        public int ServerPort { get; set; } = 52037;
        public string ServerAddressAndPort { get; set; } = "ws://127.0.0.1:52037";
    }

    public enum NetEnvMode
    {
        Offline,
        Online_IOCP,
        Online_Http,
        Online_WebSocket
    }
}

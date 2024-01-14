using SangoNetProtol;
using SangoUtils_NetOperation;

namespace SangoScripts_Unity.Net
{
    public class WebSocketService : BaseService<WebSocketService>, INetOperation
    {
        private WebSocketClientPeer _clientPeerInstance;
        private NetClientOperationHandler _netOperationHandler = new();

        private NetEnvironmentConfig _currentNetEnvironmentConfig;

        public override void OnInit()
        {
            base.OnInit();
            string ipAddressAndPort = _currentNetEnvironmentConfig.ServerAddressAndPort;
            InitClientInstance(ipAddressAndPort);

            DefaultWebSocketRequest defaultWebSocketRequest = _netOperationHandler.GetNetRequest<DefaultWebSocketRequest>(NetOperationCode.Default);
            DefaultWebSocketEvent defaultWebSocketEvent = _netOperationHandler.GetNetEvent<DefaultWebSocketEvent>(NetOperationCode.Default);
            DefaultWebSocketBroadcast defaultWebSocketBroadcast = _netOperationHandler.GetNetBroadcast<DefaultWebSocketBroadcast>(NetOperationCode.Default);
        }

        public void SetConfig(NetEnvironmentConfig netEnvironmentConfig)
        {
            _currentNetEnvironmentConfig = netEnvironmentConfig;
        }

        public void SendOperationRequest(NetOperationCode operationCode, string messageStr)
        {
            _clientPeerInstance.SendOperationRequest(operationCode, messageStr);
        }

        public void SendOperationBroadcast(NetOperationCode operationCode, string messageStr)
        {
            _clientPeerInstance.SendOperationBroadcast(operationCode, messageStr);
        }

        public void OnMessageReceived(SangoNetMessage sangoNetMessage)
        {
            _netOperationHandler.NetMessageCommandBroadcast(sangoNetMessage);
        }

        //public override void OnBinaryReceived(SangoNetMessage sangoNetMessage)
        //{
        //    NetMessageCommandBroadcast(sangoNetMessage);
        //}

        private void InitClientInstance(string ipAddressAndPort)
        {
            _clientPeerInstance = new(ipAddressAndPort);
        }

        public void CloseClientInstance()
        {
            _clientPeerInstance.Close();
        }

        public T GetNetRequest<T>(NetOperationCode netOperationCode) where T : BaseNetRequest, new()
        {
            return _netOperationHandler.GetNetRequest<T>(netOperationCode);
        }

        public T GetNetEvent<T>(NetOperationCode operationCode) where T : BaseNetEvent, new()
        {
            return _netOperationHandler.GetNetEvent<T>(operationCode);
        }

        public T GetNetBroadcast<T>(NetOperationCode operationCode) where T : BaseNetBroadcast, new()
        {
            return _netOperationHandler.GetNetBroadcast<T>(operationCode);
        }
    }
}


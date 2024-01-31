using SangoNetProtol;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_NetOperation;

namespace SangoUtils_Unity_Scripts.Net
{
    public class WebSocketService : BaseService<WebSocketService>, INetClientOperation
    {
        private WebSocketClientPeer _clientPeerInstance;
        private NetClientOperationHandler _netOperationHandler = new();

        private NetEnvironmentConfig _currentNetEnvironmentConfig;

        public override void OnInit()
        {
            
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

        protected override void OnUpdate()
        {
            
        }

        public override void OnDispose()
        {
            
        }

        public void OpenClient()
        {
            string ipAddressAndPort = _currentNetEnvironmentConfig.ServerAddressAndPort;
            _clientPeerInstance = new(ipAddressAndPort);

            DefaultWebSocketRequest defaultWebSocketRequest = _netOperationHandler.GetNetRequest<DefaultWebSocketRequest>(NetOperationCode.Default);
            DefaultWebSocketEvent defaultWebSocketEvent = _netOperationHandler.GetNetEvent<DefaultWebSocketEvent>(NetOperationCode.Default);
            DefaultWebSocketBroadcast defaultWebSocketBroadcast = _netOperationHandler.GetNetBroadcast<DefaultWebSocketBroadcast>(NetOperationCode.Default);

            WebSocketEvent_Ping ping = _netOperationHandler.GetNetEvent<WebSocketEvent_Ping>(NetOperationCode.Ping);
        }

        public void CloseClient()
        {
            _clientPeerInstance.Close();
        }
    }
}


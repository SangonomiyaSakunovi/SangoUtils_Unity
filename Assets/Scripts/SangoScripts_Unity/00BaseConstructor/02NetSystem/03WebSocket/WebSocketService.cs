using SangoNetProtol;

public class WebSocketService : BaseNetService<WebSocketService>
{
    private WebSocketClientPeer _clientPeerInstance;

    private NetEnvironmentConfig _currentNetEnvironmentConfig;

    public override void OnInit()
    {
        base.OnInit();
        string ipAddressAndPort = _currentNetEnvironmentConfig.ServerAddressAndPort;
        InitClientInstance(ipAddressAndPort);

        DefaultWebSocketRequest defaultWebSocketRequest = GetNetRequest<DefaultWebSocketRequest>(NetOperationCode.Default);
        DefaultWebSocketEvent defaultWebSocketEvent = GetNetEvent<DefaultWebSocketEvent>(NetOperationCode.Default);
        DefaultWebSocketBroadcast defaultWebSocketBroadcast = GetNetBroadcast<DefaultWebSocketBroadcast>(NetOperationCode.Default);
    }

    public override void SetConfig(NetEnvironmentConfig netEnvironmentConfig)
    {
        _currentNetEnvironmentConfig = netEnvironmentConfig;
    }

    public override void SendOperationRequest(NetOperationCode operationCode, string messageStr)
    {
        _clientPeerInstance.SendOperationRequest(operationCode, messageStr);
    }

    public override void SendOperationBroadcast(NetOperationCode operationCode, string messageStr)
    {
        _clientPeerInstance.SendOperationBroadcast(operationCode, messageStr);
    }

    public override void OnMessageReceived(SangoNetMessage sangoNetMessage)
    {
        NetMessageCommandBroadcast(sangoNetMessage);
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
}

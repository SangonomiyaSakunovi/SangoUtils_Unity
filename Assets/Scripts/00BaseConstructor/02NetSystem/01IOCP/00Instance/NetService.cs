using SangoNetProtol;
using System.Collections.Generic;

public class NetService : BaseService<NetService>
{
    public IOCPPeer<ClientPeer> ClientPeerInstance;

    private Dictionary<NetOperationCode, BaseNetRequest> _netRequestDict = new Dictionary<NetOperationCode, BaseNetRequest>();

    private Queue<SangoNetMessage> _netMessageProxyQueue = new Queue<SangoNetMessage>();

    public override void OnInit()
    {
        base.OnInit();

        string ipAddress = SangoSystemConfig.NetEnvironmentConfig.serverAddress;
        int port = SangoSystemConfig.NetEnvironmentConfig.serverPort;
        InitClientInstance(ipAddress, port);

        DefaultNetRequest defaultNetRequest = GetNetRequest<DefaultNetRequest>(NetOperationCode.Default);
    }

    protected override void OnUpdate()
    {
        if (_netMessageProxyQueue.Count > 0)
        {
            SangoNetMessage sangoNetMessage = _netMessageProxyQueue.Dequeue();
            OnRecievedMessageInMainThread(sangoNetMessage);
        }
    }

    public void AddNetMessageProxy(SangoNetMessage sangoNetMessage)
    {
        _netMessageProxyQueue.Enqueue(sangoNetMessage);
    }

    public void AddNetRequest(BaseNetRequest netRequest)
    {
        if (!_netRequestDict.ContainsKey(netRequest.NetOperationCode))
        {
            _netRequestDict.Add(netRequest.NetOperationCode, netRequest);
        }
        else
        {
            SangoLogger.Error("Aleady has this request.");
        }
    }

    public void RemoveNetRequest(BaseNetRequest netRequest)
    {
        if (_netRequestDict.ContainsKey(netRequest.NetOperationCode))
        {
            _netRequestDict.Remove(netRequest.NetOperationCode);
        }
        else
        {
            SangoLogger.Error("Already remove this request.");
        }
    }

    public T GetNetRequest<T>(NetOperationCode operationCode) where T : BaseNetRequest, new()
    {
        if (_netRequestDict.ContainsKey(operationCode))
        {
            return (T)_netRequestDict[operationCode];
        }
        else
        {
            T netRequest = new();
            netRequest.OnInit(operationCode);
            return netRequest;
        }
    }

    private void NetResponseMessageBroadcast(SangoNetMessage sangoNetMessage)
    {
        if (_netRequestDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetRequest netRequest))
        {
            netRequest.OnOperationResponse(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
        else
        {

        }
    }

    private void NetEventMessageBroadcast(SangoNetMessage sangoNetMessage)
    {

    }

    private void OnRecievedMessageInMainThread(SangoNetMessage sangoNetMessage)
    {
        switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
        {
            case NetMessageCommandCode.NetOperationResponse:
                {
                    NetResponseMessageBroadcast(sangoNetMessage);
                }
                break;
            case NetMessageCommandCode.NetEventData:
                {
                    NetEventMessageBroadcast(sangoNetMessage);
                }
                break;
        }
    }

    private void InitClientInstance(string ipAddress, int port)
    {
        ClientPeerInstance = new IOCPPeer<ClientPeer>();
        ClientPeerInstance.InitAsClient(ipAddress, port);
    }

    public void CloseClientInstance()
    {
        ClientPeerInstance.CloseClient();
    }
}

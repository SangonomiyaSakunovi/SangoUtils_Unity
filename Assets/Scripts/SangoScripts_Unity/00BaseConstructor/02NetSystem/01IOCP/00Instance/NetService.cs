using SangoNetProtol;
using System.Collections.Generic;

public class NetService : BaseService<NetService>
{
    public IOCPPeer<ClientPeer> ClientPeerInstance;

    private Dictionary<NetOperationCode, BaseNetRequest> _netRequestDict = new();
    private Dictionary<NetOperationCode, BaseNetEvent> _netEventDict = new();

    private Queue<SangoNetMessage> _netMessageProxyQueue = new();

    private NetEnvironmentConfig _currentNetEnvironmentConfig;

    public override void OnInit()
    {
        base.OnInit();

        string ipAddress = _currentNetEnvironmentConfig.serverAddress;
        int port = _currentNetEnvironmentConfig.serverPort;
        InitClientInstance(ipAddress, port);

        DefaultNetRequest defaultNetRequest = GetNetRequest<DefaultNetRequest>(NetOperationCode.Default);
        DefaultNetEvent defaultNetEvent = GetNetEvent<DefaultNetEvent>(NetOperationCode.Default);
    }

    public void SetConfig(NetEnvironmentConfig netEnvironmentConfig)
    {
        _currentNetEnvironmentConfig = netEnvironmentConfig;
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
            SangoLogger.Error("Aleady has this NetRequest.");
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

    public void RemoveNetRequest(BaseNetRequest netRequest)
    {
        if (_netRequestDict.ContainsKey(netRequest.NetOperationCode))
        {
            _netRequestDict.Remove(netRequest.NetOperationCode);
        }
        else
        {
            SangoLogger.Error("Already remove this NetRequest.");
        }
    }

    public void AddNetEvent(BaseNetEvent netEvent)
    {
        if (!_netEventDict.ContainsKey(netEvent.NetOperationCode))
        {
            _netEventDict.Add(netEvent.NetOperationCode, netEvent);
        }
        else
        {
            SangoLogger.Error("Aleady has this NetEvent.");
        }
    }

    public T GetNetEvent<T>(NetOperationCode operationCode) where T : BaseNetEvent, new()
    {
        if (_netEventDict.ContainsKey(operationCode))
        {
            return (T)_netEventDict[operationCode];
        }
        else
        {
            T netEvent = new();
            netEvent.OnInit(operationCode);
            return netEvent;
        }
    }

    public void RemoveNetEvent(BaseNetEvent netEvent)
    {
        if (_netEventDict.ContainsKey(netEvent.NetOperationCode))
        {
            _netEventDict.Remove(netEvent.NetOperationCode);
        }
        else
        {
            SangoLogger.Error("Already remove this NetEvent.");
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
            _netRequestDict.TryGetValue(NetOperationCode.Default, out BaseNetRequest? defaultNetRequest);
            defaultNetRequest?.OnOperationResponse(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
    }

    private void NetEventMessageBroadcast(SangoNetMessage sangoNetMessage)
    {
        if (_netEventDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetEvent netEvent))
        {
            netEvent.OnOperationEvent(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
        else
        {
            _netEventDict.TryGetValue(NetOperationCode.Default, out BaseNetEvent? defaultNetEvent);
            defaultNetEvent?.OnOperationEvent(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
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

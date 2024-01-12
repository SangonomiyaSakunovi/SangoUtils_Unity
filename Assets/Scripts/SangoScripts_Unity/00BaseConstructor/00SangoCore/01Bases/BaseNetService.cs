using SangoNetProtol;
using SangoScripts_Unity.Net;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNetService<T> : BaseService<T> where T : MonoBehaviour
{
    private Dictionary<NetOperationCode, BaseNetRequest> _netRequestDict = new();
    private Dictionary<NetOperationCode, BaseNetEvent> _netEventDict = new();
    private Dictionary<NetOperationCode, BaseNetBroadcast> _netBroadcastDict = new();

    public abstract void SetConfig(NetEnvironmentConfig netEnvironmentConfig);

    public abstract void SendOperationRequest(NetOperationCode operationCode, string messageStr);

    public abstract void SendOperationBroadcast(NetOperationCode operationCode, string messageStr);

    public abstract void OnMessageReceived(SangoNetMessage sangoNetMessage);

    protected void NetMessageCommandBroadcast(SangoNetMessage sangoNetMessage)
    {
        switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
        {
            case NetMessageCommandCode.NetOperationResponse:
                {
                    NetMessageResponsedBroadcast(sangoNetMessage);
                }
                break;
            case NetMessageCommandCode.NetEventData:
                {
                    NetMessageEventBroadcast(sangoNetMessage);
                }
                break;
            case NetMessageCommandCode.NetBroadcast:
                {
                    NetMessageBroadcastBroadcast(sangoNetMessage);
                }
                break;
        }
    }

    private void NetMessageResponsedBroadcast(SangoNetMessage sangoNetMessage)
    {
        if (_netRequestDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetRequest netRequest))
        {
            netRequest.OnOperationResponse(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
        else
        {
            _netRequestDict.TryGetValue(NetOperationCode.Default, out BaseNetRequest defaultNetRequest);
            defaultNetRequest?.OnOperationResponse(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
    }

    private void NetMessageEventBroadcast(SangoNetMessage sangoNetMessage)
    {
        if (_netEventDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetEvent netEvent))
        {
            netEvent.OnEventData(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
        else
        {
            _netEventDict.TryGetValue(NetOperationCode.Default, out BaseNetEvent defaultNetEvent);
            defaultNetEvent?.OnEventData(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
    }

    private void NetMessageBroadcastBroadcast(SangoNetMessage sangoNetMessage)
    {
        if (_netBroadcastDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetBroadcast netBroadcast))
        {
            netBroadcast.OnBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
        else
        {
            _netBroadcastDict.TryGetValue(NetOperationCode.Default, out BaseNetBroadcast defaultNetBroadcast);
            defaultNetBroadcast?.OnBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr);
        }
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

    public K GetNetRequest<K>(NetOperationCode operationCode) where K : BaseNetRequest, new()
    {
        if (_netRequestDict.ContainsKey(operationCode))
        {
            return (K)_netRequestDict[operationCode];
        }
        else
        {
            K netRequest = new();
            netRequest.OnInit<T>(operationCode, this);
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

    public K GetNetEvent<K>(NetOperationCode operationCode) where K : BaseNetEvent, new()
    {
        if (_netEventDict.ContainsKey(operationCode))
        {
            return (K)_netEventDict[operationCode];
        }
        else
        {
            K netEvent = new();
            netEvent.OnInit<T>(operationCode, this);
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

    public void AddNetBroadcast(BaseNetBroadcast netBroadcast)
    {
        if (!_netBroadcastDict.ContainsKey(netBroadcast.NetOperationCode))
        {
            _netBroadcastDict.Add(netBroadcast.NetOperationCode, netBroadcast);
        }
        else
        {
            SangoLogger.Error("Aleady has this NetBroadcast.");
        }
    }

    public K GetNetBroadcast<K>(NetOperationCode operationCode) where K : BaseNetBroadcast, new()
    {
        if (_netBroadcastDict.ContainsKey(operationCode))
        {
            return (K)_netBroadcastDict[operationCode];
        }
        else
        {
            K netEvent = new();
            netEvent.OnInit<T>(operationCode, this);
            return netEvent;
        }
    }

    public void RemoveNetBroadcast(BaseNetBroadcast netBroadcast)
    {
        if (_netBroadcastDict.ContainsKey(netBroadcast.NetOperationCode))
        {
            _netBroadcastDict.Remove(netBroadcast.NetOperationCode);
        }
        else
        {
            SangoLogger.Error("Already remove this NetBroadcast.");
        }
    }
}

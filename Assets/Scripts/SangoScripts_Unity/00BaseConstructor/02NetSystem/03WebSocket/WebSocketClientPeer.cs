using Best.HTTP.Shared.PlatformSupport.Memory;
using Best.WebSockets;
using SangoNetProtol;
using SangoUtils_Common.Utils;
using System;
using System.Collections.Generic;
using static System.Net.WebRequestMethods;

public class WebSocketClientPeer
{
    private WebSocket _websocketClient;

    private long _lastMessageTimestamp = long.MinValue;

    public WebSocketClientPeer(string ipAddressAndPort)
    {
        _websocketClient = new(new Uri(ipAddressAndPort));
        _websocketClient.OnOpen += OnWebSocketOpen;
        _websocketClient.OnMessage += OnMessageReceived;
        _websocketClient.OnBinary += OnBinaryMessageReceived;
        _websocketClient.OnClosed += OnWebSocketClosed;
        _websocketClient.Open();
    }

    public void SendOperationRequest(NetOperationCode operationCode, string messageStr)
    {
        NetMessageHead messageHead = new()
        {
            NetOperationCode = operationCode,
            NetMessageCommandCode = NetMessageCommandCode.NetOperationRequest
        };
        NetMessageBody messageBody = new()
        {
            NetMessageStr = messageStr
        };
        SangoNetMessage message = new()
        {
            NetMessageHead = messageHead,
            NetMessageBody = messageBody,
            NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
        };
        SendData(message);
    }

    public void SendOperationBroadcast(NetOperationCode operationCode, string messageStr)
    {
        NetMessageHead messageHead = new()
        {
            NetOperationCode = operationCode,
            NetMessageCommandCode = NetMessageCommandCode.NetBroadcast
        };
        NetMessageBody messageBody = new()
        {
            NetMessageStr = messageStr
        };
        SangoNetMessage message = new()
        {
            NetMessageHead = messageHead,
            NetMessageBody = messageBody,
            NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
        };
        SendData(message);
    }

    public void Close()
    {
        _websocketClient.Close();
    }

    //private void SendData(SangoNetMessage message)
    //{
    //    byte[] bytes = ProtoUtils.SetProtoBytes(message);
    //    _websocketClient.Send(bytes);        
    //}

    private void SendData(SangoNetMessage message)
    {
        string messageJson = JsonUtils.SetJsonString(message);
        _websocketClient.Send(messageJson);
    }

    private void OnWebSocketOpen(WebSocket webSocket)
    {
        SangoLogger.Done("WebSocket is open!");
    }

    private void OnMessageReceived(WebSocket webSocket, string message)
    {
        SangoLogger.Warning("Text Message received from server: " + message);
        SangoNetMessage sangoNetMessage = JsonUtils.DeJsonString<SangoNetMessage>(message);
        if (sangoNetMessage == null)
        {
            SangoLogger.Error("Can`t Dejson!!!");
        }
        else
        {
            WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
        }
    }

    private void OnBinaryMessageReceived(WebSocket webSocket, BufferSegment buffer)
    {
        SangoLogger.Log("Binary Message received from server. Length: " + buffer.Count);
        SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(buffer);
        WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
        //long messageTimestamp = Convert.ToInt64(sangoNetMessage.NetMessageTimestamp);
        //if (messageTimestamp > _lastMessageTimestamp)
        //{
        //    _lastMessageTimestamp = messageTimestamp;
        //    WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
        //}
    }

    private void OnWebSocketClosed(WebSocket webSocket, WebSocketStatusCodes code, string message)
    {
        SangoLogger.Done("WebSocket is Closed!");
        if (code == WebSocketStatusCodes.NormalClosure)
        {
            // Closed by request
        }
        else
        {
            // Error
        }
    }
}

using Best.HTTP.Shared.PlatformSupport.Memory;
using Best.WebSockets;
using SangoNetProtol;
using SangoUtils_Common.Utils;
using SangoUtils_Extensions_Universal.Utils;
using SangoUtils_Logger;
using System;

namespace SangoUtils_Unity_Scripts.Net
{
    public class WebSocketClientPeer
    {
        private WebSocket _websocketClient;

        //private long _lastMessageTimestamp = long.MinValue;

        public WebSocketClientPeer(string ipAddressAndPort)
        {
            _websocketClient = new(new Uri(ipAddressAndPort));
            _websocketClient.OnOpen += OnOpen;
            _websocketClient.OnMessage += OnMessage;
            _websocketClient.OnBinary += OnBinary;
            _websocketClient.OnClosed += OnClosed;
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
                NetMessageTimestamp = DateTime.Now.ToUnixTimestampString()
            };
            Send(message);
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
                NetMessageTimestamp = DateTime.Now.ToUnixTimestampString()
            };
            Send(message);
        }

        public void Close()
        {
            _websocketClient.Close();
        }

        private void Send(SangoNetMessage message)
        {
            string messageJson = JsonUtils.ToJson(message);
            Send(messageJson);
            //byte[] bytes = ProtoUtils.SetProtoBytes(message);
            //Send(bytes);
        }

        private void Send(string jsonMessage)
        {
            _websocketClient.Send(jsonMessage);
        }

        private void Send(byte[] bufferMessage)
        {
            int count = bufferMessage.Length;
            _websocketClient.SendAsBinary(new BufferSegment(bufferMessage, 0, count));
        }

        private void OnOpen(WebSocket webSocket)
        {
            SangoLogger.Done("WebSocket is open!");
        }

        private void OnMessage(WebSocket webSocket, string message)
        {
            SangoLogger.Processing("ClientMessage: " + message);
            SangoNetMessage sangoNetMessage = JsonUtils.FromJson<SangoNetMessage>(message);
            if (sangoNetMessage != null)
            {
                WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
            }
        }

        private void OnBinary(WebSocket webSocket, BufferSegment buffer)
        {
            byte[] data = new byte[buffer.Count];
            Buffer.BlockCopy(buffer.Data, 0, data, 0, buffer.Count);
            SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(data);
            WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
        }

        private void OnClosed(WebSocket webSocket, WebSocketStatusCodes code, string message)
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
}
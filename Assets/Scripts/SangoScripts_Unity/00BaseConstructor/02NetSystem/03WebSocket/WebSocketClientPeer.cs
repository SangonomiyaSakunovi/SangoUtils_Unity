using Best.HTTP.Shared.PlatformSupport.Memory;
using Best.WebSockets;
using SangoNetProtol;
using SangoUtils_Common.Utils;
using SangoUtils_Logger;
using System;

namespace SangoScripts_Unity.Net
{
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

        private void SendData(SangoNetMessage message)
        {
            string messageJson = JsonUtils.SetJsonString(message);
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

        private void OnWebSocketOpen(WebSocket webSocket)
        {
            SangoLogger.Done("WebSocket is open!");
        }

        private void OnMessageReceived(WebSocket webSocket, string message)
        {
            SangoLogger.Processing("ClientMessage: " + message);
            SangoNetMessage sangoNetMessage = JsonUtils.DeJsonString<SangoNetMessage>(message);
            if (sangoNetMessage != null)
            {
                WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
            }
        }

        private void OnBinaryMessageReceived(WebSocket webSocket, BufferSegment buffer)
        {
            byte[] data = new byte[buffer.Count];
            Buffer.BlockCopy(buffer.Data, 0, data, 0, buffer.Count);
            SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(data);
            WebSocketService.Instance.OnMessageReceived(sangoNetMessage);
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
}
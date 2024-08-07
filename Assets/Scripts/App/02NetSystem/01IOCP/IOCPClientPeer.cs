using SangoNetProtol;
using SangoUtils_Common.Utils;
using SangoUtils_Extensions_Universal.Utils;
using SangoUtils_IOCP;
using SangoUtils_Logger;
using System;

namespace SangoUtils_Unity_Scripts.Net
{
    public class IOCPClientPeer : IClientPeer_IOCP
    {
        #region private
        private string _uid = "";
        private long _lastMessageTimestamp = long.MinValue;
        #endregion

        public string UID { get { return _uid; } }

        protected override void OnOpen()
        {
            SangoLogger.Log("Connect to Server.");
        }

        protected override void OnClosed()
        {
            SangoLogger.Log("We now DisConnect.");
        }

        protected override void OnBinary(byte[] byteMessages)
        {
            SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(byteMessages);
            long messageTimestamp = Convert.ToInt64(sangoNetMessage.NetMessageTimestamp);
            if (messageTimestamp > _lastMessageTimestamp)
            {
                _lastMessageTimestamp = messageTimestamp;
                IOCPService.Instance.OnMessageReceived(sangoNetMessage);
            }
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
            SendData(message);
        }

        private void SendData(SangoNetMessage message)
        {
            byte[] bytes = ProtoUtils.SetProtoBytes(message);
            Send(bytes);
        }
    }
}
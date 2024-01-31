using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_NetOperation;

namespace SangoUtils_Unity_App.Operation
{
    public class OperationKeyBroadcast : BaseNetBroadcast
    {
        private OperationKeyReqMessage _message = new();

        public void SetAndSendOperationKeyReqMessage(string entityID, OperationKeyType operationKeyType, string operationString, uint roomID = 0)
        {
            OperationKey operationKey = new(entityID, operationKeyType, operationString);
            _message.OperationKey = operationKey;
            _message.RoomID = roomID;
            DefaultOperationBroadcast();
        }

        public override void OnBroadcast(string message)
        {
            OperationKeyReqMessage reqMessage = FromJson<OperationKeyReqMessage>(message);
            if (reqMessage != null)
            {
                switch (reqMessage.OperationKey.OperationKeyType)
                {
                    case OperationKeyType.Move:
                        SystemService.Instance.OperationKeyMoveSystem.OnMessageReceived(reqMessage.OperationKey);
                        break;
                }
            }
        }

        public override void DefaultOperationBroadcast()
        {
            string jsonString = ToJson(_message);
            WebSocketService.Instance.SendOperationBroadcast(NetOperationCode, jsonString);
        }
    }
}
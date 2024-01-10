using SangoUtils_Common.Messages;

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
        SangoLogger.Processing("OperationKeyMessage Received: " + message);
        OperationKeyReqMessage reqMessage = DeJsonString<OperationKeyReqMessage>(message);
        if (reqMessage != null )
        {
            switch (reqMessage.OperationKey.OperationKeyType)
            {
                case OperationKeyType.Move:
                    OperationKeyMoveSystem.Instance.OnMessageReceived(reqMessage);
                    break;
            }
        }
    }

    public override void DefaultOperationBroadcast()
    {
        string jsonString = SetJsonString(_message);
        WebSocketService.Instance.SendOperationBroadcast(NetOperationCode, jsonString);
    }
}

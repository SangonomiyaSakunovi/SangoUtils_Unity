using SangoNetProtol;
using SangoUtils_Common.Messages;

public class OperationKeyCoreSystem : BaseSystem<OperationKeyCoreSystem>
{
    private OperationKeyBroadcast _operationKeyBroadcast;

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _operationKeyBroadcast = WebSocketService.Instance.GetNetBroadcast<OperationKeyBroadcast>(NetOperationCode.OperationKey);
    }

    public void SetAndSendOperationKey(OperationKeyType operationKeyType, string operationString)
    {
        _operationKeyBroadcast.SetAndSendOperationKeyReqMessage(CacheService.Instance.EntityID, operationKeyType, operationString, CacheService.Instance.RoomID);
    }
}

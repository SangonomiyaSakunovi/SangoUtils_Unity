using SangoUtils_Common.Infos;
using SangoUtils_Common.Messages;
using UnityEngine;

public class OperationKeyMoveSystem : OperationKeyBaseSystem<OperationKeyMoveSystem>
{
    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        OperationKeyType = OperationKeyType.Move;
    }

    public override void OnMessageReceived(OperationKeyReqMessage reqMessage)
    {

    }

    public void AddOperationMove(Vector3 position)
    {
        Vector3Info vector3Info = new(position.x, position.y, position.z);
        string vector3InfoJson = SetJsonString(vector3Info);
        SetAndSendOperationKey(vector3InfoJson);
    }
}

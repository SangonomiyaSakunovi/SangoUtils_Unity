using SangoUtils_Common.Infos;
using SangoUtils_Common.Messages;
using UnityEngine;

public class OperationKeyMoveSystem : OperationKeyBaseSystem<OperationKeyMoveSystem>
{
    public OperationKeyMoveSystem()
    {
        OperationKeyType = OperationKeyType.Move;
    }

    public override void OnMessageReceived(OperationKey operationKey)
    {
        Vector3Info vector3Info = DeJsonString<Vector3Info>(operationKey.OperationString);
        if (vector3Info != null)
        {
            TransformData transformData = new();
            transformData.Position = new(vector3Info.X, vector3Info.Y, vector3Info.Z);
            CacheService.Instance.EntityCache.AddEntityMoveKeyOnline(operationKey.EntityID, transformData);
        }
    }

    public void AddOperationMove(Vector3 position)
    {
        Vector3Info vector3Info = new(position.x, position.y, position.z);
        string vector3InfoJson = SetJsonString(vector3Info);
        SetAndSendOperationKey(vector3InfoJson);
    }
}

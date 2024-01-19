using SangoUtils_Common.Messages;
using SangoUtils_FixedNum;

namespace SangoUtils_Unity_App.Operation
{
    public class OperationKeyMoveSystem : OperationKeyBaseSystem<OperationKeyMoveSystem>
    {
        public OperationKeyMoveSystem()
        {
            OperationKeyType = OperationKeyType.Move;
        }

        public override void OnMessageReceived(OperationKey operationKey)
        {
            Vector3Message vector3Message = DeJsonString<Vector3Message>(operationKey.OperationString);
            if (vector3Message != null)
            {
                FixedInt x = FixedInt.ZERO;
                x.ScaledValue = vector3Message.X;
                FixedInt z = FixedInt.ZERO;
                z.ScaledValue = vector3Message.Z;
                FixedVector3 logicDirection = new(x, 0, z);
                CacheService.Instance.EntityCache.AddEntityMoveKeyOnline(operationKey.EntityID, logicDirection);
            }
        }

        public void AddOperationMove(FixedVector3 logicDirection)
        {
            Vector3Message vector3Message = new(logicDirection.X.ScaledValue, logicDirection.Y.ScaledValue, logicDirection.Z.ScaledValue);
            string vector3InfoJson = SetJsonString(vector3Message);
            SetAndSendOperationKey(vector3InfoJson);
        }
    }
}
using SangoNetProtol;
using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Bases_Universal;

namespace SangoUtils_Unity_App.Operation
{
    public class OperationKeyCoreSystem : BaseSystem<OperationKeyCoreSystem>
    {
        private OperationKeyBroadcast _operationKeyBroadcast;

        public OperationKeyCoreSystem() 
        {
            _operationKeyBroadcast = WebSocketService.Instance.GetNetBroadcast<OperationKeyBroadcast>(NetOperationCode.OperationKey);
        }

        public override void OnAwake()
        {
            
        }

        public override void OnDispose()
        {
            
        }

        public override void OnInit()
        {
            
        }

        public void SetAndSendOperationKey(OperationKeyType operationKeyType, string operationString)
        {
            _operationKeyBroadcast.SetAndSendOperationKeyReqMessage(CacheService.Instance.EntityCache.PlayerEntity_This.EntityID, operationKeyType, operationString, CacheService.Instance.RoomID);
        }

        protected override void OnUpdate()
        {
            
        }
    }
}
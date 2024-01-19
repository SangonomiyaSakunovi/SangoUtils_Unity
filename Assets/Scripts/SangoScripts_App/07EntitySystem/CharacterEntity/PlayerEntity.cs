using SangoUtils_Unity_App.Controller;
using SangoUtils_FixedNum;
using SangoUtils_Physics;

namespace SangoUtils_Unity_App.Entity
{
    public class PlayerEntity : BaseObjectEntity
    {
        public PlayerEntity(string entityID, TransformData transformData, PlayerState playerState) : base(entityID, transformData, playerState)
        {
            //FixedCylinderCollider = new();
        }

        private PlayerController _controller;
        public FixedCylinderCollider FixedCylinderCollider { get; set; }

        public void SetEntityToController()
        {
            if (_controller == null)
            {
                _controller.SetPlayerEntity(this);
            }
        }

        public void SendMoveKey(FixedVector3 logicDirection)
        {
            SystemRoot.Instance.OperationKeyMoveSystem.AddOperationMove(logicDirection);
        }

        public void CalcMoveResult(FixedVector3 logicDirection)
        {
            if (logicDirection != FixedVector3.Zero)
            {
                FixedVector3 logicPostionResult = LogicPosition + logicDirection * 1;
                SendMoveKey(logicPostionResult);
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            //if (LogicPosition != LogicPositionLast)
            //{
            //    LogicPositionLast = LogicPosition;
            //    SendMoveKey(LogicPosition);
            //}
        }

        private void TickMoveResult()
        {
            if (LogicDirection != FixedVector3.Zero)
            {
                LogicPositionLast = LogicPosition;
                LogicPosition += LogicDirection * 2 * (FixedInt)0.02f;
            }
        }
    }
}
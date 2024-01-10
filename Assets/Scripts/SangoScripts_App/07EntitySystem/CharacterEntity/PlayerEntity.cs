public class PlayerEntity : BaseObjectEntity

{
    public PlayerEntity(string entityID, TransformData transformData, PlayerState playerState) : base(entityID, transformData, playerState) { }

    private PlayerController _controller;

    public void SetEntityToController()
    {
        if (_controller == null)
        {
            _controller.SetPlayerEntity(this);
        }
    }

    public void SendMoveKey(TransformData transformData)
    {
        OperationKeyMoveSystem.Instance.AddOperationMove(transformData.Position);
    }
}



public abstract class BaseObjectEntity
{
    public BaseObjectEntity(string entityID, TransformData transformData, PlayerState playerState)
    {
        EntityID = entityID;
        PlayerState = playerState;
    }

    public string EntityID { get; private set; }
    public TransformData MoveKeyTransformData { get; set; }
    public TransformData TransformData { get; set; }
    public PlayerState PlayerState { get; set; }
}

public enum PlayerState
{
    None = 0,
    Online = 1,
    Offline = 2,
}

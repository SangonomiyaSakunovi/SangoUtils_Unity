using System.Collections.Generic;

public class EntityCache : BaseCache
{
    public PlayerEntity PlayerEntity_This { get; private set; }

    private Dictionary<string, PlayerEntity> _playerEntitysOnline = new();

    public PlayerEntity AddEntityLocal(string entityID)
    {
        TransformData transformData = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1));
        PlayerEntity_This = new(entityID, transformData, PlayerState.Online);
        return PlayerEntity_This;
    }

    public PlayerEntity AddEntityOnline(string entityID)
    {
        TransformData transformData = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1));
        PlayerEntity entity = new(entityID, transformData, PlayerState.Online);
        _playerEntitysOnline.Add(entity.EntityID, entity);
        return entity;
    }

    public void RemoveEntityLocal()
    {

    }

    public void RemoveEntityOnline()
    {

    }

    public void AddEntityMoveKeyOnline(string entityID, TransformData transformData)
    {
        if (entityID != PlayerEntity_This.EntityID)
        {
            if (_playerEntitysOnline.TryGetValue(entityID, out var entity))
            {
                entity.MoveKeyTransformData = transformData;
            }
            else
            {
                entity = AddEntityOnline(entityID);
                entity.MoveKeyTransformData = transformData;
            }
        }
    }
}

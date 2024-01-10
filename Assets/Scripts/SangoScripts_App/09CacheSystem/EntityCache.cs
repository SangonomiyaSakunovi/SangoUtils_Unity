using System.Collections.Generic;

public class EntityCache : BaseCache
{
    public PlayerEntity PlayerEntity_This { get; set; }

    public Dictionary<string, PlayerEntity> PlayerEntitysOnline { get; set; } = new();
}

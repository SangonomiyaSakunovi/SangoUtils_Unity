public class CacheService : BaseService<CacheService>
{
    private EntityCache _entityCache;

    public override void OnInit()
    {
        base.OnInit();
        _entityCache = new EntityCache();
    }

    public uint RoomID { get; set; } = 0;

    public PlayerEntity PlayerEntityThis { get => _entityCache.PlayerEntity_This; set => _entityCache.PlayerEntity_This = value; }



}

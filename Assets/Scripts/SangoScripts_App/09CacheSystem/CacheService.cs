public class CacheService : BaseService<CacheService>
{
    private EntityCache _entityCache;

    public override void OnInit()
    {
        base.OnInit();
        _entityCache = new EntityCache();
    }

    public string EntityID { get => _entityCache.EntityID_This; set => _entityCache.EntityID_This = value; }

    public uint RoomID { get => _entityCache.RoomID; set => _entityCache.RoomID = value; }
}

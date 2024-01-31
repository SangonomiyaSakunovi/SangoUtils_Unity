using SangoUtils_Bases_UnityEngine;
using SangoUtils_Unity_App.Cache;

public class CacheService : BaseService<CacheService>
{
    public EntityCache EntityCache { get; set; } = new();

    public override void OnInit()
    {
        EntityCache = new EntityCache();
    }

    protected override void OnUpdate()
    {
    }

    public override void OnDispose()
    {
    }

    public uint RoomID { get; set; } = 0;
}

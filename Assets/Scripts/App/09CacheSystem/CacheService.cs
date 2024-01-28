using SangoUtils_Bases_UnityEngine;
using SangoUtils_Unity_App.Cache;

public class CacheService : BaseService<CacheService>
{
    public EntityCache EntityCache { get; set; } = new();

    public override void OnInit()
    {
        base.OnInit();
        EntityCache = new EntityCache();
    }

    public uint RoomID { get; set; } = 0;
}
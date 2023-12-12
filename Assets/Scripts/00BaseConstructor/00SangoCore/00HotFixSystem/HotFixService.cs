using YooAsset;

public class HotFixService : BaseService<HotFixService>
{
    private HotFixConfig _currentFixConfig;

    public override void OnInit()
    {
        base.OnInit();
        _currentFixConfig = SangoSystemConfig.HotFixConfig;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

}

public class HotFixConfig
{
    public string dDNServerAddress;

    public string assetsPackageName;
    public EPlayMode ePlayMode;
    public EDefaultBuildPipeline eDefaultBuildPipeline;
}

public class SangoGameRoot : BaseRoot<SangoGameRoot>
{
    public SceneMainInstance SceneMainInstance;

    private void Awake()
    {
        OnInit();
        SceneMainInstance.OnInit();
        DontDestroyOnLoad(this);
    }

    public override void OnInit()
    {
        base.OnInit();
        SetConfig();
        SangoLogger.InitLogger(SangoSystemConfig.LoggerConfig_Sango);
        ResourceService.Instance.OnInit();
        AssetService.Instance.OnInit();
        EventService.Instance.OnInit();
        SceneService.Instance.OnInit();
        PatchService.Instance.OnInit();
        SecurityCheckService.Instance.OnInit();
        InitNet();
    }

    private void SetConfig()
    {
        PatchService.Instance.SetConfig(SangoSystemConfig.PatchConfig);
        NetService.Instance.SetConfig(SangoSystemConfig.NetEnvironmentConfig);
        SceneService.Instance.SetConfig(SangoSystemConfig.SceneViewConfig);
    }

    private void InitNet()
    {
        switch (SangoSystemConfig.NetEnvironmentConfig.netEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                NetService.Instance.OnInit();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        switch (SangoSystemConfig.NetEnvironmentConfig.netEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                NetService.Instance.CloseClientInstance();
                break;
        }
    }
}

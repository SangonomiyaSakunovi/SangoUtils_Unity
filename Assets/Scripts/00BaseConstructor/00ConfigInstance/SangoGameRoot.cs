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
        SangoLogger.InitLogger(SangoSystemConfig.LoggerConfig_Sango);
        ResourceService.Instance.OnInit();
        AssetService.Instance.OnInit();
        EventService.Instance.OnInit();

        SceneService.Instance.OnInit();
    }
}

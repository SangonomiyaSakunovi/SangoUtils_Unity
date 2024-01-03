using UnityEngine;

public class SangoGameRoot : BaseRoot<SangoGameRoot>
{
    public SceneMainInstance SceneMainInstance;

    private Transform _systemRootTrans;

    private LoginSystem _loginSystem;

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
        InitService();
        InitNet();
        InitSystem();
    }

    private void InitService()
    {
        ResourceService.Instance.OnInit();
        AssetService.Instance.OnInit();
        EventService.Instance.OnInit();
        SceneService.Instance.OnInit();
        PatchService.Instance.OnInit();
        SecurityCheckService.Instance.OnInit();
    }

    private void InitSystem()
    {
        _systemRootTrans = transform.Find("SystemRoot");
        _systemRootTrans.GetComponent<AOISystem>().OnInit();
        _systemRootTrans.GetComponent<LoginSystem>().OnInit();
    }

    private void SetConfig()
    {
        PatchService.Instance.SetConfig(SangoSystemConfig.PatchConfig);
        NetService.Instance.SetConfig(SangoSystemConfig.NetEnvironmentConfig);
        SceneService.Instance.SetConfig(SangoSystemConfig.SceneViewConfig);
    }

    private void InitNet()
    {
        switch (SangoSystemConfig.NetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                NetService.Instance.OnInit();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        switch (SangoSystemConfig.NetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                NetService.Instance.CloseClientInstance();
                break;
        }
    }
}

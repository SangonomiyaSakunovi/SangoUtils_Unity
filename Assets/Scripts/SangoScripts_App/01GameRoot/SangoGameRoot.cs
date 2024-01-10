using SangoNetProtol;
using UnityEngine;

public class SangoGameRoot : BaseRoot<SangoGameRoot>
{
    public SceneMainInstance SceneMainInstance;

    private Transform _systemRootTrans;

    private LoginSystem _loginSystem;
    private OperationKeyCoreSystem _operationKeyCoreSystem;

    private void Awake()
    {
        Physics.autoSyncTransforms = true;

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
        CacheService.Instance.OnInit();
        SecurityCheckService.Instance.OnInit();
    }

    private void InitSystem()
    {
        _systemRootTrans = transform.Find("SystemRoot");
        //_systemRootTrans.GetComponent<AOISystem>().OnInit();
        _systemRootTrans.GetComponent<LoginSystem>().OnInit();
        _systemRootTrans.GetComponent<OperationKeyCoreSystem>().OnInit();
        _systemRootTrans.GetComponent<OperationKeyMoveSystem>().OnInit();
    }

    private void SetConfig()
    {
        PatchService.Instance.SetConfig(SangoSystemConfig.PatchConfig);
        SceneService.Instance.SetConfig(SangoSystemConfig.SceneViewConfig);
    }

    private void InitNet()
    {
        switch (SangoSystemConfig.NetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                IOCPService.Instance.SetConfig(SangoSystemConfig.NetEnvironmentConfig);
                IOCPService.Instance.OnInit();
                break;
            case NetEnvMode.Online_WebSocket:
                WebSocketService.Instance.GetNetEvent<PingWebSocketEvent>(NetOperationCode.Ping);
                WebSocketService.Instance.SetConfig(SangoSystemConfig.NetEnvironmentConfig);
                WebSocketService.Instance.OnInit();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        switch (SangoSystemConfig.NetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                IOCPService.Instance.CloseClientInstance();
                break;
            case NetEnvMode.Online_WebSocket:
                WebSocketService.Instance.CloseClientInstance();
                break;
        }
    }
}

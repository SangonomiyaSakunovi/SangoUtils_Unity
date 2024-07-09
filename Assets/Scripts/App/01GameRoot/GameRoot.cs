using SangoUtils.Patchs_YooAsset;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_Event;
using SangoUtils_Logger;
using SangoUtils_Unity_App.Scene;
using SangoUtils_Unity_Scripts.Net;
using UnityEngine;

public class GameRoot : BaseRoot<GameRoot>
{
    public SceneMainInstance SceneMainInstance;

    private void Awake()
    {
        OnInit();
        DontDestroyOnLoad(this);
    }

    public override void OnInit()
    {
        InitConfig();
        InitUtils();
        InitEnv();
        InitService();
        InitNet();
        InitSystem();

        StartGame();
    }

    protected override void OnUpdate()
    {
        EventService.Instance.OnUpdate();
    }

    private void InitConfig()
    {
        SceneService.Instance.SetConfig(GameConfig.SceneViewConfig);
    }

    private void InitUtils()
    {
        //gameObject.AddComponent<RuntimeLogger>();
        SangoLogger.InitLogger(GameConfig.LoggerConfig_Sango);
    }

    private void InitEnv()
    {
        Physics.autoSyncTransforms = true;
    }

    private void InitService()
    {
        SangoAssetService.Instance.Initialize();
        EventService.Instance.OnInit();
        SceneService.Instance.OnInit();
        CacheService.Instance.OnInit();
        SecurityCheckService.Instance.OnInit();
    }

    private void InitNet()
    {
        switch (GameConfig.NetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                IOCPService.Instance.SetConfig(GameConfig.NetEnvironmentConfig);
                IOCPService.Instance.OpenClient();
                break;
            case NetEnvMode.Online_WebSocket:
                WebSocketService.Instance.SetConfig(GameConfig.NetEnvironmentConfig);
                WebSocketService.Instance.OpenClient();
                break;
        }
    }

    private void InitSystem()
    {
        GetComponent<WindowRoot>().OnInit();
        //SystemService systemRoot = new();
        SystemService.Instance.OnInit();
    }

    private void StartGame()
    {
        //_sangoPatchRoot.Initialize(GameConfig.PatchConfig);
        SceneMainInstance.OnInit();
        //UIService.Instance.SwitchWindow<LoginWnd>();
    }

    private void OnPatchCompleted(bool isComplete)
    {
        
    }

    private void OnApplicationQuit()
    {
        switch (GameConfig.NetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Online_IOCP:
                IOCPService.Instance.CloseClient();
                break;
            case NetEnvMode.Online_WebSocket:
                WebSocketService.Instance.CloseClient();
                break;
        }
    }

    public override void OnDispose()
    {
    }

    public override void OnAwake()
    {

    }
}

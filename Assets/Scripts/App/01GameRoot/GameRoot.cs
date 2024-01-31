using SangoNetProtol;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_Event;
using SangoUtils_Logger;
using SangoUtils_Patch_YooAsset;
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
        PatchService.Instance.SetConfig(GameConfig.PatchConfig);
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
        AssetService.Instance.OnInit();
        EventService.Instance.OnInit();
        SceneService.Instance.OnInit();
        PatchService.Instance.OnPatchCompleted = OnPatchCompleted;
        PatchService.Instance.OnInit();
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
        SceneMainInstance.OnInit();
        //UIService.Instance.SwitchWindow<LoginWnd>();
    }

    private void OnPatchCompleted(bool isComplete)
    {
        SceneSystemEventMessage.ChangeToHomeScene.SendEventMessage();
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

using SangoNetProtol;
using SangoUtils_Unity_App.Scene;
using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Unity_Scripts.Patch;
using SangoUtils_Logger;
using UnityEngine;
using SangoUtils_Bases_UnityEngine;

public class SangoGameRoot : BaseRoot<SangoGameRoot>
{
    public SceneMainInstance SceneMainInstance;

    private void Awake()
    {
        Physics.autoSyncTransforms = true;
        //gameObject.AddComponent<RuntimeLogger>();

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
        GetComponent<WindowRoot>().OnInit();
        SystemRoot systemRoot = new();
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

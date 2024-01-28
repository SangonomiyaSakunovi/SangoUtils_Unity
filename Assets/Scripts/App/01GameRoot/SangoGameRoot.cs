using SangoNetProtol;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_Event;
using SangoUtils_Logger;
using SangoUtils_Patch_YooAsset;
using SangoUtils_Unity_App.Scene;
using SangoUtils_Unity_Scripts.Net;
using UnityEngine;

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

    private void Update()
    {
        EventService.Instance.OnUpdate();
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
        AssetService.Instance.OnInit();
        EventService.Instance.OnInit();
        SceneService.Instance.OnInit();
        PatchService.Instance.OnPatchCompleted = OnPatchCompleted;
        PatchService.Instance.OnInit();
        CacheService.Instance.OnInit();
        SecurityCheckService.Instance.OnInit();
    }

    private void OnPatchCompleted(bool isComplete)
    {
        SceneSystemEventMessage.ChangeToHomeScene.SendEventMessage();
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

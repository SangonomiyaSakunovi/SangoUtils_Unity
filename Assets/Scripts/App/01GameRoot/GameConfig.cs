using SangoUtils.Patchs_YooAsset;
using SangoUtils_Logger;
using SangoUtils_Unity_Scripts.Net;
using System;
using YooAsset;

public static class GameConfig
{
    public static readonly SceneViewConfig SceneViewConfig = new()
    {
        SceneViewResolution = SceneViewResolution._1KH_1920x1080
    };

    public static readonly SecurityCheckServiceConfig SecurityCheckServiceInfoConfig = new()
    {
        ApiKey = "s",
        ApiSecret = "s",
        SecretTimestamp = "0",
        DefaultRegistLimitDateTime = new DateTime(2022, 2, 22, 0, 0, 0)
    };

    //public static readonly SangoPatchConfig PatchConfig = new()
    //{
    //    PackageName = "DefaultPackage",
    //    PlayMode = EPlayMode.HostPlayMode,
    //    BuildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline,

    //    //Protocol: hostServerIP/CDN/Editor/Unity/appId/Patch/PC/appVersion
    //    //Protocol: hostServerIP/CDN/Online/Unity/appId/Patch/PC/appVersion
    //    HostServerIP = "https://hvr.isunupcg.com/sangonomiyasakunovi",
    //    AppID = "0000TestSangoApp",
    //    AppVersion = "1.0",

    //    //OnUpdaterDone = SceneSystemEventMessage.ChangeToHomeScene.SendEventMessage()
    //};

    public static readonly LoggerConfig_Sango LoggerConfig_Sango = new()
    {
        EnableSangoLog = true,
        LogPrefix = "#",
        EnableTimestamp = true,
        LogSeparate = ">>",
        EnableThreadID = true,
        EnableTraceInfo = true,
        EnableSaveLog = true,
        EnableCoverLog = true,
        SaveLogPath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory),
        SaveLogName = "SangoLog.txt",
        LoggerType = LoggerType.OnUnityConsole
    };

    public static NetEnvironmentConfig NetEnvironmentConfig = new()
    {
        NetEnvMode = NetEnvMode.Online_WebSocket,
        ServerAddress = "127.0.0.1",
        ServerPort = 52037,
        ServerAddressAndPort = "ws://sync_game.isunupcg.com:7373",
    };
}

using SangoScripts_Unity.Net;
using SangoScripts_Unity.Patch;
using System;
using YooAsset;

public static class SangoSystemConfig
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

    public static readonly PatchConfig PatchConfig = new()
    {
        PackageName = "DefaultPackage",
        PlayMode = EPlayMode.HostPlayMode,
        BuildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline,

        //Protocol: hostServerIP/CDN/Editor/Unity/appId/Patch/PC/appVersion
        //Protocol: hostServerIP/CDN/Online/Unity/appId/Patch/PC/appVersion
        HostServerIP = "https://hvr.isunupcg.com/sangonomiyasakunovi",
        AppId = "0000TestSangoApp",
        AppVersion = "1.0"
    };

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
        LoggerType = LoggerType.OnEditorConsole
    };

    public static NetEnvironmentConfig NetEnvironmentConfig = new()
    {
        NetEnvMode = NetEnvMode.Online_WebSocket,
        ServerAddress = "127.0.0.1",
        ServerPort = 52037,
        ServerAddressAndPort = "ws://sync_game.isunupcg.com:7373",
    };
}

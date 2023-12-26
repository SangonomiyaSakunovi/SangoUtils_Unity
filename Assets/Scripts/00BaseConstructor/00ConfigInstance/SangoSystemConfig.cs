using System;
using YooAsset;

public static class SangoSystemConfig
{
    public static readonly SceneViewConfig SceneViewConfig = new()
    {
        sceneViewResolution = SceneViewResolution._1KH_1920x1080
    };

    public static readonly SecurityCheckServiceConfig SecurityCheckServiceInfoConfig = new()
    {
        apiKey = "s",
        apiSecret = "s",
        secretTimestamp = "0",
        defaultRegistLimitDateTime = new DateTime(2022, 2, 22, 0, 0, 0)
    };

    public static readonly PatchConfig PatchConfig = new()
    {
        packageName = "DefaultPackage",
        playMode = EPlayMode.HostPlayMode,
        buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline,

        //Protocol: hostServerIP/CDN/Editor/Unity/appId/Patch/PC/appVersion
        //Protocol: hostServerIP/CDN/Online/Unity/appId/Patch/PC/appVersion
        hostServerIP = "https://hvr.isunupcg.com/sangonomiyasakunovi",
        appId = "0000TestSangoApp",
        appVersion = "1.0"
    };

    public static readonly LoggerConfig_Sango LoggerConfig_Sango = new()
    {
        enableSangoLog = true,
        logPrefix = "#",
        enableTimestamp = true,
        logSeparate = ">>",
        enableThreadID = true,
        enableTraceInfo = true,
        enableSaveLog = true,
        enableCoverLog = true,
        saveLogPath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory),
        saveLogName = "SangoLog.txt",
        loggerType = LoggerType.OnEditorConsole
    };

    public static NetEnvironmentConfig NetEnvironmentConfig = new()
    {
        netEnvMode = NetEnvMode.Online_IOCP,
        serverAddress = "127.0.0.1",
        serverPort = 52037
    };
}

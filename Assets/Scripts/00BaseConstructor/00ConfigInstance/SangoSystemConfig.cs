using System;
using YooAsset;

public static class SangoSystemConfig
{
    public static readonly SceneViewConfig SceneViewConfig = new SceneViewConfig
    {
        sceneViewResolution = SceneViewResolution._1KH_1920x1080
    };

    public static readonly SecurityCheckServiceConfig SecurityCheckServiceInfoConfig = new SecurityCheckServiceConfig
    {
        apiKey = "s",
        apiSecret = "s",
        secretTimestamp = "0",
        defaultRegistLimitDateTime = new DateTime(2022, 2, 22, 0, 0, 0)
    };

    public static readonly PatchConfig PatchConfig = new PatchConfig
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

    public static readonly LoggerConfig_Sango LoggerConfig_Sango = new LoggerConfig_Sango
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
}

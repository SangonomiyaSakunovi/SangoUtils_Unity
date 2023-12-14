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
        playMode = EPlayMode.EditorSimulateMode,
        buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline,

        //Protocol: hostServerIP/CDN/Editor/Unity/appId/Patch/PC/appVersion
        //Protocol: hostServerIP/CDN/Online/Unity/appId/Patch/PC/appVersion
        hostServerIP = "http://oss-cn-beijing.aliyuncs.com/sangonomiyasakunovi",
        appId = "0000TestSangoApp",
        appVersion = "1.0"
    };
}

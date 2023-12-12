using System;
using YooAsset;

public static class SangoSystemConfig
{
    public static readonly SecurityCheckServiceConfig SecurityCheckServiceInfoConfig = new SecurityCheckServiceConfig
    {
        apiKey = "s",
        apiSecret = "s",
        secretTimestamp = "0",
        defaultRegistLimitDateTime = new DateTime(2022, 2, 22, 0, 0, 0)
    };

    public static readonly TypeInConfig SecurityCheckPanelTypeInConfig = new TypeInConfig
    {
        keyboardTypeCode = KeyboardTypeCode.UpperCharKeyboard_Vertical_4K,
        keyboradDirectionCode = KeyboradDirectionCode.Vertical
    };

    public static readonly HotFixConfig HotFixConfig = new HotFixConfig
    {
        assetsPackageName = "DefaultPackage",
        ePlayMode = EPlayMode.EditorSimulateMode,
        eDefaultBuildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline,
        dDNServerAddress = ""
    };
}

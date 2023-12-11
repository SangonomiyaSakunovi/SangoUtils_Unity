using System.Collections;
using YooAsset;

public class HotFixService : BaseService<HotFixService>
{
    private ResourceDownloaderOperation _yooAssetResourceDownloaderOperation;
    private ResourcePackage _yooAssetResourcePackage;

    private HotFixConfig _currentFixConfig;


    public override void OnInit()
    {
        base.OnInit();
        _currentFixConfig = SangoSystemConfig.HotFixConfig;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private IEnumerator PrepareAssetBundles()
    {
        YooAssets.Initialize();
        _yooAssetResourcePackage = YooAssets.TryGetPackage("DefaultPackage");
        _yooAssetResourcePackage = YooAssets.CreatePackage("DefaultPackage");
        YooAssets.SetDefaultPackage(_yooAssetResourcePackage);
        EPlayMode playMode = _currentFixConfig.ePlayMode;
        InitializationOperation initOperation = null;
        switch (playMode)
        {
            case EPlayMode.EditorSimulateMode:
                EditorSimulateModeParameters initEditorSimulateParameters = new EditorSimulateModeParameters();
                initEditorSimulateParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(_currentFixConfig.eDefaultBuildPipeline, "DefaultPackage");
                initOperation = _yooAssetResourcePackage.InitializeAsync(initEditorSimulateParameters);
                yield return initOperation;
                break;
            case EPlayMode.HostPlayMode:
                HostPlayModeParameters initHostPlayParameters = new HostPlayModeParameters();
                //initHostPlayParameters.
                initOperation = _yooAssetResourcePackage.InitializeAsync(initHostPlayParameters);
                yield return initOperation;
                //TODO GetInfoFromParameters
                break;
            case EPlayMode.OfflinePlayMode:
                OfflinePlayModeParameters initOfflinePlayParameters = new OfflinePlayModeParameters();
                initOperation = _yooAssetResourcePackage.InitializeAsync(initOfflinePlayParameters);
                yield return initOperation;
                break;
        }
    }
}

public class HotFixConfig
{
    public string dDNServerAddress;

    public EPlayMode ePlayMode;
    public EDefaultBuildPipeline eDefaultBuildPipeline;
}

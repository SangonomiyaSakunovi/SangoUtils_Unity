using System.Collections;
using YooAsset;

public class PatchService : BaseService<PatchService>
{
    private PatchConfig _currentPatchConfig;

    private CoroutineHandler coroutine = null;

    public override void OnInit()
    {
        base.OnInit();
        coroutine = StartOperation().Start();
    }

    public void SetConfig(PatchConfig patchConfig)
    {
        _currentPatchConfig = patchConfig;
    }

    private IEnumerator StartOperation()
    {
        YooAssets.Initialize();

        PatchOperation hotFixOperation = new PatchOperation(_currentPatchConfig);
        YooAssets.StartOperation(hotFixOperation);
        yield return hotFixOperation;

        ResourcePackage assetPackage = YooAssets.GetPackage(_currentPatchConfig.packageName);
        YooAssets.SetDefaultPackage(assetPackage);

        PatchSystemEventMessage.ClosePatchWindow_PatchSystemEventMessage.SendEventMessage();
        SceneSystemEventMessage.ChangeToHomeScene.SendEventMessage();
    }
}

public class PatchConfig : BaseConfig
{
    public string hostServerIP;
    public string appId;
    public string appVersion;

    public string packageName;
    public EPlayMode playMode;
    public EDefaultBuildPipeline buildPipeline;
}

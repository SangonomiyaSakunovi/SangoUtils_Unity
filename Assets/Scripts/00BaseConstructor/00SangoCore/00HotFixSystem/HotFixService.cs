using System.Collections;
using UnityEngine;
using YooAsset;

public class HotFixService : BaseService<HotFixService>
{
    private HotFixConfig _currentHotFixConfig;

    private CoroutineHandler coroutine = null;

    public override void OnInit()
    {
        base.OnInit();
        _currentHotFixConfig = SangoSystemConfig.HotFixConfig;
        coroutine = StartOperation().Start();
    }

    private IEnumerator StartOperation()
    {
        YooAssets.Initialize();

        //var go = Resources.Load<GameObject>("PatchWindow");
        //GameObject.Instantiate(go);

        HotFixOperation hotFixOperation = new HotFixOperation(_currentHotFixConfig);
        YooAssets.StartOperation(hotFixOperation);
        yield return hotFixOperation;

        ResourcePackage assetPackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(assetPackage);
        //TODO
        SangoLogger.Log("运行至更新结束位置");
    }
}

public class HotFixConfig
{
    public string dDNServerAddress;

    public string packageName;
    public EPlayMode playMode;
    public EDefaultBuildPipeline buildPipeline;
}

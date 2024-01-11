using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneMainInstance : BaseScene
{
    public static SceneMainInstance Instance { get; private set; }

    private string _capsulePath = "ResTest/Capsule";

    private Dictionary<string, GameObject> _entityDict = new();

    private Transform _canvasTrans;
    private SangoPatchRoot _patchRoot;
    private SangoSecurityCheckRoot _securityCheckRoot;
    private SceneViewConfig _currentSceneViewConfig;

    private NetEnvironmentConfig _currentNetEnvironmentConfig;

    public override void OnInit()
    {
        base.OnInit();
        Instance = this;
        _currentSceneViewConfig = SangoSystemConfig.SceneViewConfig;
        _currentNetEnvironmentConfig = SangoSystemConfig.NetEnvironmentConfig;
        SceneService.Instance.SetHandleEventMessageCallBack(OnHandleEventMessage);
        GetTrans();
        GameStart();
    }

    private void GetTrans()
    {
        _canvasTrans = transform.Find("Canvas");
        _patchRoot = _canvasTrans.Find("SangoPatchRoot").AddComponent<SangoPatchRoot>();
    }

    private void GameStart()
    {
        _patchRoot.OnInit();
    }

    public void OnHandleEventMessage(IEventMessageBase message)
    {
        if (message is SceneSystemEventMessage.ChangeToHomeScene)
        {
            GameObject securityCheckRootObj = null;
            switch (_currentSceneViewConfig.SceneViewResolution)
            {
                case SceneViewResolution._1KH_1920x1080:
                    securityCheckRootObj = ResourceService.Instance.InstantiatePrefab(_canvasTrans, SecuritySystemConstant.SangoSecurityCheckRootPrefab_1KH_Path);
                    break;
            }
            _securityCheckRoot = securityCheckRootObj.GetOrAddComponent<SangoSecurityCheckRoot>();
            _securityCheckRoot.OnInit();
        }
        else if (message is SceneSystemEventMessage.ChangeToBattleScene)
        {

        }
    }

    public void GameEntrance()
    {
        switch (_currentNetEnvironmentConfig.NetEnvMode)
        {
            case NetEnvMode.Offline:
                //TODO
                break;
            case NetEnvMode.Online_IOCP:
                break;
        }
    }
}

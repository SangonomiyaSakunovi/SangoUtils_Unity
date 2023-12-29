using Unity.VisualScripting;
using UnityEngine;

public class SceneMainInstance : BaseScene<SceneMainInstance>
{
    public LoginSystem _loginSystem;
    
    private Transform _canvasTrans;
    private SangoPatchRoot _patchRoot;
    private SangoSecurityCheckRoot _securityCheckRoot;
    private SceneViewConfig _currentSceneViewConfig;

    private NetEnvironmentConfig _currentNetEnvironmentConfig;

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _currentSceneViewConfig = SangoSystemConfig.SceneViewConfig;
        _currentNetEnvironmentConfig = SangoSystemConfig.NetEnvironmentConfig;
        SceneService.Instance.SetHandleEventMessageCallBack(OnHandleEventMessage);
        GetTrans();
        GameStart();
    }

    private void GetTrans()
    {
        _canvasTrans = transform.Find("Canvas");
        _patchRoot = _canvasTrans.Find("SangoPatchRoot").GetOrAddComponent<SangoPatchRoot>();
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
            switch (_currentSceneViewConfig.sceneViewResolution)
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
        switch (_currentNetEnvironmentConfig.netEnvMode)
        {
            case NetEnvMode.Offline:
                //TODO
                break;
            case NetEnvMode.Online_IOCP:
                _loginSystem.OnInit();
                break;
        }        
    }
}

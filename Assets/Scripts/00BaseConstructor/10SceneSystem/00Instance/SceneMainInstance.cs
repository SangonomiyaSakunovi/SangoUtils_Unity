using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneMainInstance : BaseScene<SceneMainInstance>
{
    private Transform _canvasTrans;
    private SangoPatchRoot _patchRoot;
    private SangoSecurityCheckRoot _securityCheckRoot;
    private SceneViewConfig _currentSceneViewConfig;

    public override void OnInit()
    {
        base.OnInit();
        SetInstance(this);
        _currentSceneViewConfig = SangoSystemConfig.SceneViewConfig;
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
                    securityCheckRootObj = InstantiateGameObject(_canvasTrans, SecuritySystemConstant.SangoSecurityCheckRootPrefab_1KH_Path);
                    break;
            }
            _securityCheckRoot = securityCheckRootObj.GetOrAddComponent<SangoSecurityCheckRoot>();
            _securityCheckRoot.OnInit();
        }
        else if (message is SceneSystemEventMessage.ChangeToBattleScene)
        {

        }
    }
}

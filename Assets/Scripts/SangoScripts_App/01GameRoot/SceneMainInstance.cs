using SangoUtils_Common.Infos;
using SangoUtils_Common.Messages;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneMainInstance : BaseScene<SceneMainInstance>
{
    private string _capsulePath = "ResTest/Capsule";
    public Transform _parent;

    private Dictionary<string, GameObject> _entityDict = new();

    public LoginSystem _loginSystem;

    private Transform _canvasTrans;
    private SangoPatchRoot _patchRoot;
    private SangoSecurityCheckRoot _securityCheckRoot;
    private SceneViewConfig _currentSceneViewConfig;

    private NetEnvironmentConfig _currentNetEnvironmentConfig;

    public string _currentEntityID { get; set; } = "";

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
                _loginSystem.OnInit();
                break;
        }
    }

    public void OnAOIOperationEvent(AOIEventMessage eventMessage)
    {
        if (eventMessage.AOIViewEnterEntitys.Count > 0)
        {
            for (int i = 0; i < eventMessage.AOIViewEnterEntitys.Count; i++)
            {
                string enterEntityID = eventMessage.AOIViewEnterEntitys[i].EntityID;
                if (enterEntityID != _currentEntityID)
                {
                    
                }

                TransformInfo enterTransformInfo = eventMessage.AOIViewEnterEntitys[i].TransformInfo;
                if (_entityDict.TryGetValue(enterEntityID, out GameObject obj))
                {
                    _entityDict.Remove(enterEntityID);
                    Destroy(obj);
                }

                Vector3 positionNew = new(enterTransformInfo.Position.X, enterTransformInfo.Position.Y, enterTransformInfo.Position.Z);
                Quaternion rotationNew = new(enterTransformInfo.Rotation.X, enterTransformInfo.Rotation.Y, enterTransformInfo.Rotation.Z, enterTransformInfo.Rotation.W);

                GameObject prefab = ResourceService.Instance.LoadPrefab(_capsulePath, true);
                GameObject gameObject = Instantiate(prefab, positionNew, rotationNew, _parent);

                gameObject.name = enterEntityID;
                GameObjectCharacterController controller = gameObject.GetComponent<GameObjectCharacterController>();
                controller.IsCurrent = false;
                controller.IsLerp = false;
                controller.PositionTarget = positionNew;

                //gameObject.transform.rotation = new(enterTransformInfo.Rotation.X, enterTransformInfo.Rotation.Y, enterTransformInfo.Rotation.Z, enterTransformInfo.Rotation.W);
                //gameObject.transform.localScale = new(enterTransformInfo.Scale.X, enterTransformInfo.Scale.Y, enterTransformInfo.Scale.Z);

                _entityDict.Add(enterEntityID, gameObject);
            }
        }
        if (eventMessage.AOIViewMoveEntitys.Count > 0)
        {
            for (int j = 0; j < eventMessage.AOIViewMoveEntitys.Count; j++)
            {
                string moveEntityID = eventMessage.AOIViewMoveEntitys[j].EntityID;
                if (moveEntityID != _currentEntityID)
                {
                    

                }

                TransformInfo moveTransformInfo = eventMessage.AOIViewMoveEntitys[j].TransformInfo;

                Vector3 positionNew = new(moveTransformInfo.Position.X, moveTransformInfo.Position.Y, moveTransformInfo.Position.Z);
                Quaternion rotationNew = new(moveTransformInfo.Rotation.X, moveTransformInfo.Rotation.Y, moveTransformInfo.Rotation.Z, moveTransformInfo.Rotation.W);
                Vector3 scaleNew = new(moveTransformInfo.Scale.X, moveTransformInfo.Scale.Y, moveTransformInfo.Scale.Z);

                GameObjectCharacterController controller;

                if (!_entityDict.TryGetValue(moveEntityID, out GameObject gameObject))
                {
                    GameObject prefab = ResourceService.Instance.LoadPrefab(_capsulePath, true);
                    gameObject = Instantiate(prefab, positionNew, rotationNew, _parent);
                    gameObject.name = moveEntityID;
                    controller = gameObject.GetComponent<GameObjectCharacterController>();
                    controller.IsCurrent = false;
                    controller.IsLerp = false;
                    controller.PositionTarget = positionNew;

                    _entityDict.Add(moveEntityID, gameObject);
                }
                else
                {
                    controller = gameObject.GetComponent<GameObjectCharacterController>();
                    controller.IsLerp = true;
                    controller.PositionTarget = positionNew;
                }
            }
        }
        if (eventMessage.AOIViewExitEntitys.Count > 0)
        {
            for (int k = 0; k < eventMessage.AOIViewExitEntitys.Count; k++)
            {
                string exitEntityID = eventMessage.AOIViewExitEntitys[k].EntityID;
                if (exitEntityID != _currentEntityID)
                {
                    if (_entityDict.TryGetValue(exitEntityID, out GameObject gameObject))
                    {
                        _entityDict.Remove(exitEntityID);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}

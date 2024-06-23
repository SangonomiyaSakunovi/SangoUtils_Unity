using SangoUtils.Patchs_YooAsset;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_Event;
using SangoUtils_Extensions_UnityEngine.Service;
using SangoUtils_Unity_App.Controller;
using SangoUtils_Unity_App.Entity;
using SangoUtils_Unity_Scripts.Net;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SangoUtils_Unity_App.Scene
{
    public class SceneMainInstance : BaseScene
    {
        public static SceneMainInstance Instance { get; private set; }

        //private string _capsulePath = "ResTest/Capsule"; 

        private Dictionary<string, GameObject> _entityDict = new();

        private Transform _canvasTrans;
        private SangoSecurityCheckRoot _securityCheckRoot;
        private SceneViewConfig _currentSceneViewConfig;
        private Transform _3DSceneAOITest;

        private NetEnvironmentConfig _currentNetEnvironmentConfig;

        public override void OnInit()
        {
            Instance = this;
            _currentSceneViewConfig = GameConfig.SceneViewConfig;
            _currentNetEnvironmentConfig = GameConfig.NetEnvironmentConfig;
            SceneService.Instance.SetHandleEventMessageCallBack(OnHandleEventMessage);
            GetTrans();
        }

        private void GetTrans()
        {
            _canvasTrans = transform.Find("Canvas");
            _3DSceneAOITest = transform.Find("3DSceneAOITest");
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

        public void AddNewOnlineCapsule(PlayerEntity entity)
        {
            GameObject capsule = Resources.Load<GameObject>("ResTest/Capsule");
            GameObject gameObject = Instantiate(capsule, _3DSceneAOITest);
            gameObject.GetComponent<PlayerController>().SetPlayerEntity(entity);
        }
    }
}
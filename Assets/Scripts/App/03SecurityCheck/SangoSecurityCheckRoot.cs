using SangoUtils_Unity_App.Scene;
using SangoUtils_Logger;
using Unity.VisualScripting;
using UnityEngine;
using SangoUtils_Bases_UnityEngine;
using SangoUtils_Extensions_UnityEngine.Service;

public class SangoSecurityCheckRoot : BaseRoot<SangoSecurityCheckRoot>
{
    private SangoSecurityCheckWnd _sangoSecurityCheckWnd;

    private TypeInConfig _currentTypeInConfig;

    public override void OnInit()
    {
        base.OnInit();
        _sangoSecurityCheckWnd = transform.Find("SangoSecurityCheckWnd").GetOrAddComponent<SangoSecurityCheckWnd>();

        SecurityCheckServiceConfig config1 = SangoSystemConfig.SecurityCheckServiceInfoConfig;
        config1.RegistInfoCode = RegistInfoCode.Timestamp;
        config1.SignMethodCode = SignMethodCode.Md5;
        config1.CheckLength = 5;
        config1.RegistMixSignDataProtocol = RegistMixSignDataProtocol.A_B_C_SIGN;
        config1.OnCheckedResult = RegistInfoCheckResultActionCallBack;

        TypeInConfig config2 = new TypeInConfig();
        SceneViewConfig sceneViewConfig = SangoSystemConfig.SceneViewConfig;
        switch (sceneViewConfig.SceneViewResolution)
        {
            case SceneViewResolution._1KH_1920x1080:
                config2.keyboardTypeCode = KeyboardTypeCode.UpperCharKeyboard;
                config2.keyboradDirectionCode = KeyboradDirectionCode.Horizontal;
                break;
            case SceneViewResolution._4KH_3840x2160:
                config2.keyboardTypeCode = KeyboardTypeCode.UpperCharKeyboard_4K;
                config2.keyboradDirectionCode = KeyboradDirectionCode.Horizontal;
                break;
            case SceneViewResolution._4kV_2160x3840:
                config2.keyboardTypeCode = KeyboardTypeCode.UpperCharKeyboard_Vertical_4K;
                config2.keyboradDirectionCode = KeyboradDirectionCode.Vertical;
                break;
        }
        config2.typeInLanguage = TypeInLanguage.English;
        _currentTypeInConfig = config2;
        InitSecurityCheckService(config1); 
        CheckRegistValidation();
    }

    private void CheckRegistValidation()
    {
        SecurityCheckService.Instance.CheckRegistValidation();
    }

    public void UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        SecurityCheckService.Instance.UpdateRegistInfo(registLimitTimestampNew, signData);
    }

    public void UpdateRegistInfo(string mixSignData)
    {
        SecurityCheckService.Instance.UpdateRegistInfo(mixSignData);
    }

    private void InitSecurityCheckService(SecurityCheckServiceConfig config)
    {
        SecurityCheckService.Instance.InitService(config);
    }

    private void RegistInfoCheckResultActionCallBack(RegistInfoCheckResult result, string commands)
    {
        switch (result)
        {
            case RegistInfoCheckResult.CheckOK_Valid:
                OnSecurityCheckResultValid();
                break;
            case RegistInfoCheckResult.CheckOK_FirstRun:
                CheckRegistValidation();
                break;
            case RegistInfoCheckResult.CheckWarnning_ValidationLessThan3Days:
                _sangoSecurityCheckWnd.SetRoot(this);
                _sangoSecurityCheckWnd.SetInfo(_currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("�������" + commands + "�쵽�ڣ��뼰ʱ��ϵ������");
                _sangoSecurityCheckWnd.UpdateBtnInfo("SkipBtn", "");
                break;
            case RegistInfoCheckResult.CheckFailed_OutData:
                _sangoSecurityCheckWnd.SetRoot(this);
                _sangoSecurityCheckWnd.SetInfo(_currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("����ѹ��ڣ�������ע�����������");
                break;
            case RegistInfoCheckResult.CheckError_SystemTimeChanged:
                _sangoSecurityCheckWnd.SetRoot(this);
                _sangoSecurityCheckWnd.SetInfo(_currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("ע����Ϣ���ʧ�ܣ�ϵͳʱ�䱻�޸�");
                break;
            case RegistInfoCheckResult.UpdateOK_Success:
                _sangoSecurityCheckWnd.UpdateResult("�ɹ���������������" + commands);
                _sangoSecurityCheckWnd.UpdateBtnInfo("RegistBtn", "ȷ��");
                break;
            case RegistInfoCheckResult.UpdateFailed_OutData:
                _sangoSecurityCheckWnd.UpdateResult("����ʧ�ܣ���Կ�ѹ���");
                break;
            case RegistInfoCheckResult.UpdateFailed_SignError:
                _sangoSecurityCheckWnd.UpdateResult("���������������������ϵ������");
                break;
            case RegistInfoCheckResult.UpdateFailed_WriteInfoError:
                _sangoSecurityCheckWnd.UpdateResult("д��ע����Ϣʧ�ܣ�������");
                break;
            case RegistInfoCheckResult.UpdateError_NullInfo:
                _sangoSecurityCheckWnd.UpdateResult("���������������ݲ���Ϊ��");
                break;
            case RegistInfoCheckResult.UpdateError_SyntexError:
                _sangoSecurityCheckWnd.UpdateResult("���������������������ϵ������");
                break;
            case RegistInfoCheckResult.UpdateError_LenghthError:
                _sangoSecurityCheckWnd.UpdateResult("����������������ݳ��Ȳ���ȷ");
                break;
        }
    }

    public void OnSecurityCheckResultValid()
    {
        _sangoSecurityCheckWnd.SetWindowState(false);
        SceneMainInstance.Instance.GameEntrance();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PersistDataService.Instance.RemovePersistData("key1");
            PersistDataService.Instance.RemovePersistData("key2");
            SangoLogger.Done("���ֶ�ɾ��ע����Ϣ");
        }
    }
}

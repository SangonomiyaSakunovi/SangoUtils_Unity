using System;
using UnityEngine;

public class SangoSecurityCheckRoot : BaseRoot<SangoSecurityCheckRoot>
{
    public SangoSecurityCheckWnd _sangoSecurityCheckWnd;

    private TypeInConfig _currentTypeInConfig;

    private void Start()
    {
        SecurityCheckServiceConfig config1 = SangoSystemConfig.SecurityCheckServiceInfoConfig;
        config1.registInfoCode = RegistInfoCode.Timestamp;
        config1.signMethodCode = SignMethodCode.Md5;
        config1.checkLength = 5;
        config1.registMixSignDataProtocol = RegistMixSignDataProtocol.A_B_C_SIGN;
        config1.resultActionCallBack = RegistInfoCheckResultActionCallBack;

        TypeInConfig config2 = SangoSystemConfig.SecurityCheckPanelTypeInConfig;
        config2.typeInLanguage = TypeInLanguage.English;

        _currentTypeInConfig = config2;
        InitSecurityCheckService(config1);
        CheckRegistValidation();
    }

    private void RegistInfoCheckResultActionCallBack(RegistInfoCheckResult result, string commands)
    {
        Debug.Log("��ʼ���лص�");
        switch (result)
        {
            case RegistInfoCheckResult.CheckOK_Valid:
                OnSecurityCheckResultValid();
                break;
            case RegistInfoCheckResult.CheckOK_FirstRun:
                CheckRegistValidation();
                break;
            case RegistInfoCheckResult.CheckWarnning_ValidationLessThan3Days:
                _sangoSecurityCheckWnd.SetRoot(this, _currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("�������" + commands + "�쵽�ڣ��뼰ʱ��ϵ������");
                _sangoSecurityCheckWnd.UpdateBtnInfo("SkipBtn", "");
                break;
            case RegistInfoCheckResult.CheckFailed_OutData:
                _sangoSecurityCheckWnd.SetRoot(this, _currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("����ѹ��ڣ�������ע�����������");
                break;
            case RegistInfoCheckResult.CheckError_SystemTimeChanged:
                _sangoSecurityCheckWnd.SetRoot(this, _currentTypeInConfig);
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PersistDataService.Instance.RemovePersistData("key1");
            PersistDataService.Instance.RemovePersistData("key2");
            Debug.Log("���ֶ�ɾ��ע����Ϣ");
        }
    }
}

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
        Debug.Log("开始运行回调");
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
                _sangoSecurityCheckWnd.UpdateResult("软件还有" + commands + "天到期，请及时联系开发者");
                _sangoSecurityCheckWnd.UpdateBtnInfo("SkipBtn", "");
                break;
            case RegistInfoCheckResult.CheckFailed_OutData:
                _sangoSecurityCheckWnd.SetRoot(this, _currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("软件已过期，请输入注册码后点击激活");
                break;
            case RegistInfoCheckResult.CheckError_SystemTimeChanged:
                _sangoSecurityCheckWnd.SetRoot(this, _currentTypeInConfig);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("注册信息检查失败，系统时间被修改");
                break;
            case RegistInfoCheckResult.UpdateOK_Success:
                _sangoSecurityCheckWnd.UpdateResult("成功激活，软件可运行至" + commands);
                _sangoSecurityCheckWnd.UpdateBtnInfo("RegistBtn", "确定");
                break;
            case RegistInfoCheckResult.UpdateFailed_OutData:
                _sangoSecurityCheckWnd.UpdateResult("激活失败，密钥已过期");
                break;
            case RegistInfoCheckResult.UpdateFailed_SignError:
                _sangoSecurityCheckWnd.UpdateResult("输入有误，请重新输入或联系开发者");
                break;
            case RegistInfoCheckResult.UpdateFailed_WriteInfoError:
                _sangoSecurityCheckWnd.UpdateResult("写入注册信息失败，请重试");
                break;
            case RegistInfoCheckResult.UpdateError_NullInfo:
                _sangoSecurityCheckWnd.UpdateResult("输入有误，输入数据不能为空");
                break;
            case RegistInfoCheckResult.UpdateError_SyntexError:
                _sangoSecurityCheckWnd.UpdateResult("输入有误，请重新输入或联系开发者");
                break;
            case RegistInfoCheckResult.UpdateError_LenghthError:
                _sangoSecurityCheckWnd.UpdateResult("输入有误，输入的数据长度不正确");
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
            Debug.Log("已手动删除注册信息");
        }
    }
}

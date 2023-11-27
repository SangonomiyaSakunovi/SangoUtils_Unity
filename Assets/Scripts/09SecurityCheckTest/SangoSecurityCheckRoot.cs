public class SangoSecurityCheckRoot : BaseRoot
{
    public SangoSecurityCheckWnd _sangoSecurityCheckWnd;

    private void Start()
    {
        SecurityCheckServiceConfig config = new SecurityCheckServiceConfig
        {
            apiKey = "SangoSecurityRegistKey",
            apiSecret = "SangoSecurityRegistSecret",
            defaultRegistLimitTimestamp = "1701053130",
            secretTimestamp = "1645467742",
            registInfoCode = RegistInfoCode.Timestamp,
            signMethodCode = SignMethodCode.Md5,
            resultActionCallBack = RegistInfoCheckResultActionCallBack
        };

        InitSecurityCheckService(config);
        CheckRegistValidation();
        GetNewRegistInfo("1700859600");
    }

    public override void RegistInfoCheckResultActionCallBack(RegistInfoCheckResult result)
    {
        switch (result)
        {
            case RegistInfoCheckResult.CheckOK_Valid:
                _sangoSecurityCheckWnd.SetWindowState(false);
                OnSecurityCheckResultValid();
                break;
            case RegistInfoCheckResult.CheckOK_FirstRun:
                _sangoSecurityCheckWnd.SetWindowState(false);
                OnSecurityCheckResultValid();
                break;
            case RegistInfoCheckResult.CheckFailed_OutData:
                _sangoSecurityCheckWnd.SetRoot(this);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("注册信息检查失败，请联系开发者获取注册密钥");
                break;
            case RegistInfoCheckResult.CheckError_SystemTimeChanged:
                _sangoSecurityCheckWnd.SetRoot(this);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("注册信息检查失败，系统时间被修改");
                break;
            case RegistInfoCheckResult.UpdateOK_Success:
                _sangoSecurityCheckWnd.SetWindowState(false);
                OnSecurityCheckResultValid();
                break;
            case RegistInfoCheckResult.UpdateFailed_OutData:
                _sangoSecurityCheckWnd.UpdateResult("注册失败，密钥已过期");
                break;
            case RegistInfoCheckResult.UpdateFailed_SignError:
                _sangoSecurityCheckWnd.UpdateResult("注册失败，密钥错误");
                break;
            case RegistInfoCheckResult.UpdateFailed_WriteInfoError:
                _sangoSecurityCheckWnd.UpdateResult("写入注册信息失败，请重试");
                break;
            case RegistInfoCheckResult.UpdateError_NullInfo:
                _sangoSecurityCheckWnd.UpdateResult("注册失败，注册数据不能为空");
                break;
            case RegistInfoCheckResult.UpdateError_SyntexError:
                _sangoSecurityCheckWnd.UpdateResult("注册失败，请输入正确的格式数据");
                break;
        }
    }

    protected override void OnSecurityCheckResultValid()
    {
        base.OnSecurityCheckResultValid();
        //TODO
    }
}

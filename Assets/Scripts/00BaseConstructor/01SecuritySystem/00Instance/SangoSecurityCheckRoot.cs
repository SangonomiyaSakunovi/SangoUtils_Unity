using System;

public class SangoSecurityCheckRoot : BaseRoot<SangoSecurityCheckRoot>
{
    public SangoSecurityCheckWnd _sangoSecurityCheckWnd;

    private void Start()
    {
        SecurityCheckServiceConfig config = new SecurityCheckServiceConfig
        {
            apiKey = "s",
            apiSecret = "s",
            defaultRegistLimitDateTime = new DateTime(2022, 2, 22, 0, 0, 0),
            secretTimestamp = "0",
            registInfoCode = RegistInfoCode.Timestamp,
            signMethodCode = SignMethodCode.Md5,
            checkLength = 5,
            registMixSignDataProtocol = RegistMixSignDataProtocol.A_B_C_SIGN,
            resultActionCallBack = RegistInfoCheckResultActionCallBack
        };

        InitSecurityCheckService(config);
        CheckRegistValidation();
    }

    private void RegistInfoCheckResultActionCallBack(RegistInfoCheckResult result)
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
                _sangoSecurityCheckWnd.UpdateResult("ע����Ϣ���ʧ�ܣ�����ϵ�����߻�ȡע����Կ");
                break;
            case RegistInfoCheckResult.CheckError_SystemTimeChanged:
                _sangoSecurityCheckWnd.SetRoot(this);
                _sangoSecurityCheckWnd.SetWindowState();
                _sangoSecurityCheckWnd.UpdateResult("ע����Ϣ���ʧ�ܣ�ϵͳʱ�䱻�޸�");
                break;
            case RegistInfoCheckResult.UpdateOK_Success:
                _sangoSecurityCheckWnd.SetWindowState(false);
                OnSecurityCheckResultValid();
                break;
            case RegistInfoCheckResult.UpdateFailed_OutData:
                _sangoSecurityCheckWnd.UpdateResult("ע��ʧ�ܣ���Կ�ѹ���");
                break;
            case RegistInfoCheckResult.UpdateFailed_SignError:
                _sangoSecurityCheckWnd.UpdateResult("ע��ʧ�ܣ���Կ����");
                break;
            case RegistInfoCheckResult.UpdateFailed_WriteInfoError:
                _sangoSecurityCheckWnd.UpdateResult("д��ע����Ϣʧ�ܣ�������");
                break;
            case RegistInfoCheckResult.UpdateError_NullInfo:
                _sangoSecurityCheckWnd.UpdateResult("ע��ʧ�ܣ�ע�����ݲ���Ϊ��");
                break;
            case RegistInfoCheckResult.UpdateError_SyntexError:
                _sangoSecurityCheckWnd.UpdateResult("ע��ʧ�ܣ���������ȷ�ĸ�ʽ����");
                break;
            case RegistInfoCheckResult.UpdateError_LenghthError:
                _sangoSecurityCheckWnd.UpdateResult("ע��ʧ�ܣ���������ݳ��Ȳ���ȷ");
                break;
        }
    }

    private void OnSecurityCheckResultValid()
    {       
        //TODO
    }
}

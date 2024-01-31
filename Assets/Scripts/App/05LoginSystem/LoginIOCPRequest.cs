using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Logger;
using SangoUtils_NetOperation;

public class LoginIOCPRequest : BaseNetRequest
{
    private LoginReqMessage _message;

    public void SetAndSendLoginReqMessage(LoginMode loginMode, string uID, string password)
    {
        _message = new(loginMode, uID, password);
        DefaultOperationRequest();
    }

    protected override void DefaultOperationRequest()
    {
        string jsonString = ToJson(_message);
        IOCPService.Instance.SendOperationRequest(NetOperationCode, jsonString);
    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Done("The loginResult from server: " + message);
        LoginRspMessage loginReqMessage = FromJson<LoginRspMessage>(message);
        if (loginReqMessage != null )
        {
            SystemService.Instance.LoginSystem.OnLoginSucceed(loginReqMessage.EntityID);
        }
    }
} 

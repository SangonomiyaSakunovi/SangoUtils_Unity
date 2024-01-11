using SangoUtils_Common.Messages;

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
        string jsonString = SetJsonString(_message);
        IOCPService.Instance.SendOperationRequest(NetOperationCode, jsonString);
    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Done("The loginResult from server: " + message);
        LoginRspMessage loginReqMessage = DeJsonString<LoginRspMessage>(message);
        if (loginReqMessage != null )
        {
            SystemRoot.Instance.LoginSystem.OnLoginSucceed(loginReqMessage.EntityID);
        }
    }
} 

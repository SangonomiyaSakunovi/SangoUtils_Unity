using SangoUtils_Common.Messages;

public class LoginNetRequest : BaseNetRequest
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
        NetService.Instance.SendOperationRequest(NetOperationCode, jsonString);
    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Done("The loginResult from server: " + message);
        LoginRspMessage loginReqMessage = DeJsonString<LoginRspMessage>(message);
        if (loginReqMessage != null )
        {
            LoginSystem.Instance.OnLoginSucceed(loginReqMessage.EntityID);
        }
    }
}

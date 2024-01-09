using SangoUtils_Common.Messages;

public class LoginWebSocketRequest : BaseNetRequest
{
    private LoginReqMessage _message;

    public void SetAndSendLoginReqMessage(LoginMode loginMode, string uID, string password)
    {
        _message = new(loginMode, uID, password);
        DefaultOperationRequest();
    }
    
    public override void OnOperationResponse(string message)
    {
        SangoLogger.Done("The loginResult from server: " + message);
        LoginRspMessage loginRspMessage = DeJsonString<LoginRspMessage>(message);
        if (loginRspMessage != null)
        {
            LoginSystem.Instance.OnLoginSucceed(loginRspMessage.EntityID);
        }
    }

    protected override void DefaultOperationRequest()
    {
        string jsonString = SetJsonString(_message);
        WebSocketService.Instance.SendOperationRequest(NetOperationCode, jsonString);
    }
}

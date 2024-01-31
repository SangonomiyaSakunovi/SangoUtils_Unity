using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Logger;
using SangoUtils_NetOperation;

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
        LoginRspMessage_SunUp loginRspMessage = FromJson<LoginRspMessage_SunUp>(message);
        if (loginRspMessage != null)
        {
            SystemService.Instance.LoginSystem.OnLoginSucceed(loginRspMessage.client_id);
        }
    }

    protected override void DefaultOperationRequest()
    {
        string jsonString = ToJson(_message);
        WebSocketService.Instance.SendOperationRequest(NetOperationCode, jsonString);
    }
}

using SangoNetProtol;
using SangoUtils_Common.Messages;

public class LoginSystem : BaseSystem<LoginSystem>
{
    private LoginWebSocketRequest _loginWebSocketRequest;
    public LoginWnd _loginWnd;
    public GameObjectCharacterController _characterController;

    private string _entityId = "SangoTestCapsule001";

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _loginWebSocketRequest = WebSocketService.Instance.GetNetRequest<LoginWebSocketRequest>(NetOperationCode.Login);
        _loginWnd.SetSystem(this);
        _loginWnd.SetWindowState();
    }

    public void LoginAsGuest()
    {
        _loginWebSocketRequest.SetAndSendLoginReqMessage(LoginMode.Guest, _entityId, "");
    }

    public void OnLoginSucceed(string entityID)
    {
        _characterController.EntityID = entityID; 
        SceneMainInstance.Instance._currentEntityID = entityID;
    }
}

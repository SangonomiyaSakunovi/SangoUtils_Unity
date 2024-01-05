using SangoNetProtol;
using SangoUtils_Common.Messages;

public class LoginSystem : BaseSystem<LoginSystem>
{
    private LoginNetRequest _loginNetRequest;
    public LoginWnd _loginWnd;
    public GameObjectCharacterController _characterController;

    private string _entityId = "SangoTestCapsule001";

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _loginNetRequest = NetService.Instance.GetNetRequest<LoginNetRequest>(NetOperationCode.Login);
        _loginWnd.SetSystem(this);
        _loginWnd.SetWindowState();
    }

    public void LoginAsGuest()
    {
        _loginNetRequest.SetAndSendLoginReqMessage(LoginMode.Guest, _entityId, "");
    }

    public void OnLoginSucceed(string entityID)
    {
        _characterController.EntityID = entityID; 
        SceneMainInstance.Instance._currentEntityID = entityID;
    }
}

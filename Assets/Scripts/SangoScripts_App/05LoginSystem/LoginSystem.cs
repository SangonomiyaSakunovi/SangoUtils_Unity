using SangoNetProtol;
using SangoUtils_Common.Messages;

public class LoginSystem : BaseSystem<LoginSystem>
{
    private LoginWebSocketRequest _loginWebSocketRequest;
    public LoginWnd _loginWnd;
    public PlayerController _playerController;

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
        _playerController.EntityID = entityID;
        TransformData transformData = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1));
        CacheService.Instance.PlayerEntityThis = new(entityID, transformData, PlayerState.Online);
        _playerController.SetPlayerEntity(CacheService.Instance.PlayerEntityThis);
    }
}

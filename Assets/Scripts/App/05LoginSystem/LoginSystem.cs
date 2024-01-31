using SangoNetProtol;
using SangoUtils_Unity_App.Controller;
using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Bases_Universal;
using SangoUtils_Bases_UnityEngine;

public class LoginSystem : BaseSystem<LoginSystem>
{
    private LoginWebSocketRequest _loginWebSocketRequest;
    private LoginWnd _loginWnd;
    private PlayerController _playerController;

    private string _entityId = "SangoTestCapsule001";

    public LoginSystem()
    {
        _loginWebSocketRequest = WebSocketService.Instance.GetNetRequest<LoginWebSocketRequest>(NetOperationCode.Login);
        _loginWnd = WindowRoot.Instance.LoginWnd;
        _playerController = WindowRoot.Instance.PlayerController;
        _loginWnd.SetWindowState();
    }

    public void LoginAsGuest()
    {
        _loginWebSocketRequest.SetAndSendLoginReqMessage(LoginMode.Guest, _entityId, "");
    }

    public override void OnAwake()
    {
        
    }

    public override void OnDispose()
    {
        
    }

    public override void OnInit()
    {
        
    }

    public void OnLoginSucceed(string entityID)
    {
        _playerController.EntityID = entityID;
        CacheService.Instance.EntityCache.AddEntityLocal(entityID);
        _playerController.SetPlayerEntity(CacheService.Instance.EntityCache.PlayerEntity_This);
    }

    protected override void OnUpdate()
    {

    }
}

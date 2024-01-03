using SangoNetProtol;
using SangoUtils_Common.Messages;

public class LoginSystem : BaseSystem<LoginSystem>
{
    private LoginNetRequest _loginNetRequest;
    private LoginWnd _loginWnd;

    private string _entityId = "SangoTestCapsule001";

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _loginNetRequest = NetService.Instance.GetNetRequest<LoginNetRequest>(NetOperationCode.Login);
        _loginWnd = GetComponent<LoginWnd>();
        _loginWnd.SetSystem(this);
        _loginWnd.SetWindowState();
    }

    public void LoginAsGuest()
    {
        _loginNetRequest.SetLoginReqMessage(LoginMode.Guest, _entityId, "");
    }

    public void OnLoginSucceed()
    {
        //Invoke SceneSystem
    }
}

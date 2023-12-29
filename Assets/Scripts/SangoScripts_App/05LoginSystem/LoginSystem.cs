using SangoNetProtol;

public class LoginSystem : BaseSystem<LoginSystem>
{
    private LoginNetRequest _loginNetRequest;
    private LoginWnd _loginWnd;

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
        //TODO
    }
}

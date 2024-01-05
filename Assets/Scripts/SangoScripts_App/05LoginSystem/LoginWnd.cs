using UnityEngine.UI;

public class LoginWnd : BaseWindow
{
    private LoginSystem _loginSystem;

    private Button _btnLoginAsGuest;

    public override void SetSystem<T>(BaseSystem<T> baseSystem)
    {
        base.SetSystem(baseSystem);
        _loginSystem = baseSystem as LoginSystem;
    }

    protected override void OnInit()
    {
        base.OnInit();
        _btnLoginAsGuest = transform.Find("btnLoginAsGuest").GetComponent<Button>();
        SetButtonListener(_btnLoginAsGuest, OnBtnLoginAsGuestClicked);
    }

    private void OnBtnLoginAsGuestClicked(Button button)
    {
        _loginSystem.LoginAsGuest();
        base.SetWindowState(false);
    }
}

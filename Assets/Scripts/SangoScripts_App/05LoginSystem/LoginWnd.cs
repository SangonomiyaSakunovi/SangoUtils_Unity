using UnityEngine.UI;

public class LoginWnd : BaseWindow
{
    private Button _btnLoginAsGuest;

    protected override void OnInit()
    {
        base.OnInit();
        _btnLoginAsGuest = transform.Find("btnLoginAsGuest").GetComponent<Button>();
        SetButtonListener(_btnLoginAsGuest, OnBtnLoginAsGuestClicked);
    }

    private void OnBtnLoginAsGuestClicked(Button button)
    {
        SystemRoot.Instance.LoginSystem.LoginAsGuest();       
        base.SetWindowState(false);
    }
}

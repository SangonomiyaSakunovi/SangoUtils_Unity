using SangoUtils_Extensions_UnityEngine.Core;
using UnityEngine.UI;

public class LoginWnd : BaseWindow
{
    private Button _btnLoginAsGuest;

    protected override void OnInit()
    {
        base.OnInit();
        _btnLoginAsGuest = transform.Find("btnLoginAsGuest").GetComponent<Button>();
        _btnLoginAsGuest.AddListener_OnClick(OnBtnLoginAsGuestClicked);
    }

    private void OnBtnLoginAsGuestClicked()
    {
        SystemRoot.Instance.LoginSystem.LoginAsGuest();       
        base.SetWindowState(false);
    }
}

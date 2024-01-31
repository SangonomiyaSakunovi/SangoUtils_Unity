using SangoUtils_Bases_UnityEngine;
using SangoUtils_Extensions_UnityEngine.Core;
using UnityEngine.UI;

public class LoginWnd : BaseWindow
{
    private Button _btnLoginAsGuest;

    public override void OnAwake()
    {
        WindowLayer = WindowLayer.Base;
        AddWindow(this);
    }

    protected override void OnDispose()
    {
        
    }

    protected override void OnInit()
    {       
        _btnLoginAsGuest = transform.Find("btnLoginAsGuest").GetComponent<Button>();
        _btnLoginAsGuest.AddListener_OnClick(OnBtnLoginAsGuestClicked);

        WindowLayer = WindowLayer.Base;
        AddWindow(this);
    }

    private void OnBtnLoginAsGuestClicked()
    {
        SystemService.Instance.LoginSystem.LoginAsGuest();       
        base.SetWindowState(false);
    }
}

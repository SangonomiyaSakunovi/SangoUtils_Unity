using SangoScripts_App.Controller;
using UnityEngine;

public class WindowRoot : MonoBehaviour
{
    public static WindowRoot Instance { get; private set; }

    public LoginWnd LoginWnd;
    public PlayerController PlayerController;

    public void OnInit()
    {
        Instance = this;
    }
}

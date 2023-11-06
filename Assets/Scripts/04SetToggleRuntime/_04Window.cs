using UnityEngine;
using UnityEngine.UI;

public class _04Window : BaseWindow
{
    public Transform toggleRootTans;

    private void Start()
    {
        SetToggleListeners(toggleRootTans, OnToggleClicked);
    }

    private void OnToggleClicked(Toggle toggle)
    {
        if (toggle.isOn)
        {
            Debug.Log(toggle.name);
        }
    }
}

using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    public void SetPanelState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
            if (isActive)
            {
                OnInit();
            }
            else
            {
                OnDispose();
            }
        }
    }

    protected virtual void OnInit() { }

    protected virtual void OnDispose() { }
}
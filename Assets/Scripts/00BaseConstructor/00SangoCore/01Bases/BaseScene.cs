using UnityEngine;

public class BaseScene<T> : MonoSingleton<T> where T : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
    }

    private void OnDestroy()
    {
        OnDispose();
    }

    public virtual void OnInit()
    {

    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnDispose()
    {

    }
}

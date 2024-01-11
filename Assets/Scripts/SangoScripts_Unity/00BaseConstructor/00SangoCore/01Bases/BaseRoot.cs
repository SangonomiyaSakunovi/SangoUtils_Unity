using UnityEngine;

public abstract class BaseRoot<T> : MonoBehaviour
{
    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnInit()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    public virtual void OnDispose()
    {

    }
}

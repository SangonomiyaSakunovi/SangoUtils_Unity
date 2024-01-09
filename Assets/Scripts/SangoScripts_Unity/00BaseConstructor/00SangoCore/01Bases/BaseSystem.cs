using UnityEngine;

public abstract class BaseSystem<T> : MonoSingleton<T> where T : MonoBehaviour
{
    private void Update()
    {
        OnUpdate();
    }

    public virtual void SetSystem()
    {

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

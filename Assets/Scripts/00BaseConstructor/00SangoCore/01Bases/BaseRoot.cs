using UnityEngine;

public class BaseRoot<T> : UnitySingleton<T> where T : MonoBehaviour
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

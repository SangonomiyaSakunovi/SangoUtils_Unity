using UnityEngine;

public class BaseService<T> : UnitySingleton<T> where T : MonoBehaviour
{
    private void Awake()
    {
        OnInit();
    }

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

using SangoUtils_Extensions_UnityEngine.Core;
using UnityEngine;

public abstract class BaseService<T> : UnitySingleton<T> where T : MonoBehaviour
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

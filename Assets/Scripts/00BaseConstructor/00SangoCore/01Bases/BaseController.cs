using UnityEngine;

public class BaseController : MonoBehaviour
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

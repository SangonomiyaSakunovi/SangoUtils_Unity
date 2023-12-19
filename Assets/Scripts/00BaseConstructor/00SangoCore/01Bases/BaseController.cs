using UnityEngine;

public class BaseController : BaseUIElements
{
    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }
}

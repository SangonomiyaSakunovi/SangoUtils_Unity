using UnityEngine;

public class BaseWindow : BaseUIElements
{
    public virtual void SetRoot<T>(BaseRoot<T> baseRoot) where T : MonoBehaviour
    {

    }

    public virtual void SetSystem(BaseSystem baseSystem)
    {

    }
}

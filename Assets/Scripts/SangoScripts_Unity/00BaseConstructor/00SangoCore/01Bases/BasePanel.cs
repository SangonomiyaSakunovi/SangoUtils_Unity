using UnityEngine;

public abstract class BasePanel : BaseUIElements
{
    public virtual void SetRoot<T>(BaseRoot<T> baseRoot) where T : MonoBehaviour
    {

    }

    public virtual void SetSystem<T>(BaseSystem<T> baseSystem) where T : MonoBehaviour
    {

    }
}

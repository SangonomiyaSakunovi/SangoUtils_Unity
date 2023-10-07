using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    protected BaseService _service = null;

    public void SetWindowState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }
        if (isActive)
        {
            InitWindow();
        }
        else
        {
            ClearWindow();
        }
    }

    protected virtual void InitWindow()
    {

    }

    protected virtual void ClearWindow()
    {

    }

    protected T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null)
        {
            t = gameObject.AddComponent<T>();
        }
        return t;
    }

    protected void RemoveComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t != null)
        {
            Destroy(t);
        }
    }
}

public class BaseService : MonoBehaviour
{

}

public abstract class BaseSystem : MonoBehaviour
{
    public abstract void InitSystem();
}

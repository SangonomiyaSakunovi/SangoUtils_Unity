using UnityEngine;
using UnityEngine.UI;

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

    protected void SetSprite(Image image, string path, bool isCache = false)
    {
        Sprite sprite = ResourceService.Instance.LoadSprite(path, isCache);
        Image imageComponent = image.GetComponent<Image>();
        imageComponent.sprite = sprite;
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
    public virtual void InitService()
    {

    }
}

public abstract class BaseSystem : MonoBehaviour
{
    public abstract void InitSystem();
}

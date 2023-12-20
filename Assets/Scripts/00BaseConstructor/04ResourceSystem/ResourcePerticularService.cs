using UnityEngine;

public class ResourcePerticularService : BaseService<ResourcePerticularService>
{
    public override void OnInit()
    {
        base.OnInit();
    }

    private T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null)
        {
            t = gameObject.AddComponent<T>();
        }
        return t;
    }

}

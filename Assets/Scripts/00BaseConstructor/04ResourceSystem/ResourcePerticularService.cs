using System;
using UnityEngine;
using UnityGLTF;

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

    public void LoadAndSetGLTFModelOnlineAsync(GameObject parentObject, string urlPath)
    {
        GLTFComponent component = GetOrAddComponent<GLTFComponent>(parentObject);
        component.GLTFUri = urlPath;
    }
}

using UnityEngine;

public class BaseService<T> : UnitySingleton<T> where T : MonoBehaviour
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

    protected GameObject InstantiateGameObject(Transform parentTrans, string path, bool isCache = false)
    {
        GameObject prefab = ResourceService.Instance.LoadPrefab(path, isCache);
        GameObject instantiatedPrefab = Instantiate(prefab, parentTrans);
        return instantiatedPrefab;
    }

    protected GameObject InstantiateGameObject(GameObject parentObject, string path, bool isCache = false)
    {
        return InstantiateGameObject(parentObject.transform, path, isCache);
    }
}

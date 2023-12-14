using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get { return _instance; }
    }

    protected void SetInstance(T instance)
    {
        _instance = instance;
    }

    protected GameObject InstantiateGameObject(Transform parentTrans, string path, bool isAssetBundle = false, bool isCache = false)
    {
        GameObject prefab = null;
        if (isAssetBundle)
        {

        }
        else
        {
            prefab = ResourceService.Instance.LoadPrefab(path, isCache);
        }
        GameObject instantiatedPrefab = Instantiate(prefab, parentTrans);
        return instantiatedPrefab;
    }

    protected GameObject InstantiateGameObject(GameObject parentObject, string path, bool isAssetBundle = false, bool isCache = false)
    {
        return InstantiateGameObject(parentObject.transform, path, isAssetBundle, isCache);
    }
}

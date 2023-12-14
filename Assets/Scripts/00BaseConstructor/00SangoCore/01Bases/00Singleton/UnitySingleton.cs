using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (null == _instance)
                {
                    GameObject gameObject = new GameObject("[" + typeof(T).FullName + "]");
                    _instance = gameObject.AddComponent<T>();
                    DontDestroyOnLoad(gameObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (null != _instance && _instance != this)
        {
            Destroy(gameObject);
        }
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

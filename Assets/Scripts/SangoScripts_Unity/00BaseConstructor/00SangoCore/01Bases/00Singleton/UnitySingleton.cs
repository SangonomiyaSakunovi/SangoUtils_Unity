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
}

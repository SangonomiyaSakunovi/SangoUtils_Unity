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
}

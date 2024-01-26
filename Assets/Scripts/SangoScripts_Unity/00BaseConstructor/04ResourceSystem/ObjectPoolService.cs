using SangoUtils_Bases_UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolService : BaseService<ObjectPoolService>
{
    private ObjectPool<List<GameObject>> _childrenObjectsPool;

    public override void OnInit()
    {
        _childrenObjectsPool = new ObjectPool<List<GameObject>>(() => new List<GameObject>());
    }

    public List<GameObject> GetGameObjects()
    {
        return _childrenObjectsPool.Get();
    }

    public void Release(List<GameObject> gameObjects)
    {
        _childrenObjectsPool.Release(gameObjects);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectFactory : ScriptableObject
{
    protected GameObject CreateInstance(GameObject prefab)
    {
        GameObject instance = ObjectPool.Instance.Spawn(prefab);
        return instance;
    }

    
}

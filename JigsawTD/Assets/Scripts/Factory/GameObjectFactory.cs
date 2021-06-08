using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectFactory : ScriptableObject
{
    protected ReusableObject CreateInstance(ReusableObject prefab)
    {
        ReusableObject instance = ObjectPool.Instance.Spawn(prefab);
        return instance;
    }


}

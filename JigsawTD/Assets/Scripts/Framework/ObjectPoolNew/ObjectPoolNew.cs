using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolNew : Singleton<ObjectPoolNew>
{
    public string ResourceDir = "";
    Dictionary<string, SubPoolNew> m_pools = new Dictionary<string, SubPoolNew>();
    Dictionary<SubPoolNew, Transform> m_pools_parent = new Dictionary<SubPoolNew, Transform>();

    public void ClearPools()
    {
        m_pools.Clear();
    }
    //取对象
    public ReusableObject Spawn(string name, Vector2 pos)
    {
        if (!m_pools.ContainsKey(name))
            RegisterNew(name);
        SubPoolNew pool = m_pools[name];

        return pool.Spawn(m_pools_parent[pool], pos);

    }

    public ReusableObject Spawn(ReusableObject gameObj, Vector2 pos)
    {
        if (!m_pools.ContainsKey(gameObj.name))
            RegisterNew(gameObj);
        SubPoolNew pool = m_pools[gameObj.name];

        return pool.Spawn(m_pools_parent[pool], pos);
    }
    //回收对象
    public void UnSpawn(ReusableObject go)
    {
        SubPoolNew pool = null;
        foreach (SubPoolNew p in m_pools.Values)
        {
            if (p.Contains(go))
            {
                pool = p;
                break;
            }
        }
        pool.UnSpawn(go);
    }
    //回收所有对象
    public void UnSpawnAll()
    {
        foreach (SubPoolNew p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }
    //创建新子池子
    void RegisterNew(string name)
    {
        //预设路径
        string path = "";
        if (string.IsNullOrEmpty(ResourceDir))
            path = name;
        else
            path = ResourceDir + "/" + name;

        //加载预设
        ReusableObject prefab = Resources.Load<ReusableObject>(path);

        //创建子对象池
        SubPoolNew pool = new SubPoolNew(name, prefab);
        m_pools.Add(pool.Name, pool);

        GameObject container = new GameObject($"Pool-{name}");
        m_pools_parent.Add(pool, container.transform);
    }

    void RegisterNew(ReusableObject obj)
    {
        ReusableObject prefab = obj;

        SubPoolNew pool = new SubPoolNew(obj.name, prefab);
        m_pools.Add(pool.Name, pool);

        GameObject container = new GameObject($"Pool-{obj.name}");
        m_pools_parent.Add(pool, container.transform);
    }

}

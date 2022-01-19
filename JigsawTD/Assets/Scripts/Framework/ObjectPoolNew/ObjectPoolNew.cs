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
    //ȡ����
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
    //���ն���
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
    //�������ж���
    public void UnSpawnAll()
    {
        foreach (SubPoolNew p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }
    //�������ӳ���
    void RegisterNew(string name)
    {
        //Ԥ��·��
        string path = "";
        if (string.IsNullOrEmpty(ResourceDir))
            path = name;
        else
            path = ResourceDir + "/" + name;

        //����Ԥ��
        ReusableObject prefab = Resources.Load<ReusableObject>(path);

        //�����Ӷ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    ReusableObject m_prefab;
    string m_Name;
    List<ReusableObject> m_objects = new List<ReusableObject>();
    public string Name
    {
        //get { return m_prefab.name; }
        get { return m_Name; }
    }
    public SubPool(string name, ReusableObject prefab)
    {
        this.m_prefab = prefab;
        m_Name = name;
    }



    public ReusableObject Spawn(Transform container)
    {
        ReusableObject go = null;
        foreach (ReusableObject obj in m_objects)
        {
            if (!obj.gameObject.activeSelf)
            {
                go = obj;
                break;
            }
        }

        if (go == null)
        {
            go = GameObject.Instantiate<ReusableObject>(m_prefab);
            m_objects.Add(go);
            go.transform.SetParent(container.transform);
            go.ParentObj = container.transform;
        }

        go.gameObject.SetActive(true);
        go.OnSpawn();
        return go;
    }

    //回收对象
    public void UnSpawn(ReusableObject go)
    {
        if (Contains(go))
        {
            go.OnUnSpawn();
            go.gameObject.SetActive(false);
        }
    }

    public void UnSpawnAll()
    {
        foreach (ReusableObject item in m_objects)
        {
            if (item.gameObject.activeSelf)
            {
                UnSpawn(item);
            }
        }
    }

    public bool Contains(ReusableObject go)
    {
        return m_objects.Contains(go);
    }
}

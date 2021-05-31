using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T m_instance = null;
    public static T Instance
    {
        get { return m_instance; }
    }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            Debug.Log(this.name + "已经创建了相同singleton实例");
        }
        else
            m_instance = this as T;
    }

}

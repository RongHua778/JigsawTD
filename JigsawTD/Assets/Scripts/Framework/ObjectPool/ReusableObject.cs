using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReusableObject : MonoBehaviour,IResuable
{
    public Transform ParentObj;
    public virtual void OnSpawn()
    {

    }
    public virtual void OnUnSpawn()
    {
        if (ParentObj != null)
            transform.SetParent(ParentObj);
    }
}

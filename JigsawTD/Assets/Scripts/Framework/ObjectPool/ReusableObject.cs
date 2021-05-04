using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReusableObject : MonoBehaviour,IResuable
{
    public Transform ParentObj { get; set; }
    public virtual void OnSpawn()
    {

    }
    public virtual void OnUnSpawn()
    {
        SetBackToParent();
    }

    public void SetBackToParent()
    {
        if (ParentObj != null)
            transform.SetParent(ParentObj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReusableObject : MonoBehaviour,IResuable
{
    [HideInInspector]
    public Transform ParentObj = null;
    public bool isActive;
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

    public void UnspawnAfterTime(float time)
    {
        StartCoroutine(UnspawnCor(time));
    }

    IEnumerator UnspawnCor(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.UnSpawn(this);
    }
}

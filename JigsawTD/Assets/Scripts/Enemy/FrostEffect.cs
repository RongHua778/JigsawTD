using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostEffect : ReusableObject
{
    private Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    public void Broke(float time = 0)
    {
        anim.SetTrigger("Broke");
    }


    public void ReclaimFrost()
    {
        ObjectPool.Instance.UnSpawn(this);
    }

}

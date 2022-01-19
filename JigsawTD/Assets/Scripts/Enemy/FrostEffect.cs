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
    public void Broke()
    {
        anim.SetBool("Frost", false);
    }


    public void ReclaimFrost()
    {
        ObjectPool.Instance.UnSpawn(this);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        anim.SetBool("Frost", true);
        //m_Partical.Play();
    }

}

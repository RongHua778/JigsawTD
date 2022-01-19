using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalControl : ReusableObject
{
    private ParticleSystem ps;
    private void Awake()
    {
        ps = this.GetComponent<ParticleSystem>();
    }

    //private void Update()
    //{
    //    if (!ps.IsAlive())
    //    {
    //        ObjectPoolNew.Instance.UnSpawn(this);                     
    //    }
    //}
    private void FixedUpdate()
    {
        if (!ps.IsAlive())
        {
            ObjectPool.Instance.UnSpawn(this);
        }
    }

    public void PlayEffect()
    {
        ps.Play();
        //Invoke(nameof(ReclaimPartical), ReclaimTime);
    }

    //protected void ReclaimPartical()
    //{
    //    ObjectPoolNew.Instance.UnSpawn(this);
    //}

}

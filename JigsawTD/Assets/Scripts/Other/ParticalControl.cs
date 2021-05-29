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

    private void Update()
    {
        if (!ps.IsAlive())
        {
            ObjectPool.Instance.UnSpawn(this.gameObject);                     
        }
    }

}

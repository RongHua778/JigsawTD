using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainPerfectAnim : ReusableObject
{
    public void ReclaimSelf()
    {
        ObjectPool.Instance.UnSpawn(this);
    }
}

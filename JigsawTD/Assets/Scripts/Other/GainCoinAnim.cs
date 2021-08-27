using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainCoinAnim : ReusableObject
{
    public void ReclaimSelf()
    {
        ObjectPool.Instance.UnSpawn(this);
    }
}

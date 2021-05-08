using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;

    private void Awake()
    {
        bulletSpeed = 5f;
    }

}

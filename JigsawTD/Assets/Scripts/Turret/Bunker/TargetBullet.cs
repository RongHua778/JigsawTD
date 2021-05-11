using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;

    protected override void DealDamage()
    {
        target.Enemy.ApplyDamage(damage);
    }
}

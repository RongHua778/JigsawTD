using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;

    protected override void DealDamage()
    {
        base.DealDamage();
        TriggerHitEffect(Target.Enemy);
        Target.Enemy.ApplyDamage(Random.value <= CriticalRate ? Damage * 2 : Damage);
    }
}

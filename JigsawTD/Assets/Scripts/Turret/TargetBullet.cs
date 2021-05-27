using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;

    protected override void DealDamage()
    {
        base.DealDamage();
        float damage = Random.value <= CriticalRate ? Damage * 2 : Damage;
        if (SputteringRange > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, SputteringRange, enemyLayerMask);
            foreach (Collider2D hit in hits)
            {
                TargetPoint target = hit.GetComponent<TargetPoint>();
                if (target != null)
                {
                    TriggerHitEffect(target.Enemy);
                    target.Enemy.ApplyDamage(damage);
                }
            }
        }
        else
        {
            TriggerHitEffect(Target.Enemy);
            Target.Enemy.ApplyDamage(damage);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBullet : Bullet
{
    public override BulletType BulletType => BulletType.Ground;

    protected override void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, SputteringRange, enemyLayerMask);
        foreach (Collider2D hit in hits)
        {
            TargetPoint target = hit.GetComponent<TargetPoint>();
            if (target != null)
            {
                TriggerHitEffect(target.Enemy);
                target.Enemy.ApplyDamage(Damage);
            }
        }

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();

    }




}

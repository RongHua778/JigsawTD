using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBullet : Bullet
{
    public override BulletType BulletType => BulletType.Ground;

    protected override void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sputteringRange, enemyLayerMask);
        foreach (Collider2D hit in hits)
        {
            TargetPoint target = hit.GetComponent<TargetPoint>();
            if (target != null)
            {
                target.Enemy.ApplyDamage(damage);
            }
        }

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();

    }




}

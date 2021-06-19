using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : Bullet
{
    public override BulletType BulletType => BulletType.Penetrate;
    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        base.Initialize(turret, target, pos);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
    public override bool GameUpdate()
    {
        RotateBullet(TargetPos);
        return MoveTowards(TargetPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<TargetPoint>().Enemy;
        DealRealDamage(enemy, Damage);
    }

    protected override void TriggerDamage()
    {
        if (SputteringRange > 0)
        {
            if (SputteringEffect != null)
            {
                ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
                effect.transform.position = transform.position;
                effect.transform.localScale = Vector3.one * SputteringRange * 2;
                effect.PlayEffect();
            }
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, SputteringRange, enemyLayerMask);
            foreach (Collider2D hit in hits)
            {
                TargetPoint target = hit.GetComponent<TargetPoint>();
                DealRealDamage(target.Enemy, SputteringRate * Damage);
            }
        }

    }
}

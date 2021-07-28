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
        if (collision.GetComponent<TargetPoint>())
        {
            SputteredCount = 0;
            TriggerPreHitEffect();
            IDamageable enemy = collision.GetComponent<TargetPoint>().Enemy;
            EnemyDamageProcess(enemy);

            ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
            effect.transform.position = transform.position;
            effect.transform.localScale = 0.2f * Vector3.one;
            effect.PlayEffect();
        }
    }

    public override void TriggerDamage()
    {
        if (SputteringRange > 0)
        {
            ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
            effect.transform.position = transform.position;
            effect.transform.localScale = Vector3.one * SputteringRange;//´©Í¸×Óµ¯µÄ½¦Éä·¶Î§¼õ°ë
            effect.PlayEffect();
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, SputteringRange / 2, enemyLayerMask);
            SputteredCount = hits.Length;
            TriggerPreHitEffect();
            foreach (Collider2D hit in hits)
            {
                TargetPoint target = hit.GetComponent<TargetPoint>();
                EnemyDamageProcess(target.Enemy, true);
            }
        }

    }
}

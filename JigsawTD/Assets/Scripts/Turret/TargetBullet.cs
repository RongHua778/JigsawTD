using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;
    protected override Vector2 TargetPos
    {
        get
        {
            if (Target == null)
                return base.TargetPos;
            else
                return Target.Position;
        }
        set => base.TargetPos = value;
    }
    protected override void DealDamage()
    {
        base.DealDamage();
        float damage = Random.value <= CriticalRate ? Damage * 2 : Damage;
        if (SputteringRange > 0)
        {
            if (SputteringEffect != null)
            {
                GameObject effect = ObjectPool.Instance.Spawn(SputteringEffect);
                effect.transform.position = transform.position;
                effect.transform.localScale = Vector3.one * SputteringRange * 2;
                effect.GetComponent<ParticleSystem>().Play();
            }
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
            if (Target == null)
                return;
            TriggerHitEffect(Target.Enemy);
            Target.Enemy.ApplyDamage(damage);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SputteringRange);
    }
}

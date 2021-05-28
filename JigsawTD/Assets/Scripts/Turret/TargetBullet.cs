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
    protected override void TriggerDamage()
    {
        base.TriggerDamage();

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
                    DealRealDamage(target.Enemy);
                }
            }
        }
        else
        {
            if (Target == null)
                return;
            DealRealDamage(Target.Enemy);
        }

    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SputteringRange);
    }
}

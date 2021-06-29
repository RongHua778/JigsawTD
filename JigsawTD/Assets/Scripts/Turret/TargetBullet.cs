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
    public override void TriggerDamage()
    {
        base.TriggerDamage();

        ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Mathf.Max(0.3f, SputteringRange * 2) * Vector3.one;
        effect.PlayEffect();

        if (SputteringRange > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, SputteringRange, enemyLayerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                TargetPoint target = hits[i].GetComponent<TargetPoint>();
                if (target)
                {
                    if (target == Target)
                    {
                        DealRealDamage(target.Enemy, Damage);
                    }
                    else
                    {
                        DealRealDamage(target.Enemy, SputteringRate * Damage);
                    }
                }

            }
        }
        else
        {
            if (Target == null)
                return;
            DealRealDamage(Target.Enemy, Damage);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SputteringRange);
    }
}

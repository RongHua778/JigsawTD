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
                ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
                effect.transform.position = transform.position;
                effect.transform.localScale = Vector3.one * SputteringRange * 2;
                effect.PlayEffect();
            }
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, SputteringRange, enemyLayerMask);
            for (int i=0;i<hits.Length;i++)
            {
                TargetPoint target = hits[i].GetComponent<TargetPoint>();
                if (target.Object.Type == ObjectType.Armor)
                {
                    DealRealDamage(target.Object, Damage);
                }
                if (target == Target)
                {
                    DealRealDamage(target.Object, Damage);
                }
                else
                {
                    DealRealDamage(target.Object, SputteringRate * Damage);
                }
            }
        }
        else
        {
            if (Target == null)
                return;
            DealRealDamage(Target.Object,Damage);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TargetPoint>())
        {
            hit = true;

            Enemy enemy = collision.GetComponent<TargetPoint>().Object;
            DealRealDamage(enemy, Damage);
            if (enemy.Type == ObjectType.Armor)
            {
                ReclaimBullet();
                //hit = true;
                //Target = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SputteringRange);
    }
}

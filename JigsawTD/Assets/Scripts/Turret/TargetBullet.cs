using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;
    protected override Vector2 TargetPos
    {
        get => Target == null ? base.TargetPos : Target.Position;
        set => base.TargetPos = value;
    }

    public override bool GameUpdate()
    {
        if (Target != null && !Target.gameObject.activeSelf)//·Éµ½µØÃæ
        {
            TargetPos = Target.transform.position;
            Target = null;
        }
        RotateBullet(TargetPos);
        return MoveTowards(TargetPos);
    }
    public override void TriggerDamage()
    {
        SputteredCount = 0;
        Collider2D[] hits = null;
        if (SplashRange > 0)
        {
            hits = Physics2D.OverlapCircleAll(transform.position, SplashRange, StaticData.EnemyLayerMask);
            SputteredCount = hits.Length;
        }
        TriggerPreHitEffect();
        if (SplashRange > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                TargetPoint target = hits[i].GetComponent<TargetPoint>();
                if (target)
                {
                    if (target == Target)
                    {
                        DamageProcess(target.Enemy);
                    }
                    else
                    {
                        DamageProcess(target.Enemy, true);
                    }
                }

            }
        }
        else
        {
            if (Target != null)
            {
                TriggerPreHitEffect();
                DamageProcess(Target.Enemy);
            }
        }
        ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Mathf.Max(0.3f, SplashRange * 2) * Vector3.one;
        effect.PlayEffect();
        base.TriggerDamage();
    }

}

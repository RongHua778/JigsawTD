using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : Bullet
{
    Vector3 initScale;
    public override BulletType BulletType => BulletType.Penetrate;
    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        base.Initialize(turret, target, pos);
        initScale = transform.localScale;
        transform.localScale *= (1 + (SplashRange / (SplashRange + 5)) * 5);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        transform.localScale = initScale;
    }
    public override bool GameUpdate()
    {
        RotateBullet(TargetPos);
        //MoveTowards(TargetPos);
        return DistanceCheck(TargetPos);
        //return MoveTowardsRig(TargetPos);
    }

    public void FixedUpdate()
    {
        MoveTowardsRig(TargetPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetPoint point = collision.GetComponent<TargetPoint>();
        if (point != null)
        {
            SputteredCount = 0;
            TriggerPreHitEffect();
            //IDamage enemy = collision.GetComponent<TargetPoint>().Enemy;
            DamageProcess(point);

            //ParticalControl effect = ObjectPoolNew.Instance.Spawn(SputteringEffect, transform.position) as ParticalControl;
            //effect.transform.position = transform.position;
            //effect.transform.localScale = 0.2f * Vector3.one;
            //effect.PlayEffect();
        }
    }


}

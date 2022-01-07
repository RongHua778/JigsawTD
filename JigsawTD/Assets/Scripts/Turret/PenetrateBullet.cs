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
        transform.localScale *= (1 + (SplashRange / (SplashRange + 10)) * 5);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        transform.localScale = initScale;
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
            IDamage enemy = collision.GetComponent<TargetPoint>().Enemy;
            DamageProcess(enemy);

            ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
            effect.transform.position = transform.position;
            effect.transform.localScale = 0.2f * Vector3.one;
            effect.PlayEffect();
        }
    }


}

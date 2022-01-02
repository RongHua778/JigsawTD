using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBullet : Bullet
{
    public override BulletType BulletType => BulletType.Self;

    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        this.Target = target;
        this.TargetPos = pos ?? target.Position;
        SetAttribute(turret);
        TriggerShootEffect();
    }

    public override bool GameUpdate()
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TargetPoint>())
        {
            SputteredCount = 0;
            TriggerPreHitEffect();
            IDamageable enemy = collision.GetComponent<TargetPoint>().Enemy;
            DamageProcess(enemy);

            ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
            effect.transform.position = transform.position;
            effect.transform.localScale = 0.2f * Vector3.one;
            effect.PlayEffect();
        }
    }


}

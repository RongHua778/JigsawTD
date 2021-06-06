using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : Bullet
{
    public override BulletType BulletType => BulletType.Penetrate;
    private Vector3 initScale;
    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        base.Initialize(turret, target, pos);
        initScale = transform.localScale;
        transform.localScale = transform.localScale * (1 + 2f * SputteringRange);
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
        Enemy enemy = collision.GetComponent<TargetPoint>().Enemy;
        DealRealDamage(enemy);
    }
}

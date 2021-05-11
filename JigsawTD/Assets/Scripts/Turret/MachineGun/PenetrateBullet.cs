using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : Bullet
{
    public override BulletType BulletType => BulletType.Penetrate;

    public override bool GameUpdate()
    {
        RotateBullet(TargetPos);
        return MoveTowards(TargetPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<TargetPoint>().Enemy.ApplyDamage(damage);
        }
    }
}

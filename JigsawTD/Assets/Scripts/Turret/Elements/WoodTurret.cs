using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTurret : Turret
{
    public override void InitializeTurret()
    {
        base.InitializeTurret();
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }

    protected override void Shoot()
    {
        foreach (var target in Target)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = shootPoint.position + Random.Range(-0.05f, 0.05f) * shootPoint.right;
            Vector2 pos = (Vector2)shootPoint.position + (Vector2)shootPoint.up * AttackRange + AttackRange * 0.1f * Random.insideUnitCircle;
            bullet.Initialize(this, target, pos);
        }
    }



}

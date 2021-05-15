using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : Turret
{
    public override void InitializeTurret(GameTile tile)
    {
        base.InitializeTurret(tile);
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }


    protected override void Shoot()
    {
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = shootPoint.position;
        Vector2 pos = (Vector2)shootPoint.position + (Vector2)shootPoint.up * AttackRange;
        bullet.Initialize(this, pos);
    }
}

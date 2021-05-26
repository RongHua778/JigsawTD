using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTurret : Turret
{
    protected override void Shoot()
    {
        base.Shoot();
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = shootPoint.position;
        bullet.Initialize(this);
    }
}

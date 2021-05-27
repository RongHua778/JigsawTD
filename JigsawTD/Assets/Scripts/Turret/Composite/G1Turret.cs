using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G1Turret : Turret
{
    //同时攻击多个目标
    protected override void Shoot()
    {
        base.Shoot();
<<<<<<< Updated upstream
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = shootPoint.position;
        bullet.Initialize(this);
=======
>>>>>>> Stashed changes
    }
}

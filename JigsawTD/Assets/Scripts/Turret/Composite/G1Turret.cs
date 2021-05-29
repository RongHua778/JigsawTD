using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G1Turret : CompositeTurret
{
    private const float ShootPointSideOffset = 0.14f;
    private const float ShootInterval = 0.1f;
    [SerializeField] Transform shootPoint1=default;
    [SerializeField] Transform shootPoint2 = default;
    bool shootDir = false;
    //同时攻击多个目标
    protected override void Shoot()
    {
        turretAnim.SetTrigger("Attack");
        foreach (TargetPoint target in Target)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = shootDir ? shootPoint1.position : shootPoint2.position;
            shootDir = !shootDir;
            bullet.Initialize(this, target);
        }
    }

    IEnumerator ShootCor()
    {
        turretAnim.SetTrigger("Attack");
        foreach (TargetPoint target in Target)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            //bullet.transform.position = shootDir ? shootPoint1 : shootPoint2;
            shootDir = !shootDir;
            bullet.Initialize(this, target);
            yield return new WaitForSeconds(ShootInterval);
        }
    }

    public override void SetGraphic()
    {
        base.SetGraphic();
        shootPoint1.position = (Vector2)shootPoint.position + Vector2.right * ShootPointSideOffset;
        shootPoint2.position = (Vector2)shootPoint.position - Vector2.right * ShootPointSideOffset;

    }
}

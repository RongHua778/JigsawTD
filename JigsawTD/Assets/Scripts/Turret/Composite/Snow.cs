using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snow : RefactorTurret
{

    protected override void Shoot()
    {
        PlayAudio(ShootClip, false);
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = shootPoint.position;
        Vector2 pos;
        if (Strategy.RangeType == RangeType.Line)
        {
            pos = (Vector2)shootPoint.position + (Vector2)transform.up * Strategy.FinalRange;
        }
        else
        {
            Vector2 dir = Target[0].transform.position - transform.position;
            pos = (Vector2)shootPoint.position + dir.normalized * Strategy.FinalRange;
        }
        bullet.Initialize(this, Target[0], pos);
    }

    //protected override void Shoot()
    //{
    //    turretAnim.SetTrigger("Attack");
    //    PlayAudio(ShootClip, false);
    //    float minDis = Mathf.Infinity;
    //    RaycastHit2D hitTurret = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, Mathf.Infinity, LayerMask.GetMask(StaticData.TurretMask));
    //    RaycastHit2D hitGround = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, Mathf.Infinity, LayerMask.GetMask(StaticData.GroundTileMask));
    //    if (hitGround.collider != null)
    //    {
    //        minDis = Vector2.Distance(transform.position, hitGround.transform.position);
    //    }
    //    if (hitTurret.collider != null)
    //    {
    //        float dis2 = Vector2.Distance(transform.position, hitTurret.transform.position);
    //        if (dis2 < minDis && dis2 > Strategy.FinalRange)
    //        {
    //            minDis = dis2;
    //        }
    //    }
    //    Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
    //    bullet.transform.position = shootPoint.position;
    //    Vector2 pos = (Vector2)shootPoint.position + (Vector2)transform.up * (minDis - 1);
    //    bullet.Initialize(this, Target[0], pos);
    //}
}

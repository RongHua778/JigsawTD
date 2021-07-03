using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class T23Turret : CompositeTurret
{
    private const float ShootInterval = 0.15f;
    public override void OnSpawn()
    {
        base.OnSpawn();
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootCor());
        //foreach (var target in Target)
        //{
        //    Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        //    float offset = Random.Range(-0.02f, 0.02f);
        //    bullet.transform.position = shootPoint.position + offset * shootPoint.right;
        //    Vector2 dir = bullet.transform.position - transform.position;
        //    Vector2 pos = (Vector2)shootPoint.position + dir.normalized * AttackRange;
        //    bullet.Initialize(this, target, pos);
        //}
    }

    IEnumerator ShootCor()
    {
        foreach (TargetPoint target in Target.ToList())
        {
            if (!target.gameObject.activeSelf)
                continue;
            turretAnim.SetTrigger("Attack");
            PlayAudio(ShootClip, false);
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = shootPoint.position;
            Vector2 pos = (Vector2)shootPoint.position + (Vector2)transform.up * Strategy.FinalRange;
            bullet.Initialize(this, target, pos);
            yield return new WaitForSeconds(ShootInterval);
        }
    }
}

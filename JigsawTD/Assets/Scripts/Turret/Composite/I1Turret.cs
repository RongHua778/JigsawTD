using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I1Turret : CompositeTurret
{
    float rotSpeed = 0;
    float RotSpeed { get => Mathf.Min(720, rotSpeed * Mathf.Pow(Strategy.FinalSpeed, 2)); set => rotSpeed = value; }
    protected override void RotateTowards()
    {

    }

    public override bool GameUpdate()
    {
        base.GameUpdate();
        SelfRotateControl();
        return true;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        rotTrans.rotation = Quaternion.identity;
        RotSpeed = 10f;
        CheckAngle = 360f;
    }

    private void SelfRotateControl()
    {
        rotTrans.Rotate(Vector3.forward * RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = 0.3f * Random.insideUnitCircle.normalized + (Vector2)transform.position;
        Vector2 targetPos = (bullet.transform.position - transform.position).normalized * (Strategy.FinalRange + 0.5f) + transform.position;
        bullet.Initialize(this, null, targetPos);
    }

}

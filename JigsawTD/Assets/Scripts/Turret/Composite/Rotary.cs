using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotary : CompositeTurret
{
    float rotSpeed = 0;
    float RotSpeed { get => Mathf.Max(-720, -rotSpeed * Mathf.Pow(Strategy.FinalFireRate, 2)); set => rotSpeed = value; }

    public override void InitializeTurret()
    {
        base.InitializeTurret();
        Strategy.CheckAngle = 360f;
    }
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
        RotSpeed = -10f;
    }

    private void SelfRotateControl()
    {
        rotTrans.Rotate(Vector3.forward * -RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = 0.3f * Random.insideUnitCircle.normalized + (Vector2)transform.position;
        Vector2 targetPos = (bullet.transform.position - transform.position).normalized * (Strategy.FinalRange + 0.5f) + transform.position;
        bullet.Initialize(this, null, targetPos);
    }

}

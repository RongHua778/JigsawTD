using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : TurretContent
{
    //public override float AttackSpeed => base.AttackSpeed * (1 + RotSpeed / 180);
    float changeSpeed = 60f;
    float rotSpeed = 0;
    float RotSpeed { get => rotSpeed; set => rotSpeed = Mathf.Clamp(value, 0, 360); }

    public override GameTileContentType ContentType => throw new System.NotImplementedException();

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
        RotSpeed = 0f;
        CheckAngle = 360f;
    }

    private void SelfRotateControl()
    {
        if (Target != null)
            RotSpeed += changeSpeed * Time.deltaTime;
        else
            RotSpeed -= 2 * changeSpeed * Time.deltaTime;
        rotTrans.Rotate(Vector3.forward * RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        //Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        //bullet.transform.position = 0.3f * Random.insideUnitCircle.normalized + (Vector2)transform.position;
        //Vector2 targetPos = (bullet.transform.position - transform.position).normalized * (AttackRange + 1) + transform.position;
        //bullet.Initialize(this, targetPos);
    }

}

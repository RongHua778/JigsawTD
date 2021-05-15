using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : Turret
{
    public override float AttackSpeed => base.AttackSpeed * (1 + RotSpeed / 180);
    float changeSpeed = 60f;
    float rotSpeed = 0;
    float RotSpeed { get => rotSpeed; set => rotSpeed = Mathf.Clamp(value, 0, 360); }
    protected override void RotateTowards()
    {

    }

    public override bool GameUpdate()
    {
        base.GameUpdate();
        SelfRotateControl();
        return true;
    }

    public override void InitializeTurret(GameTile tile)
    {
        base.InitializeTurret(tile);
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
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = 0.3f * Random.insideUnitCircle.normalized + (Vector2)transform.position;
        Vector2 targetPos = (bullet.transform.position - transform.position).normalized * (AttackRange + 1) + transform.position;
        bullet.Initialize(this, targetPos);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Vector3 position = transform.position;
        position.z -= 0.1f;
        Gizmos.DrawWireSphere(position, SputteringRange);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : TurretContent
{
    public override void OnSpawn()
    {
        base.OnSpawn();
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }


    protected override void Shoot()
    {
        base.Shoot();
    }
}

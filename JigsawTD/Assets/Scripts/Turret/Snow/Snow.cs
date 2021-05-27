using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : Turret
{
    public override void InitializeTurret()
    {
        base.InitializeTurret();
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }


    protected override void Shoot()
    {
        base.Shoot();
    }
}

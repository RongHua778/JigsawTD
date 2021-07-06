using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : TurretContent
{
    public override void InitializeTurret()
    {
        base.InitializeTurret();
        Strategy.RotSpeed = 0;
        CheckAngle = 45f;
    }


    protected override void Shoot()
    {
        base.Shoot();
    }
}

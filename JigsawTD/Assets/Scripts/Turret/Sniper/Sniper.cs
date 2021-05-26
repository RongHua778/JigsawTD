using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Turret
{
    public override void InitializeTurret()
    {
        base.InitializeTurret();
        _rotSpeed = 1f;
    }
}

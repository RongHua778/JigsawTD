using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTurret : ElementTurret
{
    public override Element Element => Element.Water;

    protected override void Shoot()
    {
        base.Shoot();
    }
}

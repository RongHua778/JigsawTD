using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : ElementTurret
{
    public override Element Element => Element.Fire;

    protected override void Shoot()
    {
        base.Shoot();

    }
}

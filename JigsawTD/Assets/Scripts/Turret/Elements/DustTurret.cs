using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTurret : ElementTurret
{
    public override Element Element => Element.Dust;

    protected override void Shoot()
    {
        base.Shoot();

    }

}

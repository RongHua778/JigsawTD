using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldTurret : ElementTurret
{
    public override Element Element => Element.Gold;

    protected override void Shoot()
    {
        base.Shoot();

    }
}

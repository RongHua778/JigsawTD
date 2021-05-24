using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Turret
{
    public override void InitializeTurret(GameTile tile,int quality)
    {
        base.InitializeTurret(tile, quality);
        _rotSpeed = 1f;
    }
}

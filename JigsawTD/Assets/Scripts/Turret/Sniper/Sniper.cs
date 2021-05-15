using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Turret
{
    public override void InitializeTurret(GameTile tile)
    {
        base.InitializeTurret(tile);
        _rotSpeed = 1f;
    }
}

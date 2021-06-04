using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : TurretContent
{
    public override void OnSpawn()
    {
        base.OnSpawn();
        _rotSpeed = 1f;
    }
}

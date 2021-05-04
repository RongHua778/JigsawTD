using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTile : GameTile
{
    public override int TileLevel => 3;

    public override int TileID => 5;

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
}

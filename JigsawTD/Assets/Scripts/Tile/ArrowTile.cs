using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTile : GameTile
{
    public override int TileLevel => 2;

    public override int TileID => 2;

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
}

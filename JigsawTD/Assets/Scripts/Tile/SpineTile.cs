using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineTile : GameTile
{
    public override int TileLevel => 1;

    public override int TileID => 3;

    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameTile
{
    public override int TileLevel => 0;
    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
}

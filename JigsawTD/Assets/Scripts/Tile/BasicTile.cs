using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameTile
{
    public override BasicTileType BasicTileType => BasicTileType.None;

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
}

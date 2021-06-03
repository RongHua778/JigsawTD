using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameTile
{
    public override BasicTileType BasicTileType => BasicTileType.None;



    protected override void TileDropCheck(Collider2D col)
    {
        base.TileDropCheck(col);
        if (col != null)
            ObjectPool.Instance.UnSpawn(this.gameObject);
        else
        {
            SetGroundTile();
        }
    }
}

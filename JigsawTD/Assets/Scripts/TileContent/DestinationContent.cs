using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Destination;

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            if (tile == BoardSystem.SelectingTile)
            {
                BoardSystem.SelectingTile = null;
            }
            ObjectPool.Instance.UnSpawn(tile.gameObject);
        }
    }
}

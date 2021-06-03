using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Empty;

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            ObjectPool.Instance.UnSpawn(m_GameTile.gameObject);
        }
    }
}

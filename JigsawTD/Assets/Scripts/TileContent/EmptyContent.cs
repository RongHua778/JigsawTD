using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Empty;


    public override void ContentLanded()
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            ObjectPool.Instance.UnSpawn(m_GameTile.gameObject);
            return;
        }
        StaticData.SetNodeWalkable(m_GameTile, true);
    }
}

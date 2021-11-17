using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Empty;


    public override void ContentLanded()
    {
        base.ContentLanded();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);
    }

    protected override void SaveContent()
    {
        base.SaveContent();
        m_ContentStruct.ContentName = "Empty";
    }



    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)//底下如果有格子，则回收自身，否则将自身设置为可寻路
        {
            ObjectPool.Instance.UnSpawn(m_GameTile);
            return;
        }
        else
        {
            StaticData.SetNodeWalkable(m_GameTile, true);
        }
    }
}

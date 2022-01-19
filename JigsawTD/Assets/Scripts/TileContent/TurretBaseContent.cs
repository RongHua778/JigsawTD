using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.TurretBase;

    [HideInInspector]
    public TurretBaseAttribute m_TurretBaseAttribute;
    public bool needReset = false;//是否需要重置朝向

    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = StaticData.OnlyRefactorTag;
        StaticData.SetNodeWalkable(m_GameTile, false, false);
    }

    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        if (value)
        {
            //GameManager.Instance.ShowTurretBaseTips(this);
        }
    }

    public override void CorretRotation()
    {
        base.CorretRotation();
        if (needReset)
        {
            transform.rotation = Quaternion.identity;
        }
    }

}

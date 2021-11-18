using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.ElementTurret;


    public override void ContentLanded()
    {
        base.ContentLanded();
        GameManager.Instance.elementTurrets.Add(this);
    }

    public override void SaveContent()
    {
        base.SaveContent();
        m_ContentStruct.Element = (int)Strategy.Attribute.element;
        m_ContentStruct.Quality = Strategy.Quality;
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.TempGroundMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = true;
        }
        GameManager.Instance.elementTurrets.Remove(this);
    }
}

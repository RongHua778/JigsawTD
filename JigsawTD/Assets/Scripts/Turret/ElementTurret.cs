using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.ElementTurret;
    //����Ԫ������
    //private Element element;
    //public abstract Element Element { get; }


    public override void ContentLanded()
    {
        base.ContentLanded();
        GameManager.Instance.elementTurrets.Add(this);
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

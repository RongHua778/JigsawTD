using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class ElementTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.ElementTurret;


    public override void ContentLanded()
    {
        base.ContentLanded();
        GameManager.Instance.elementTurrets.Add(this);
        m_GameTile.tag = StaticData.UndropablePoint;
    }

    public override void SaveContent(out ContentStruct contentStruct)
    {
        base.SaveContent(out contentStruct);
        contentStruct = m_ContentStruct;
        m_ContentStruct.Element = (int)Strategy.Attribute.element;
        m_ContentStruct.Quality = Strategy.Quality;
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.TempGroundMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = true;//元素塔可能被回收，回收时，将地板TILE设为上方没有物体
        }
        GameManager.Instance.elementTurrets.Remove(this);
    }
}

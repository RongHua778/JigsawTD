using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameTile : TileBase
{
    public DraggingShape m_DraggingShape { get; set; }
    public abstract int TileID { get; }
    public abstract int TileLevel { get; }

    GameObject previewGlow;
    public Transform tileBase { get; set; }
    public Transform tileType { get; set; }
    public Turret TileTurret { get; set; }
    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }
    GameTile nextOnPath;
    public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }
    public Vector3 ExitPoint { get; set; }

    //GameTileContent content;
    //public GameTileContent Content
    //{
    //    get => content;
    //    set
    //    {
    //        Debug.Assert(value != null, "Null Content Assigned");
    //        if (content != null)
    //            content.Recycle();
    //        content = value;
    //        content.transform.localPosition = transform.localPosition;
    //    }
    //}

    private void Awake()
    {
        previewGlow = transform.Find("PreviewGlow").gameObject;
        tileBase = transform.Find("TileBase");
        tileType = transform.Find("TileType");
        TileTurret = transform.GetComponentInChildren<Turret>();
    }

    public virtual void TileDroped()
    {

    }

    public void SetPreviewing(bool isPreviewing)
    {
        if (isPreviewing)
        {
            previewGlow.SetActive(true);
        }
        else
        {
            previewGlow.SetActive(false);
        }
    }

    public virtual void OnTilePass()
    {

    }


    private void OnMouseDown()
    {
        if (m_DraggingShape != null)
        {
            m_DraggingShape.StartDragging();
        }
    }

    private void OnMouseUp()
    {
        if (m_DraggingShape != null)
        {
            m_DraggingShape.EndDragging();
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        m_DraggingShape = null;
        gameObject.layer = LayerMask.NameToLayer(StaticData.TempTileMask);
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
    }

}

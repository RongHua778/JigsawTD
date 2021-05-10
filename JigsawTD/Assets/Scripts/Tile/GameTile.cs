using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameTile : TileBase
{
    public DraggingShape m_DraggingShape { get; set; }
    //public int TileID = default;
    public int TileLevel = default;

    GameObject previewGlow;
    public Transform tileBase { get; set; }
    public Transform tileType { get; set; }
    public Turret TileTurret { get; set; }
    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }
    GameTile nextOnPath;
    public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }
    public Vector3 ExitPoint { get; set; }


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

    public virtual void OnTilePass(Enemy enemy)
    {

    }


    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (m_DraggingShape != null)
            {
                m_DraggingShape.StartDragging();
            }
            GameEvents.Instance.TileClick();
        }
    }


    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (m_DraggingShape != null)
            {
                m_DraggingShape.EndDragging();
            }
            GameEvents.Instance.TileUp(this);
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

    public void ShowTurretRange(bool show)
    {
        if (TileTurret != null)
        {
            TileTurret.ShowRange(show);
        }
    }
}

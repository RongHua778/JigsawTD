using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameTile : TileBase
{
    public abstract BasicTileType BasicTileType { get; }
    public DraggingShape m_DraggingShape { get; set; }
    //public int TileID = default;
    public int TileLevel = default;
    public GroundTile m_GroundTile;
    GameObject previewGlow;
    Transform directionCheckPoint;
    public Transform tileBase { get; set; }
    public Transform tileType { get; set; }

    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }
    GameTile nextOnPath;
    public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }

    Direction tileDirection;

    public Vector3 ExitPoint { get; set; }

    protected virtual void Awake()
    {
        previewGlow = transform.Find("PreviewGlow").gameObject;
        tileBase = transform.Find("TileBase");
        tileType = transform.Find("TileType");
        directionCheckPoint = tileType.Find("CheckPoint");
        GetTileDirection();
    }

    public Direction GetTileDirection()
    {
        tileDirection = DirectionExtensions.GetDirection(transform.position, directionCheckPoint.position);
        return tileDirection;
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
        m_GroundTile = null;
        m_DraggingShape = null;
        gameObject.layer = LayerMask.NameToLayer(StaticData.TempTileMask);
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
    }


}

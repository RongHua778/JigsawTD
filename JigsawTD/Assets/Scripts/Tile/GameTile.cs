using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameTile : TileBase
{
    public abstract BasicTileType BasicTileType { get; }
    public DraggingShape m_DraggingShape { get; set; }

    [HideInInspector] public Material BaseMaterial;

    GameObject previewGlow;
    Transform directionCheckPoint;
    public Transform tileBase { get; set; }
    public Transform tileType { get; set; }

    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }

    GameTile nextOnPath;
    public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }

    Direction tileDirection;

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            gameObject.layer = value ? LayerMask.NameToLayer(StaticData.ConcreteTileMask) : LayerMask.NameToLayer(StaticData.TempTileMask);
        }
    }

    bool previewing;
    public bool Previewing
    {
        get => previewing;
        set
        {
            previewing = value;
            previewGlow.SetActive(value);
        }
    }

    public Vector3 ExitPoint { get; set; }


    protected virtual void Awake()
    {
        previewGlow = transform.Find("PreviewGlow").gameObject;
        tileBase = transform.Find("TileBase");
        BaseMaterial = tileBase.GetComponent<SpriteRenderer>().material;
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
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        TileDropCheck(col);
        SetBackToParent();
        Previewing = false;
        IsActive = true;
    }


    protected virtual void TileDropCheck(Collider2D col)
    {

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
        m_DraggingShape = null;
        IsActive = false;
        BaseMaterial.color = Color.white;
        //Debug.Log("UNSPAWNed");
        base.OnUnSpawn();
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
    }

    protected void SetGroundTile()
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));//ÐÞ¸Ägroundtile²ã
        if (col != null)
        {
            GroundTile groundTile = col.GetComponent<GroundTile>();
            groundTile.TileAbrove = this;
            groundTile.IsActive = false;
        }
    }
}

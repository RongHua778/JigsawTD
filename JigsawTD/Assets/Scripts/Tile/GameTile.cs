using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameTile : TileBase
{
    GameObject previewGlow;
    Transform directionCheckPoint;
    Transform tileBase;

    public bool isWalkable { get => Content.IsWalkable; }
    public DraggingShape m_DraggingShape { get; set; }
    public SpriteRenderer BaseRenderer { get; set; }
    public SpriteRenderer PreviewRenderer { get; set; }
    public Vector3 ExitPoint { get; set; }



    //Direction pathDirection;
    //public Direction PathDirection { get => pathDirection; set => pathDirection = value; }

    //GameTile nextOnPath;
    //public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }

    Direction tileDirection;
    public Direction TileDirection { get => tileDirection; set => tileDirection = value; }

    public override bool IsLanded
    {
        get => base.IsLanded;
        set
        {
            base.IsLanded = value;
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



    protected virtual void Awake()
    {
        previewGlow = transform.Find("PreviewGlow").gameObject;
        PreviewRenderer = previewGlow.GetComponent<SpriteRenderer>();
        tileBase = transform.Find("TileBase");
        BaseRenderer = tileBase.GetComponent<SpriteRenderer>();
        directionCheckPoint = transform.Find("CheckPoint");
    }

    public virtual void TileLanded()//tile被放入版图时
    {
        SetBackToParent();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        StaticData.CorrectTileCoord(this);
        Previewing = false;

        Content.ContentLanded();//这个可能会回收自身
        IsLanded = true;//这个必须在CONTENTLANDED下面，否则会导致回收自己
    }

    public virtual void OnTilePass(Enemy enemy)//经过触发特殊效果
    {
        Content.OnContentPass(enemy);
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (m_DraggingShape != null)
        {
            m_DraggingShape.StartDragging();
        }
    }


    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (m_DraggingShape != null)
        {
            m_DraggingShape.EndDragging();
        }

    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        IsLanded = false;//这个必须在生成时设置,Gameboard生成tile时
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (BoardSystem.SelectingTile == this)
        {
            BoardSystem.SelectingTile = null;
        }
        ObjectPool.Instance.UnSpawn(Content);
        gameObject.tag = "Untagged";
        m_DraggingShape = null;
        BaseRenderer.color = Color.white;
        Content = null;

        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.TempGroundMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = true;
        }

    }

    public void SetRandomRotation()
    {
        int randomDir = UnityEngine.Random.Range(0, 4);
        TileDirection = DirectionExtensions.GetDirection(randomDir);
        transform.rotation = TileDirection.GetRotation();
        CorrectRotation();
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
        Content.CorretRotation();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //if(collision.GetComponent<TargetPoint>())
    //    collision.GetComponent<TargetPoint>().Object.CurrentTile = this;
    //}
}

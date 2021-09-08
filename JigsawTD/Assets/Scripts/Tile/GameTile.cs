using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameTile : TileBase
{
    private GameTileContent content;
    public GameTileContent Content
    {
        get => content;
        set => content = value;
    }

    GameObject previewGlow;
    Transform directionCheckPoint;
    Transform tileBase;

    public bool isWalkable { get => Content.IsWalkable; }
    public DraggingShape m_DraggingShape { get; set; }
    public List<SpriteRenderer> TileRenderers { get; set; }
    public SpriteRenderer PreviewRenderer { get; set; }
    public Vector3 ExitPoint { get; set; }

    //tile的朝向
    Direction tileDirection;
    public Direction TileDirection { get => tileDirection; set => tileDirection = value; }

    //格子特殊效果
    private float tileDamageIntensify = 0;
    public float TileDamageIntensify { get => tileDamageIntensify; set => tileDamageIntensify = value; }
    private float tileSlowIntensify = 0;
    public float TileSlowIntensify { get => tileSlowIntensify; set => tileSlowIntensify = value; }

    private int bounsCoin = 0;
    public int BounsCoin { get => bounsCoin; set => bounsCoin = value; }

    private float trapIntensify = 1;
    public float TrapIntensify { get => trapIntensify; set => trapIntensify = value; }



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
        directionCheckPoint = transform.Find("CheckPoint");
    }

    public void SetTileColor(Color colorToSet)
    {
        foreach(var sr in TileRenderers)
        {
            sr.color = colorToSet;
        }
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

    public virtual void SetContent(GameTileContent content)
    {
        content.transform.SetParent(this.transform);
        content.transform.position = transform.position + Vector3.forward * 0.01f;
        content.m_GameTile = this;
        Content = content;
    }

    public virtual void OnTilePass(Enemy enemy)//经过触发特殊效果
    {
        Content.OnContentPass(enemy);
    }
    public virtual void OnTileLeave(Enemy enemy)
    {

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
        SetTileColor(Color.white);
        Content = null;

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

}

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
    Transform tileBase;
    [SerializeField] GameObject hightLightEffect = default;

    public bool isWalkable { get => Content.IsWalkable; }
    public DraggingShape m_DraggingShape { get; set; }
    public List<SpriteRenderer> TileRenderers { get; set; }
    public SpriteRenderer PreviewRenderer { get; set; }
    public Vector3 ExitPoint { get; set; }

    //tile的朝向
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
    }

    public void SetTileColor(Color colorToSet)
    {
        foreach (var sr in TileRenderers)
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
        GetTileDirection();

        Content.ContentLanded();//这个可能会回收自身
        IsLanded = true;//这个必须在CONTENTLANDED下面，否则会导致回收自己
    }

    public virtual void SetContent(GameTileContent content)
    {
        content.transform.SetParent(this.transform);
        content.transform.position = transform.position + Vector3.forward * 0.01f;
        content.transform.localRotation = Quaternion.identity;
        content.m_GameTile = this;
        Content = content;
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
        SetTileColor(Color.white);
        Content = null;
        transform.rotation = Quaternion.identity;
    }

    public void SetRandomRotation(int dir = -1)
    {
        int randomDir = dir == -1 ? UnityEngine.Random.Range(0, 4) : dir;
        transform.rotation = DirectionExtensions.GetDirection(randomDir).GetRotation();
        CorrectRotation();
    }

    public void SetRotation(int direction)
    {
        transform.rotation = DirectionExtensions.GetDirection(direction).GetRotation();
        CorrectRotation();
    }

    private void GetTileDirection()
    {
        TileDirection = DirectionExtensions.GetDirection(transform.position, transform.position + transform.up);
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
        Content.CorretRotation();
    }
    public void Highlight(bool value)
    {
        hightLightEffect.SetActive(value);
    }

}

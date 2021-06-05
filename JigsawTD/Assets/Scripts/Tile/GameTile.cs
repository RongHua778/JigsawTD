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
    public DraggingShape m_DraggingShape { get; set; }
    public SpriteRenderer BaseRenderer { get; set; }
    public Vector3 ExitPoint { get; set; }

    private GameTileContent content;
    public GameTileContent Content
    {
        get => content;
        set => content = value;
    }

    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }

    GameTile nextOnPath;
    public GameTile NextTileOnPath { get => nextOnPath; set => nextOnPath = value; }

    Direction tileDirection;

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
        tileBase = transform.Find("TileBase");
        BaseRenderer = tileBase.GetComponent<SpriteRenderer>();
        directionCheckPoint = transform.Find("CheckPoint");
    }

    public Direction GetTileDirection()
    {
        tileDirection = DirectionExtensions.GetDirection(transform.position, directionCheckPoint.position);
        return tileDirection;
    }

    public virtual void TileLanded()//tile�������ͼʱ
    {
        SetBackToParent();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Previewing = false;
        Content.ContentLanded();//������ܻ��������
        IsLanded = true;//���������CONTENTLANDED���棬����ᵼ�»����Լ�
    }

    public virtual void OnTilePass(Enemy enemy)//������������Ч��
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

    public override void OnSpawn()
    {
        base.OnSpawn();
        IsLanded = false;//�������������ʱ����
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (BoardSystem.SelectingTile == this)
        {
            BoardSystem.SelectingTile = null;
        }
        ObjectPool.Instance.UnSpawn(Content.gameObject);
        gameObject.tag = "Untagged";
        m_DraggingShape = null;
        BaseRenderer.color = Color.white;
        Content = null;
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
        Content.CorretRotation();
    }

    protected void SetGroundTile()
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));//�޸�groundtile��
        if (col != null)
        {
            GroundTile groundTile = col.GetComponent<GroundTile>();
            groundTile.TileAbrove = this;
            groundTile.IsLanded = false;
        }
    }

    public void SetContent(GameTileContent content)
    {
        content.transform.SetParent(this.transform);
        content.transform.position = transform.position;
        content.m_GameTile = this;
        Content = content;
    }


}

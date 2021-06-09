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

    private GameTileContent content;
    public GameTileContent Content
    {
        get => content;
        set => content = value;
    }

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

    public virtual void TileLanded()//tile�������ͼʱ
    {
        SetBackToParent();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        StaticData.CorrectTileCoord(this);
        Previewing = false;

        Content.ContentLanded();//������ܻ��������
        IsLanded = true;//���������CONTENTLANDED���棬����ᵼ�»����Լ�
    }

    public virtual void OnTilePass(Enemy enemy)//������������Ч��
    {
        Content.OnContentPass(enemy);
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
        IsLanded = false;//�������������ʱ����,Gameboard����tileʱ
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


    public void SetContent(GameTileContent content)
    {
        content.transform.SetParent(this.transform);
        content.transform.position = transform.position;
        content.m_GameTile = this;
        Content = content;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<TargetPoint>().Enemy.CurrentTile = this;
    }
}

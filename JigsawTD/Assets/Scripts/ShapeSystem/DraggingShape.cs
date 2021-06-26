using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingShape : DraggingActions
{
    Vector2 lastPos;
    Transform menuTrans;
    TileShape tileShape;
    public TileShape TileShape { get => tileShape; set => tileShape = value; }

    bool canDrop = false;
    bool overLapPoint = false;
    bool waitingForPath = false;

    Collider2D[] attachedResult = new Collider2D[10];

    [SerializeField]
    Color wrongColor, correctColor, transparentColor, holdColor, dropColor = default;

    public void Initialized(TileShape shape)
    {
        menuTrans = transform.Find("DragMenu");
        TileShape = shape;
    }

    private void SetAllColor(Color colorToSet)//��ͼЧ������
    {
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.BaseRenderer.color = colorToSet;
        }
    }

    private void SetPreviewColor(Color colorToSet)//�ⷢ��Ч������
    {
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.PreviewRenderer.color = colorToSet;
        }
    }

    private void SetTileColor(Color colorToSet, GameTile tile)
    {
        tile.BaseRenderer.color = colorToSet;
    }

    protected override void Update()
    {
        base.Update();
        if (tileShape.IsPreviewing && Input.GetKeyDown(KeyCode.R))
        {
            RotateShape();
        }

    }



    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        Vector3 mousePos = MouseInWorldCoords();
        transform.position = new Vector3(Mathf.Round(mousePos.x + pointerOffset.x), Mathf.Round(mousePos.y + pointerOffset.y), transform.position.z);
        if ((Vector2)transform.position != lastPos)
        {

            if (CheckCanDrop())
            {
                StopAllCoroutines();
                StartCoroutine(TryFindPath());
            }
        }
        lastPos = transform.position;

    }


    public void ShapeSpawned()//����ģ��󣬼��һ�¿ɷ���ú�Ѱ·
    {
        if (CheckCanDrop())
        {
            StartCoroutine(TryFindPath());
        }
    }

    private bool CheckCanDrop()
    {
        canDrop = true;
        Physics2D.SyncTransforms();
        //CheckAttached();
        CheckOverLap();
        CheckMapEdge();
        if (!canDrop)
        {
            SetAllColor(wrongColor);
            return false;
        }
        return true;
    }
    private void CheckMapEdge()
    {
        Vector2Int groundSize = BoardSystem._groundSize;
        int maxX = (groundSize.x - 1) / 2;
        int minX = -(groundSize.x - 1) / 2;
        int maxY = (groundSize.y - 1) / 2;
        int minY = -(groundSize.y - 1) / 2;
        foreach (GameTile tile in TileShape.tiles)
        {
            if (tile.transform.position.x > maxX ||
                tile.transform.position.x < minX ||
                tile.transform.position.y > maxY ||
                tile.transform.position.y < minY)
            {
                canDrop = false;
                break;
            }
        }
    }
    private void CheckAttached()//����Ƿ�����
    {
        int hits;
        foreach (GameTile tile in TileShape.tiles)
        {
            Vector2 pos = tile.transform.position;
            hits = Physics2D.OverlapCircleNonAlloc(pos, 0.51f, attachedResult, LayerMask.GetMask(StaticData.ConcreteTileMask));
            if (hits > 0)
            {
                canDrop = true;
                break;
            }
        }
    }
    private void CheckOverLap()
    {
        foreach (var tile in TileShape.tiles)
        {
            Collider2D col = StaticData.RaycastCollider(tile.transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
            if (col == null)
            {
                SetTileColor(correctColor, tile);//û���²�TILE����Ϊ����
                continue;
            }
            if (tile.Content.ContentType != GameTileContentType.Empty)//������з������ģ��ͱȶԳ�ͻ
            {
                if (col.CompareTag("UnDropablePoint"))//��ͻ�����أ�������ɫ����Ϊ��ɫ
                {
                    canDrop = false;
                    overLapPoint = true;
                    break;
                }
                else
                {
                    SetTileColor(correctColor, tile);//����ͻ������ɫ
                }
            }
            else
            {
                SetTileColor(transparentColor, tile);//����TILE����Ϊ͸��
            }
        }

    }

    private IEnumerator TryFindPath()
    {
        waitingForPath = true;
        yield return new WaitForSeconds(0.1f);
        Sound.Instance.PlayEffect("Sound_Shape");
        ChangeAstarPath();
        //yield return new WaitForSeconds(0.05f);
        GameEvents.Instance.SeekPath();
        yield return new WaitForSeconds(0.1f);
        waitingForPath = false;
    }
    List<GridNodeBase> ChangeNodes = new List<GridNodeBase>();

    private void ChangeAstarPath()
    {
        var grid = AstarPath.active.data.gridGraph;
        foreach (var node in ChangeNodes)
        {
            node.Walkable = !node.Walkable;
            grid.CalculateConnectionsForCellAndNeighbours(node.XCoordinateInGrid, node.ZCoordinateInGrid);
        }
        ChangeNodes.Clear();
        foreach (var tile in TileShape.tiles)
        {
            StaticData.CorrectTileCoord(tile);

            AstarPath.active.AddWorkItem(ctx =>
            {
                int x = tile.OffsetCoord.x;
                int z = tile.OffsetCoord.y;

                GridNodeBase node = grid.nodes[z * grid.width + x];

                if (!node.ChangeAbleNode)
                    return;
                if (node.Walkable != tile.isWalkable)
                {
                    node.Walkable = !node.Walkable;
                    ChangeNodes.Add(node);
                    grid.CalculateConnectionsForCellAndNeighbours(x, z);
                }
            });
        }
    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.CorrectRotation();
        }
        if (CheckCanDrop())
        {
            StopAllCoroutines();
            StartCoroutine(TryFindPath());
            SetPreviewColor(dropColor);
        }

    }


    public void ConfirmShape()
    {
        if (waitingForPath)
        {
            GameManager.Instance.ShowMessage("����̫����");
            return;
        }
        if (canDrop)
        {
            if (!BoardSystem.FindPath)
            {
                GameManager.Instance.ShowMessage("�����е�·���������յ�");
                return;
            }
            Sound.Instance.PlayEffect("Sound_ConfirmShape");
            // EnableGroundColliders();
            foreach (GameTile tile in TileShape.tiles)
            {
                tile.TileLanded();
            }
            GameManager.Instance.ConfirmShape();
            GameManager.Instance.TriggerGuide(4);
            Destroy(this.gameObject);
        }
        else if (overLapPoint)
        {
            GameManager.Instance.ShowMessage("��������������������ص�");
        }
        else
        {
            GameManager.Instance.ShowMessage("������������������");
        }
    }


    public void StartDragging()
    {
        isDragging = true;
        DraggingThis = this;
        pointerOffset = transform.position - MouseInWorldCoords();
        pointerOffset.z = 0;
        SetPreviewColor(holdColor);
        OnStartDrag();
    }

    public void EndDragging()
    {
        if (isDragging)
        {
            if(CheckCanDrop())
                SetPreviewColor(dropColor);
            else
                SetPreviewColor(wrongColor);
            isDragging = false;
            DraggingThis = null;
            OnEndDrag();
        }
    }

}

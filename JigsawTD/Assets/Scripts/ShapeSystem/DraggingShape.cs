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
    List<Collider2D> groundColliders = new List<Collider2D>();

    [SerializeField]
    Color wrongColor, correctColor, transparentColor = default;

    public void Initialized(TileShape shape)
    {
        menuTrans = transform.Find("DragMenu");
        TileShape = shape;
    }

    private void SetAllColor(Color colorToSet)
    {
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.BaseRenderer.color = colorToSet;
        }
    }

    private void SetTileColor(Color colorToSet, GameTile tile)
    {
        tile.BaseRenderer.color = colorToSet;
    }

    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        Vector3 mousePos = MouseInWorldCoords();
        transform.position = new Vector3(Mathf.Round(mousePos.x + pointerOffset.x), Mathf.Round(mousePos.y + pointerOffset.y), transform.position.z);
        if ((Vector2)transform.position != lastPos)
        {
            CheckCanDrop();
            StopAllCoroutines();
            StartCoroutine(TryFindPath());
        }
        lastPos = transform.position;
    }

    public void ShapeSpawned()//����ģ��󣬼��һ�¿ɷ���ú�Ѱ·
    {
        CheckCanDrop();
        StartCoroutine(TryFindPath());
    }

    private void SetGroundForPathFinding()
    {
        Physics2D.SyncTransforms();
        EnableGroundColliders();
        foreach (GameTile tile in TileShape.tiles)
        {
            Collider2D colHit = StaticData.RaycastCollider(tile.transform.position, LayerMask.GetMask(StaticData.GroundTileMask));
            if (colHit != null)
                groundColliders.Add(colHit);
        }
        foreach (var groundTile in groundColliders)
        {
            groundTile.enabled = false;
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
        else
        {
            //SetAllColor(correctColor);
            return true;
        }

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
        foreach (GameTile tile in TileShape.tiles)
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
        SetGroundForPathFinding();

        GameEvents.Instance.SeekPath();
        yield return new WaitForSeconds(0.1f);
        waitingForPath = false;
    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.CorrectRotation();
        }
        CheckCanDrop();
        StopAllCoroutines();
        StartCoroutine(TryFindPath());
    }


    public void ConfirmShape()
    {
        if (waitingForPath)
        {
            GameEvents.Instance.Message("����̫����");
            return;
        }
        if (canDrop)
        {
            if (!BoardSystem.FindPath)
            {
                GameEvents.Instance.Message("�����е�·���������յ�");
                return;
            }
            Sound.Instance.PlayEffect("Sound_ConfirmShape");
            EnableGroundColliders();
            GameEvents.Instance.ConfirmShape();
            foreach (GameTile tile in TileShape.tiles)
            {
                tile.TileLanded();
            }
            GameEvents.Instance.CheckBluePrint();

            StaticData.holdingShape = null;
            Destroy(this.gameObject);
        }
        else if (overLapPoint)
        {
            GameEvents.Instance.Message("��������������������ص�");
        }
        else
        {
            GameEvents.Instance.Message("������������������");
        }
    }

    private void EnableGroundColliders()
    {
        foreach (var groundCol in groundColliders)
        {
            groundCol.enabled = true;
        }
        groundColliders.Clear();
    }

    public void StartDragging()
    {
        isDragging = true;
        DraggingThis = this;
        pointerOffset = transform.position - MouseInWorldCoords();
        pointerOffset.z = 0;
        OnStartDrag();
    }

    public void EndDragging()
    {
        if (isDragging)
        {
            isDragging = false;
            DraggingThis = null;
            OnEndDrag();
        }
    }

}

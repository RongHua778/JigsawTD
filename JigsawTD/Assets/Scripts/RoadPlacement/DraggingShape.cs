using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingShape : DraggingActions
{
    Vector3 lastPos;
    Transform menuTrans;
    TileShape tileShape;

    bool canDrop = false;
    bool overLapPoint = false;
    bool waitingForPath = false;

    Collider2D[] attachedResult = new Collider2D[10];

    List<Material> tileMaterials = new List<Material>();
    List<Collider2D> detectCollider = new List<Collider2D>();
    List<Collider2D> groundTileList = new List<Collider2D>();


    [SerializeField]
    Color wrongColor, correctColor, transparentColor = default;

    Collider2D turretCollider;
    public Collider2D TurretCollider { get => turretCollider; set => turretCollider = value; }

    public void Initialized()
    {
        menuTrans = transform.Find("DragMenu");
        tileShape = this.GetComponent<TileShape>();
        foreach (GameTile tile in tileShape.tiles)
        {
            tileMaterials.Add(tile.tileBase.GetComponent<SpriteRenderer>().material);
            detectCollider.Add(tile.GetComponent<Collider2D>());
        }
    }

    private void SetAllColor(Color colorToSet)
    {
        foreach (Material mat in tileMaterials)
        {
            mat.color = colorToSet;
        }
    }

    private void SetTileColor(Color colorToSet, GameTile tile)
    {
        tile.tileBase.GetComponent<SpriteRenderer>().material.color = colorToSet;
    }

    private void SetTransparent()//设为透明
    {
        foreach (GameTile tile in tileShape.tiles)
        {
            Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
            GameTile t = StaticData.Instance.GetTile(pos);
            if (t != null)
            {
                if (t.GetComponentInParent<TurretTile>() || t.GetComponentInParent<TrapTile>())
                {
                    SetTileColor(transparentColor, tile);
                }
            }
        }
    }
    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        Vector3 mousePos = MouseInWorldCoords();
        transform.position = new Vector3(Mathf.Round(mousePos.x + pointerOffset.x), Mathf.Round(mousePos.y + pointerOffset.y), transform.position.z);
        if (transform.position != lastPos)
        {
            CheckCanDrop();
            StopAllCoroutines();
            StartCoroutine(TryFindPath());
        }
        lastPos = transform.position;
    }

    public void ShapeSpawned()//生成模块后，检查一下可否放置和寻路
    {
        CheckCanDrop();
        StartCoroutine(TryFindPath());
    }

    private void SetGroundForPathFinding()
    {
        Physics2D.SyncTransforms();
        EnableGroundColliders();
        RaycastHit2D hitInfo;
        foreach (Collider2D col in detectCollider)
        {
            hitInfo = Physics2D.Raycast(col.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask(StaticData.GroundTileMask));
            if (hitInfo.collider != null)
                groundTileList.Add(hitInfo.collider);
        }
        foreach (var groundTile in groundTileList)
        {
            groundTile.enabled = false;
        }

    }
    private bool CheckCanDrop()
    {
        canDrop = false;
        Physics2D.SyncTransforms();
        CheckAttached();
        CheckOverLap();

        //canDrop = CheckTrapDroppable();
        //canDrop = CheckMapEdge();
        if (!canDrop)
        {
            SetAllColor(wrongColor);
            return false;
        }
        else
        {
            SetAllColor(correctColor);
            SetTransparent();
            return true;
        }

    }
    private bool CheckMapEdge()
    {
        int maxX = (GameManager.Instance.GroundSize.x - 1) / 2;
        int minX = -(GameManager.Instance.GroundSize.x - 1) / 2;
        int maxY = (GameManager.Instance.GroundSize.y - 1) / 2;
        int minY = -(GameManager.Instance.GroundSize.y - 1) / 2;
        foreach (GameTile tile in tileShape.tiles)
        {
            if (tile.transform.position.x > maxX ||
                tile.transform.position.x < minX ||
                tile.transform.position.y > maxY ||
                tile.transform.position.y < minY)
            {
                return false;
            }
        }
        return canDrop;
    }
    private void CheckAttached()//检查是否相连
    {
        int hits;
        foreach (Collider2D col in detectCollider)
        {
            Vector2 pos = col.transform.position;
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
        Vector2 pos = TurretCollider.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity, LayerMask.GetMask(StaticData.ConcreteTileMask));
        if (hit.collider != null && hit.collider.CompareTag("UnDropablePoint"))
        {
            canDrop = false;
            overLapPoint = true;
        }
    }

    private IEnumerator TryFindPath()
    {
        waitingForPath = true;
        yield return new WaitForSeconds(0.1f);
        SetGroundForPathFinding();
        //Debug.Log("Try FindPath");
        GameEvents.Instance.SeekPath();
        yield return new WaitForSeconds(0.1f);
        waitingForPath = false;
    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
        foreach (GameTile tile in tileShape.tiles)
        {
            tile.CorrectRotation();
        }
        CheckCanDrop();
        StopAllCoroutines();
        StartCoroutine(TryFindPath());
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
        menuTrans.rotation = Quaternion.identity;
    }

    public void ConfirmShape()
    {
        if (waitingForPath)
        {
            GameEvents.Instance.Message("你点的太快了，道路判定中>>>");
            return;
        }
        if (canDrop)
        {
            if (!BoardSystem.FindPath)
            {
                GameEvents.Instance.Message("必须有道路连接起点和终点");
                return;
            }
            Sound.Instance.PlayEffect("Sound_ConfirmShape");
            EnableGroundColliders();
            GameEvents.Instance.AddTiles(tileShape.tiles);
            SetTrapActived();
            StaticData.holdingShape = null;
            Destroy(this.gameObject);
        }
        else if (overLapPoint)
        {
            GameEvents.Instance.Message("不可覆盖起点或终点");
        }
        else
        {
            GameEvents.Instance.Message("必须覆盖或与已有区域相连");
        }
    }
    //取消选择当前模块，返回模块选择界面
    private void SetTrapActived()
    {
        foreach (Collider2D col in detectCollider)
        {
            Vector3 pos = new Vector3(col.transform.position.x, col.transform.position.y, 0);
            int hits = Physics2D.OverlapCircleNonAlloc(pos, 0.51f, attachedResult, LayerMask.GetMask(StaticData.TrapTileMask));
            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {

                    attachedResult[i].gameObject.GetComponent<GameTile>().Actived = true;
                }
            }
        }
    }
    private void EnableGroundColliders()
    {
        if (groundTileList.Count > 0)
        {
            foreach (var groundTile in groundTileList)
            {
                groundTile.enabled = true;
            }
            groundTileList.Clear();
        }
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

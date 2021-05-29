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
    Collider2D[] collideResult = new Collider2D[10];
    Collider2D[] collideTurretResult = new Collider2D[10];


    List<Material> tileMaterials = new List<Material>();
    List<Collider2D> detectCollider = new List<Collider2D>();
    List<Collider2D> groundTileList = new List<Collider2D>();

    [SerializeField]
    LayerMask CheckDropLayer = default;
    [SerializeField]
    LayerMask CheckTurretLayer = default;
    [SerializeField]
    LayerMask TrapLayer = default;

    [SerializeField]
    Color wrongColor, correctColor = default;

    Collider2D turretCollider;
    Color draggingColor = new Color(1, 1, 1, 0.3f);


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

    private void SetColor(Color colorToSet)
    {
        foreach (Material mat in tileMaterials)
        {
            mat.color = colorToSet;
        }
    }
    private void SetColor(Color colorToSet,GameTile tile)
    {
        tile.tileBase.GetComponent<SpriteRenderer>().material.color = colorToSet;
    }
    private void DraggingColor()
    {
        foreach (GameTile tile in tileShape.tiles)
        {
            Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
            GameTile t = StaticData.Instance.GetTile(pos);
            if (t != null)
            {
               // Debug.LogWarning("*********");
                if (t.GetComponentInParent<TurretTile>() || t.GetComponentInParent<TrapTile>())
                {
                    //Debug.LogWarning("hahahhaahhah");
                    SetColor(draggingColor, tile);
                }
                if (t.GetComponentInParent<BasicTile>())
                {
                   // Debug.LogWarning("菜鸡");

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
            hitInfo = Physics2D.Raycast(col.transform.position, Vector3.forward, Mathf.Infinity, StaticData.RunTimeFindPathLayer);
            if (hitInfo.collider != null)
                groundTileList.Add(hitInfo.collider);
        }
        foreach (var groundTile in groundTileList)
        {
            groundTile.enabled = false;
            //groundTile.gameObject.layer=LayerMask.NameToLayer(StaticData.TempGroundMask);
        }

    }
    private bool CheckCanDrop()
    {
        canDrop = false;
        Physics2D.SyncTransforms();
        canDrop = CheckAttachedDroppable();
        canDrop = CheckTurretDroppable();
        canDrop = CheckTrapDroppable();
        if (!canDrop)
        {
            SetColor(wrongColor);
            return false;
        }
        else
        {
            SetColor(correctColor);
            DraggingColor();
            return true;
        }

    }
    private bool CheckAttachedDroppable()
    {
        bool result=false;
        int hits;
        foreach (Collider2D col in detectCollider)
        {
            Vector3 pos = new Vector3(col.transform.position.x, col.transform.position.y, 0);
            hits = Physics2D.OverlapCircleNonAlloc(pos, 0.51f, collideResult, CheckDropLayer);
            if (hits > 0)
            {
                overLapPoint = false;
                result = true;
                //CheckDropLayer不包括陷阱和塔
                for (int i = 0; i < hits; i++)
                {
                    if (collideResult[i].CompareTag("UnDropablePoint"))
                    {
                        if (col.OverlapPoint(collideResult[i].transform.position))
                        {
                            result = false;
                            overLapPoint = true;
                            SetColor(wrongColor);
                            return result;
                        }
                    }
                }
            }
        }
        return result;
    }
    private bool CheckTurretDroppable()
    {
        Vector3 posTurret = new Vector3(turretCollider.transform.position.x, turretCollider.transform.position.y, 0);
        int hits = Physics2D.OverlapBoxNonAlloc(posTurret, new Vector2(0.01f, 0.01f), 0, collideTurretResult, CheckDropLayer);
        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                if (collideTurretResult[i].CompareTag("UnDropableTurret"))
                {
                    if (turretCollider.OverlapPoint(collideTurretResult[i].transform.position))
                    {
                        canDrop = false;
                        overLapPoint = true;
                        SetColor(wrongColor);
                        return canDrop;
                    }
                }
            }
        }
        return canDrop;
    }
    private bool CheckTrapDroppable()
    {
        Vector3 posTurret = new Vector3(turretCollider.transform.position.x, turretCollider.transform.position.y, 0);
        int hits = Physics2D.OverlapBoxNonAlloc(posTurret, new Vector2(0.01f, 0.01f), 0, collideTurretResult, TrapLayer);
        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                    if (turretCollider.OverlapPoint(collideTurretResult[i].transform.position))
                    {
                        canDrop = false;
                        overLapPoint = true;
                        SetColor(wrongColor);
                        return canDrop;
                    }
            }
        }
        return canDrop;
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
        foreach(GameTile tile in tileShape.tiles)
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
            if (!GameBoard.FindPath)
            {
                GameEvents.Instance.Message("必须有道路连接起点和终点");
                return;
            }
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
            int hits = Physics2D.OverlapCircleNonAlloc(pos, 0.51f, collideResult, TrapLayer);
            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    collideResult[i].gameObject.GetComponent<TrapTile>().Actived = true;
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

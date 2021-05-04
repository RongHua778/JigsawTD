using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingShape : DraggingActions
{
    Vector3 lastPos;
    Transform menuTrans;
    TileShape tileShape;
    bool CanDrop = false;
    bool OverLapPoint = false;
    Collider2D[] collideResult = new Collider2D[10];

    List<Material> tileMaterials = new List<Material>();
    List<Collider2D> detectCollider = new List<Collider2D>();
    List<Collider2D> groundTileList = new List<Collider2D>();

    [SerializeField]
    LayerMask CheckDropLayer = default;

    [SerializeField]
    Color wrongColor, correctColor = default;

    public void Initialized()
    {
        menuTrans = transform.Find("DragMenu");
        tileShape = this.GetComponent<TileShape>();
        foreach (GameTile tile in tileShape.tiles)
        {
            tileMaterials.Add(tile.GetComponent<SpriteRenderer>().material);
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


    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        Vector3 mousePos = MouseInWorldCoords();
        transform.position = new Vector3(Mathf.Round(mousePos.x + pointerOffset.x), Mathf.Round(mousePos.y + pointerOffset.z), transform.position.z);
        if (transform.position != lastPos)
        {
            CheckCanDrop();
            StopAllCoroutines();
            StartCoroutine(TryFindPath());
        }
        lastPos = transform.position;
    }

    private void SetGroundForPathFinding()
    {
        Physics2D.SyncTransforms();
        if (groundTileList.Count > 0)
        {
            foreach (var groundTile in groundTileList)
            {
                groundTile.enabled = true;
            }
            groundTileList.Clear();
        }
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
        Physics2D.SyncTransforms();
        CanDrop = false;
        int hits;
        foreach (Collider2D col in detectCollider)
        {
            Vector3 pos = new Vector3(col.transform.position.x, col.transform.position.y, 0);
            hits = Physics2D.OverlapCircleNonAlloc(pos, 0.51f, collideResult, CheckDropLayer);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    if (collideResult[i].CompareTag("UnDropablePoint"))
                    {
                        if (col.OverlapPoint(collideResult[i].transform.position))
                        {
                            CanDrop = false;
                            OverLapPoint = true;
                            SetColor(wrongColor);
                            return false;
                        }
                    }
                }
                OverLapPoint = false;
                CanDrop = true;
            }
        }
        if (!CanDrop)
        {
            SetColor(wrongColor);
            return false;
        }
        else
        {
            SetColor(correctColor);
            return true;
        }

    }


    private IEnumerator TryFindPath()
    {
        yield return new WaitForSeconds(0.1f);
        SetGroundForPathFinding();
        Debug.Log("Try FindPath");
        GameEvents.Instance.SeekPath();
    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
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
        if (CanDrop)
        {
            GameEvents.Instance.AddTiles(tileShape.tiles);
            StaticData.holdingShape = null;
            Destroy(this.gameObject);
        }
        else if (OverLapPoint)
        {
            GameEvents.Instance.Message("不可覆盖起点或终点");
        }
        else
        {
            GameEvents.Instance.Message("必须覆盖或与已有区域相连");
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

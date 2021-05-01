using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingShape : DraggingActions
{
    GroundTile lastTile;
    TileShape tileShape;
    List<Material> tileMaterials;
    //List<Collider2D> detectColliders;
    Collider2D detectCollider;

    Collider2D[] collideResult = new Collider2D[10];
    ContactFilter2D filter;

    [SerializeField]
    LayerMask GameTileLayer = default;

    [SerializeField]
    Color wrongColor, correctColor = default;

    [SerializeField]
    Transform menuTrans = default;

    private bool CanDrop = false;

    protected override void Awake()
    {
        base.Awake();
        tileShape = this.GetComponent<TileShape>();
        tileMaterials = new List<Material>();
        detectCollider = this.GetComponent<Collider2D>();

        foreach (GameTile tile in tileShape.tiles)
        {
            tileMaterials.Add(tile.GetComponent<SpriteRenderer>().material);
        }
        filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(GameTileLayer);
        filter.useLayerMask = true;
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
        PosCheck();
        CheckCollid();
    }
    private void CheckCollid()
    {

        int hit = Physics2D.OverlapCollider(detectCollider, filter, collideResult);//根据layer判断是否与Gametile重合
        CanDrop = false;
        for (int i = 0; i < hit; i++)
        {
            if (collideResult[i].CompareTag("UnDropablePoint"))
            {
                CanDrop = false;
                break;
            }
            else
            {
                CanDrop = true;
            }
        }
        if (!CanDrop)
        {
            SetColor(wrongColor);
        }
        else
        {
            SetColor(correctColor);
        }
    }

    private void PosCheck()
    {
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(MouseInWorldCoords() + pointerOffset, Vector3.forward, Mathf.Infinity);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Tile"))
            {
                GroundTile tileBase = hit.collider.GetComponent<GroundTile>();

                if (lastTile != tileBase||lastTile==null)
                {
                    lastTile = tileBase;
                    StopAllCoroutines();
                    StartCoroutine(TryFindPath());
                    Debug.Log("TileChange");
                }
                transform.position = hit.transform.position + Vector3.back;
            }
        }
    }

    private IEnumerator TryFindPath()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Try FindPath");
        GameEvents.Instance.SeekPath();
    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
        Physics2D.SyncTransforms();
        CheckCollid();
    }

    public void ConfirmShape()
    {
        if (CanDrop)
        {
            GameEvents.Instance.AddTiles(tileShape.tiles);
            GameManager.holdingShape = null;
            Destroy(this.gameObject);
        }
        else
            GameEvents.Instance.Message("必须与原区块相连");

    }


}

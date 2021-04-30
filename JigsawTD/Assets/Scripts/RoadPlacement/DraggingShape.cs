using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingShape : DraggingActions
{

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
        CheckCollid();
    }
    private void CheckCollid()
    {

        int hit = Physics2D.OverlapCollider(detectCollider, filter, collideResult);
        CanDrop = false;
        for (int i = 0; i < hit; i++)
        {
            if (collideResult[i].CompareTag("GameTile"))
            {
                CanDrop = false;
                break;
            }
            if (collideResult[i].CompareTag("TempTile"))
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

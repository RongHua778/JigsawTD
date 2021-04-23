using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingShape : DraggingActions
{
    List<Material> tileMaterials;
    List<Collider2D> detectColliders;

    Collider2D[] collideResult = new Collider2D[10];
    ContactFilter2D filter;

    [SerializeField]
    LayerMask GameTileLayer;

    [SerializeField]
    Color wrongColor, correctColor = default;

    [SerializeField]
    Transform menuTrans = default;

    private void Start()
    {
        tileMaterials = new List<Material>();
        detectColliders = new List<Collider2D>();
        foreach (GameTile tile in transform.GetComponentsInChildren<GameTile>())
        {
            detectColliders.Add(tile.GetComponent<Collider2D>());
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
        int hit = 0;
        foreach(Collider2D collider in detectColliders)
        {
            hit += Physics2D.OverlapCollider(collider, filter, collideResult);

        }
        if (hit > 0)
        {
            SetColor(wrongColor);
        }
        else
        {
            SetColor(correctColor);
        }
    }

    public void ConfirmShape()
    {

    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
        Physics2D.SyncTransforms();
        CheckCollid();
    }


}

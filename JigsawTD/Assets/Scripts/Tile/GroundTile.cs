using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GroundTile : TileBase
{
    public override bool IsLanded
    {
        get => base.IsLanded;
        set
        {
            base.IsLanded = value;
            gameObject.layer = value ? LayerMask.NameToLayer(StaticData.GroundTileMask) : LayerMask.NameToLayer(StaticData.TempGroundMask);
        }
    }

    public GameTile TileAbrove;

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
}

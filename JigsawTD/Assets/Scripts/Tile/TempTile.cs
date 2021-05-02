using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTile : TileBase
{
    public override void OnSpawn()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }

    public override void OnUnSpawn()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTile : GameTile
{
    [SerializeField] GameObject Turret = default;

    public override void TileDroped()
    {
        base.TileDroped();
        Turret.layer = LayerMask.NameToLayer(StaticData.GroundTileMask);
    }
    public override void OnSpawn()
    {
        TileTurret.InitializeTurret();
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        TileTurret.RecycleRanges();
        Turret.layer = LayerMask.NameToLayer(StaticData.TurretMask);
        Turret.GetComponent<Collider2D>().enabled = true;//������ʱ���ᱻ���߼��disable��groundtile���Collider
    }
}

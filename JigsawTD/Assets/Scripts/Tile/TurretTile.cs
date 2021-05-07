using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTile : GameTile
{
    public override int TileLevel => 1;

    public override int TileID => 1;
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
        Turret.GetComponent<Collider2D>().enabled = true;//被覆盖时，会被射线检测disable掉groundtile层的Collider
    }
}

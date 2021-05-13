using UnityEngine;

public class TurretTile : GameTile
{
    [SerializeField] GameObject Turret = default;
    public Turret TileTurret { get; set; }

    public override BasicTileType BasicTileType => BasicTileType.Turret;

    protected override void Awake()
    {
        base.Awake();
        TileTurret = transform.GetComponentInChildren<Turret>();
    }
    public override void TileDroped()
    {
        base.TileDroped();
        TileTurret.Spawned = true;
        TileTurret.TriggerPoloEffect(true);
        Turret.layer = LayerMask.NameToLayer(StaticData.TurretMask);
    }
    public override void OnSpawn()
    {
        TileTurret.InitializeTurret();
        Turret.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
    }

    public void ShowTurretRange(bool show)
    {
        if (TileTurret != null)
        {
            TileTurret.ShowRange(show);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        TileTurret.Spawned = false;
        TileTurret.RecycleRanges();
        //Turret.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
        //Turret.GetComponent<Collider2D>().enabled = true;//被覆盖时，会被射线检测disable掉groundtile层的Collider
        TileTurret.TriggerPoloEffect(false);
    }
}

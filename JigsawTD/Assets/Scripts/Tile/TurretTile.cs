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
        Turret.layer = LayerMask.NameToLayer(StaticData.GroundTileMask);
    }
    public override void OnSpawn()
    {
        TileTurret.InitializeTurret();
        TileTurret.Spawned = true;
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
        Turret.layer = LayerMask.NameToLayer(StaticData.TurretMask);
        Turret.GetComponent<Collider2D>().enabled = true;//������ʱ���ᱻ���߼��disable��groundtile���Collider
    }
}

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
        TileTurret.Dropped = true;
        TileTurret.TriggerPoloEffect(true);
        Turret.layer = LayerMask.NameToLayer(StaticData.TurretMask);
    }
    public override void OnSpawn()
    {
        TileTurret.InitializeTurret(this);
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
        if (TileTurret.Dropped)
        {
            TileTurret.TriggerPoloEffect(false);
        }
        TileTurret.AttackIntensify = 0;
        TileTurret.RangeIntensify = 0;
        TileTurret.SpeedIntensify = 0;
        TileTurret.targetList.Clear();
        TileTurret.Dropped = false;
        TileTurret.RecycleRanges();
        //Turret.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
        //Turret.GetComponent<Collider2D>().enabled = true;//被覆盖时，会被射线检测disable掉groundtile层的Collider
    }
}

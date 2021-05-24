using UnityEngine;

public class TurretTile : GameTile
{
    [SerializeField] GameObject TurretBase = default;
    public Turret tile;

    public override BasicTileType BasicTileType => BasicTileType.Turret;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void TileDroped()
    {
        base.TileDroped();
        tile.Dropped = true;
        tile.TriggerPoloEffect(true);
        TurretBase.layer = LayerMask.NameToLayer(StaticData.TurretMask);
    }
    public override void OnSpawn()
    {
        tile.InitializeTurret(this,tile.Quality);
        TurretBase.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
    }

    public void ShowTurretRange(bool show)
    {
        if (tile != null)
        {
            tile.ShowRange(show);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (tile.Dropped)
        {
            tile.TriggerPoloEffect(false);
        }
        tile.AttackIntensify = 0;
        tile.RangeIntensify = 0;
        tile.SpeedIntensify = 0;
        tile.targetList.Clear();
        tile.Dropped = false;
        tile.RecycleRanges();
        //Turret.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
        //Turret.GetComponent<Collider2D>().enabled = true;//被覆盖时，会被射线检测disable掉groundtile层的Collider
    }
}

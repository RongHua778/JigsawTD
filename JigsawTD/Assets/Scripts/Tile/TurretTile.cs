using UnityEngine;

public class TurretTile : GameTile
{
    private GameObject TurretBase = default;
    [HideInInspector] public Turret turret;

    public override BasicTileType BasicTileType => BasicTileType.Turret;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void TileDroped()
    {
        base.TileDroped();
        turret.Dropped = true;
        turret.TriggerPoloEffect(true);
        TurretBase.layer = LayerMask.NameToLayer(StaticData.TurretMask);
    }
    public override void OnSpawn()
    {
        TurretBase = transform.Find("TileBase/TurretBase").gameObject;
        turret = GetComponentInChildren<Turret>();
        turret.InitializeTurret(this,turret.Quality);
        TurretBase.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
    }

    public void ShowTurretRange(bool show)
    {
        if (turret != null)
        {
            turret.ShowRange(show);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (turret.Dropped)
        {
            turret.TriggerPoloEffect(false);
        }
        turret.AttackIntensify = 0;
        turret.Compositions.Clear();
        turret.Quality=1;
        turret.Element = Element.Gold;
        turret.RangeIntensify = 0;
        turret.SpeedIntensify = 0;
        turret.targetList.Clear();
        turret.Dropped = false;
        turret.ClearTurnIntensify();
        turret.RecycleRanges();
        //Turret.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);
        //Turret.GetComponent<Collider2D>().enabled = true;//被覆盖时，会被射线检测disable掉groundtile层的Collider
    }
}

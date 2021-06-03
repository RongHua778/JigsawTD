using System;
using UnityEngine;

public class TurretTile : GameTile
{
    private GameObject TurretBase = default;
    [HideInInspector] public Turret turret;

    public override BasicTileType BasicTileType => BasicTileType.Turret;

    public override void TileDroped()
    {
        base.TileDroped();

        SetGroundTile();
        turret.Dropped = true;
        turret.TriggerPoloEffect(true);
        GameManager.Instance.turrets.Add(turret);
    }

    protected override void TileDropCheck(Collider2D col)
    {
        base.TileDropCheck(col);
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            if (tile == BoardSystem.SelectingTile)
            {
                BoardSystem.SelectingTile = null;
            }
            ObjectPool.Instance.UnSpawn(tile.gameObject);
        }
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
        turret.ClearTurret();
    }

    public void Initialize(TurretAttribute attribute,int quality)
    {
        TurretBase = transform.Find("TileBase/TurretBase").gameObject;
        TurretBase.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);

        turret = this.GetComponentInChildren<Turret>();
        turret.m_TurretAttribute = attribute;
        turret.Quality = quality;
        turret.InitializeTurret();
        turret.m_TurretTile = this;
    }
}

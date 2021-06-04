using System;
using UnityEngine;

public class TurretTile : GameTile
{
    private GameObject TurretBase = default;
    [HideInInspector] public TurretContent turret;


    public override void TileLanded()
    {
        base.TileLanded();

        SetGroundTile();
        turret.Dropped = true;
        //turret.TriggerPoloEffect(true);
        GameManager.Instance.turrets.Add(turret);
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
        //turret.ClearTurret();
    }

    public void Initialize(TurretAttribute attribute,int quality)
    {
        TurretBase = transform.Find("TileBase/TurretBase").gameObject;
        TurretBase.layer = LayerMask.NameToLayer(StaticData.TempTurretMask);

        turret = this.GetComponentInChildren<TurretContent>();
        turret.m_TurretAttribute = attribute;
        //turret.Quality = quality;
        //turret.InitializeTurret();
        //turret.m_TurretTile = this;
    }
}

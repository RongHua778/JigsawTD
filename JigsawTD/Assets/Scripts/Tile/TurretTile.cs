using System;
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
        //if (turret.Dropped)
        //{
        //    turret.TriggerPoloEffect(false);
        //}
        //turret.AttackIntensify = 0;
        //turret.Quality = 1;
        //turret.Element = Element.Gold;
        //turret.RangeIntensify = 0;
        //turret.SpeedIntensify = 0;
        //turret.targetList.Clear();
        //turret.CriticalPercentage = 1.5f;
        //turret.Dropped = false;
        //turret.TargetCount = 1;
        //turret.DamageAnalysis = 0;
        //GameManager.Instance.turrets.behaviors.Remove(turret);//´ÓÁÐ±íÒÆ³ý
        //turret.ClearTurnIntensify();
        //turret.RecycleRanges();
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

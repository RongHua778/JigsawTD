using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GroundTile : TileBase
{
    int rangeIntensify;

    public GameTile TileAbrove;
    public int RangeIntensify { get => rangeIntensify; set { rangeIntensify = value; txt.text = value.ToString(); } }
    public float AttackIntensify;
    public float SpeedIntensify;
    [SerializeField] TextMeshPro txt = default;

    public void TriggerIntensify()
    {
        if (TileAbrove == null)
            return;
        if (TileAbrove.BasicTileType == BasicTileType.Turret)
        {
            Turret turret = ((TurretTile)TileAbrove).TileTurret;
            turret.RangeIntensify = RangeIntensify;
            turret.AttackIntensify = AttackIntensify;
            turret.SpeedIntensify = SpeedIntensify;
        }
    }


    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
}
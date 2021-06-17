using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.CompositeTurret;


    public Blueprint CompositeBluePrint;



    public override float AttackIntensify { get => base.AttackIntensify + CompositeBluePrint.CompositeAttackDamage; }
    public override float SpeedIntensify { get => base.SpeedIntensify + CompositeBluePrint.CompositeAttackSpeed; set => base.SpeedIntensify = value; }
    public override float CriticalIntensify { get => base.CriticalIntensify + CompositeBluePrint.CompositeCriticalRate; set => base.CriticalIntensify = value; }
    public override float SlowIntensify { get => base.SlowIntensify + CompositeBluePrint.CompositeSlowRate; set => base.SlowIntensify = value; }
    public override float SputteringIntensify { get => base.SputteringIntensify + CompositeBluePrint.CompositeSputteringRange; set => base.SputteringIntensify = value; }

    public override void ContentLanded()
    {
        base.ContentLanded();
        GameManager.Instance.compositeTurrets.Add(this);
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        CompositeBluePrint = null;
        GameManager.Instance.compositeTurrets.Remove(this);

    }

}

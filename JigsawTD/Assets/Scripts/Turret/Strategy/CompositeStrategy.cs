using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeStrategy : BasicStrategy
{
    public override StrategyType strategyType => StrategyType.Composite;
    public Blueprint CompositeBluePrint;
    public CompositeStrategy(TurretAttribute attribute,  Blueprint bluePrint, int quality) : base(attribute,  quality)
    {
        CompositeBluePrint = bluePrint;
    }

    public override float AttackIntensify { get => base.AttackIntensify + CompositeBluePrint.CompositeAttackDamage; }
    public override float SpeedIntensify { get => base.SpeedIntensify + CompositeBluePrint.CompositeAttackSpeed; }
    public override float CriticalIntensify { get => base.CriticalIntensify + CompositeBluePrint.CompositeCriticalRate; }
    public override float SlowIntensify { get => base.SlowIntensify + CompositeBluePrint.CompositeSlowRate; }
    public override float SputteringIntensify { get => base.SputteringIntensify + CompositeBluePrint.CompositeSputteringRange; }


}

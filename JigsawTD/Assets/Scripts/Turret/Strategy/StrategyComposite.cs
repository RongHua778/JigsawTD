using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyComposite : StrategyBase
{
    public override StrategyType strategyType => StrategyType.Composite;
    public Blueprint CompositeBluePrint;
    public StrategyComposite(TurretAttribute attribute, Blueprint bluePrint, int quality, TurretContent turret) : base(attribute, quality, turret)
    {
        CompositeBluePrint = bluePrint;
    }

    public override void SetQualityValue()
    {
        base.SetQualityValue();
        BaseAttackIntensify += CompositeBluePrint.CompositeAttackDamage;
        BaseSpeedIntensify += CompositeBluePrint.CompositeAttackSpeed;
        BaseCriticalRateIntensify += CompositeBluePrint.CompositeCriticalRate;
        BaseSputteringRangeIntensify += CompositeBluePrint.CompositeSputteringRange;
        BaseSlowRateIntensify += CompositeBluePrint.CompositeSlowRate;
        TurretSkillInfos = m_Att.TurretEffects;
        GetTurretSkills();
    }

    //下一级属性
    public float NextAttack { get => m_Att.TurretLevels[Quality].AttackDamage * (1 + BaseAttackIntensify); }
    public float NextSpeed { get => m_Att.TurretLevels[Quality].AttackSpeed * (1 + BaseSpeedIntensify); }
    public float NextSputteringRange { get => m_Att.TurretLevels[Quality].SputteringRange + BaseSputteringRangeIntensify; }
    public float NextCriticalRate { get => m_Att.TurretLevels[Quality].CriticalRate + BaseCriticalRateIntensify; }
    public float NextSlowRate { get => m_Att.TurretLevels[Quality].SlowRate + BaseSlowRateIntensify; }



}

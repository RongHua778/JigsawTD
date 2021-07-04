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
        ElementSkillInfos = m_Att.ElementSkills;
    }

    //下一级属性
    public float NextAttack { get => m_Att.TurretLevels[Quality].AttackDamage * (1 + BaseAttackIntensify); }
    public float NextSpeed { get => m_Att.TurretLevels[Quality].AttackSpeed * (1 + BaseSpeedIntensify); }
    public float NextSputteringRange { get => m_Att.TurretLevels[Quality].SputteringRange + BaseSputteringRangeIntensify; }
    public float NextCriticalRate { get => m_Att.TurretLevels[Quality].CriticalRate + BaseCriticalRateIntensify; }
    public float NextSlowRate { get => m_Att.TurretLevels[Quality].SlowRate + BaseSlowRateIntensify; }

    public ElementSkill ElementSkill1 { get; set; }
    public ElementSkill ElementSkill2 { get; set; }

    public void GetTurretSkills()//首次获取并激活效果
    {

        TurretSkill effect;
        foreach (TurretSkillInfo info in TurretSkillInfos)
        {
            effect = TurretEffectFactory.GetInitialSkill((int)info.EffectName);
            effect.strategy = this;
            effect.KeyValue = info.KeyValue;
            TurretSkills.Add(effect);
        }

        List<int> elements = new List<int>();
        foreach(var com in CompositeBluePrint.Compositions)
        {
            elements.Add(com.elementRequirement);
        }
        effect = TurretEffectFactory.GetElementSkill(elements);
        if (effect != null)
        {
            effect.strategy = this;
            ElementSkill1 = effect as ElementSkill;
            TurretSkills.Add(effect);
        }

        BuildTurretEffects();
    }


}

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


    public void GetComIntensify()
    {
        BaseAttackIntensify += CompositeBluePrint.CompositeAttackDamage;
        BaseSpeedIntensify += CompositeBluePrint.CompositeAttackSpeed;
        BaseCriticalRateIntensify += CompositeBluePrint.CompositeCriticalRate;
        BaseSputteringRangeIntensify += CompositeBluePrint.CompositeSputteringRange;
        BaseSlowRateIntensify += CompositeBluePrint.CompositeSlowRate;
    }

    //下一级属性
    public float NextAttack { get => m_Att.TurretLevels[Quality].AttackDamage * (1 + BaseAttackIntensify); }
    public float NextSpeed { get => m_Att.TurretLevels[Quality].AttackSpeed * (1 + BaseSpeedIntensify); }
    public float NextSputteringRange { get => m_Att.TurretLevels[Quality].SputteringRange + BaseSputteringRangeIntensify; }
    public float NextCriticalRate { get => m_Att.TurretLevels[Quality].CriticalRate + BaseCriticalRateIntensify; }
    public float NextSlowRate { get => m_Att.TurretLevels[Quality].SlowRate + BaseSlowRateIntensify; }


    public ElementSkill ElementSkill1 { get; set; }
    public TileSkill TileSkill { get; set; }


    public void GetTurretSkills()//首次获取并激活效果
    {
        TurretSkills.Clear();
        TurretSkill effect;
        effect = TurretEffectFactory.GetInitialSkill((int)m_Att.TurretSkill);
        effect.strategy = this;
        TurretSkill = effect;
        TurretSkills.Add(effect);

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
        else
        {
            Debug.LogWarning("没有该元素技能");
        }

        BuildTurretEffects();
    }

    public void LandedTurretSkill()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Detect();//放置效果
        }
    }

    public void DrawTurretSkill()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Draw();//抽取效果
        }
    }
    public void CompositeSkill()
    {
        foreach(var skill in TurretSkills)
        {
            skill.Composite();
        }
    }

    public void AddTileSkill(TileSkillName skillName)
    {
        if (TileSkill != null)
        {
            Debug.Log("已经有地形技能了");
            return;
        }
        TileSkill skill = TurretEffectFactory.GetTileSkill((int)skillName);
        skill.strategy = this;
        TileSkill = skill;
        TurretSkills.Add(skill);
        TileSkill.Build();
    }

}

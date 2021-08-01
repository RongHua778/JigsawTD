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


    public override void GetComIntensify(Blueprint bluePrint)
    {
        BaseAttackIntensify += bluePrint.CompositeAttackDamage;
        BaseSpeedIntensify += bluePrint.CompositeAttackSpeed;
        BaseCriticalRateIntensify += bluePrint.CompositeCriticalRate;
        BaseSputteringRangeIntensify += bluePrint.CompositeSputteringRange;
        BaseSlowRateIntensify += bluePrint.CompositeSlowRate;
    }

    //��һ������
    public float NextAttack { get => m_Att.TurretLevels[Quality].AttackDamage * (1 + BaseAttackIntensify); }
    public float NextSpeed { get => m_Att.TurretLevels[Quality].AttackSpeed * (1 + BaseSpeedIntensify); }
    public float NextSputteringRange { get => m_Att.TurretLevels[Quality].SputteringRange + BaseSputteringRangeIntensify; }
    public float NextCriticalRate { get => m_Att.TurretLevels[Quality].CriticalRate + BaseCriticalRateIntensify; }
    public float NextSlowRate { get => m_Att.TurretLevels[Quality].SlowRate + BaseSlowRateIntensify; }


    public void GetTurretSkills()//�״λ�ȡ������Ч��
    {
        TurretSkills.Clear();

        TurretSkill effect = TurretEffectFactory.GetInitialSkill((int)m_Att.TurretSkill);//�Դ�����
        TurretSkill = effect;
        AddSkill(effect);

        //Ԫ����ϼ���
        List<int> elements = new List<int>();
        foreach(var com in CompositeBluePrint.Compositions)
        {
            elements.Add(com.elementRequirement);
        }
        TurretSkill effect2 = TurretEffectFactory.GetElementSkill(elements);
        ElementSkill = effect2 as ElementSkill;
        AddSkill(effect2);

    }



    public void LandedTurretSkill()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Detect();//����Ч��
        }
    }

    public void DrawTurretSkill()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Draw();//��ȡЧ��
        }
    }
    public void CompositeSkill()
    {
        foreach(var skill in TurretSkills)
        {
            skill.Composite();
        }
    }

    //public void AddTileSkill(TileSkillName skillName)
    //{
    //    if (TileSkill != null)
    //    {
    //        Debug.Log("�Ѿ��е��μ�����");
    //        return;
    //    }
    //    TileSkill skill = TurretEffectFactory.GetTileSkill((int)skillName);
    //    skill.strategy = this;
    //    TileSkill = skill;
    //    TurretSkills.Add(skill);
    //    TileSkill.Build();
    //}

}

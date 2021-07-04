using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyType
{
    Element, Composite
}

public abstract class StrategyBase
{
    public virtual StrategyType strategyType => StrategyType.Element;
    //字段
    public TurretContent m_Turret;
    public TurretAttribute m_Att;
    private int quality = 0;
    public int Quality { get => quality; set => quality = value; }

    public StrategyBase(TurretAttribute attribute, int quality, TurretContent turret)
    {
        m_Att = attribute;
        m_Turret = turret;
        this.Quality = quality;
    }

    //基础属性
    private float initAttack;
    private float initSpeed;
    private int initRange;
    private float initCriticalRate;
    private float initSputteringRange;
    private float initSlowRate;
    public float InitAttack { get => initAttack; set => initAttack = value; }
    public float InitSpeed { get => initSpeed; set => initSpeed = value; }
    public int InitRange { get => initRange; set => initRange = value; }
    public float InitCriticalRate { get => initCriticalRate; set => initCriticalRate = value; }
    public float InitSputteringRange { get => initSputteringRange; set => initSputteringRange = value; }
    public float InitSlowRate { get => initSlowRate; set => initSlowRate = value; }

    //二级属性
    private float initSputteringPercentage = 0.5f;//溅射伤害率
    private float initCriticalPercentage = 1.5f;//暴击伤害率
    private int initTargetCount = 1;//目标数
    private float rotSpeed = 10f;//炮塔转速
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }

    //基础加成
    private float baseAttackIntensify;
    private float baseSpeedIntensify;
    private int baseRangeIntensify;
    private float baseCriticalRateIntensify;
    private float baseSlowRateIntensify;
    private float baseSputteringRangeIntensify;
    private float baseSputteringPercentageIntensify;
    private float baseCriticalPercentageIntensify;
    private int baseTargetCountIntensify;
    public float BaseAttackIntensify { get => baseAttackIntensify; set => baseAttackIntensify = value; }
    public float BaseSpeedIntensify { get => baseSpeedIntensify; set => baseSpeedIntensify = value; }
    public int BaseRangeIntensify { get => baseRangeIntensify; set => baseRangeIntensify = value; }
    public float BaseCriticalRateIntensify { get => baseCriticalRateIntensify; set => baseCriticalRateIntensify = value; }
    public float BaseSlowRateIntensify { get => baseSlowRateIntensify; set => baseSlowRateIntensify = value; }
    public float BaseSputteringPercentageIntensify { get => baseSputteringPercentageIntensify; set => baseSputteringPercentageIntensify = value; }
    public float BaseSputteringRangeIntensify { get => baseSputteringRangeIntensify; set => baseSputteringRangeIntensify = value; }
    public int BaseTargetCountIntensify { get => baseTargetCountIntensify; set => baseTargetCountIntensify = value; }
    public float BaseCriticalPercentageIntensify { get => baseCriticalPercentageIntensify; set => baseCriticalPercentageIntensify = value; }


    //最终基础属性
    private int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }
    public float BaseAttack { get => InitAttack * (1 + BaseAttackIntensify); }
    public float BaseSpeed { get => InitSpeed * (1 + BaseSpeedIntensify); }
    public int BaseRange { get => InitRange + BaseRangeIntensify; }
    public float BaseCriticalRate { get => InitCriticalRate + BaseCriticalRateIntensify; }
    public float BaseCriticalPercentage { get => initCriticalPercentage + BaseCriticalPercentageIntensify; }
    public float BaseSputteringRange { get => InitSputteringRange + BaseSputteringRangeIntensify; }
    public float BaseSputteringPercentage { get => initSputteringPercentage + BaseSputteringPercentageIntensify; }
    public float BaseSlowRate { get => InitSlowRate + BaseSlowRateIntensify; }
    public int BaseTargetCount { get => initTargetCount + BaseTargetCountIntensify; }

    //战斗中属性
    public float FinalAttack { get => BaseAttack * TurnAttackIntensify + TurnFixAttack; }
    public float FinalSpeed { get => BaseSpeed * TurnSpeedIntensify + TurnFixSpeed; }
    public int FinalRange { get => BaseRange + TurnFixRange; }
    public float FinalCriticalRate { get => BaseCriticalRate * TurnCriticalRateIntensify + TurnFixCriticalRate; }
    public float FinalCriticalPercentage { get => (BaseCriticalPercentage + FinalCriticalRate) * TurnCriticalRateIntensify + TurnFixCriticalPercentage; }
    public float FinalSputteringRange { get => BaseSputteringRange * TurnSputteringRangeIntensify + TurnFixSputteringRange; }
    public float FinalSputteringPercentage { get => BaseSputteringPercentage * TurnSputteringPercentageIntensify + TurnFixSputteringPercentage; }
    public float FinalSlowRate { get => BaseSlowRate * TurnSlowRateIntensify + TurnFixSlowRate; }
    public int FinalTargetCount { get => BaseTargetCount + TurnFixTargetCount; }


    #region 战斗中固定加成
    private float turnFixAttack;
    private float turnFixSpeed;
    private int turnFixRange;
    private float turnFixSputteringRange;
    private float turnFixSputteringPercentage;
    private float turnFixCriticalRate;
    private float turnFixCriticalPercentage;
    private float turnFixSlowRate;
    private int turnFixTargetCount;
    public float TurnFixAttack { get => turnFixAttack; set => turnFixAttack = value; }
    public float TurnFixSpeed { get => turnFixSpeed; set => turnFixSpeed = value; }
    public int TurnFixRange { get => turnFixRange; set => turnFixRange = value; }
    public float TurnFixSputteringRange { get => turnFixSputteringRange; set => turnFixSputteringRange = value; }
    public float TurnFixSputteringPercentage { get => turnFixSputteringPercentage; set => turnFixSputteringPercentage = value; }
    public float TurnFixCriticalRate { get => turnFixCriticalRate; set => turnFixCriticalRate = value; }
    public float TurnFixCriticalPercentage { get => turnFixCriticalPercentage; set => turnFixCriticalPercentage = value; }
    public float TurnFixSlowRate { get => turnFixSlowRate; set => turnFixSlowRate = value; }
    public int TurnFixTargetCount { get => turnFixTargetCount; set => turnFixTargetCount = value; }
    #endregion

    #region 战斗中百分比加成
    private float turnAttackIntensify = 1;
    private float turnSpeedIntensify = 1;
    private float turnCriticalRateIntensify = 1;
    private float turnCriticalPercentageIntensify = 1;
    private float turnSputteringRangeIntensify = 1;
    private float turnSputteringPercentageIntensify = 1;
    private float turnSlowRateIntensify = 1;
    public float TurnAttackIntensify { get => turnAttackIntensify; set => turnAttackIntensify = value; }
    public float TurnSpeedIntensify { get => turnSpeedIntensify; set => turnSpeedIntensify = value; }
    public float TurnCriticalRateIntensify { get => turnCriticalRateIntensify; set => turnCriticalRateIntensify = value; }
    public float TurnCriticalPercentageIntensify { get => turnCriticalPercentageIntensify; set => turnCriticalPercentageIntensify = value; }
    public float TurnSputteringRangeIntensify { get => turnSputteringRangeIntensify; set => turnSputteringRangeIntensify = value; }
    public float TurnSputteringPercentageIntensify { get => turnSputteringPercentageIntensify; set => turnSputteringPercentageIntensify = value; }
    public float TurnSlowRateIntensify { get => turnSlowRateIntensify; set => turnSlowRateIntensify = value; }
    #endregion

    protected List<TurretSkillInfo> TurretSkillInfos { get; set; }
    protected List<ElementSkillInfo> ElementSkillInfos { get; set; }

    public List<TurretSkill> TurretSkills = new List<TurretSkill>();



    public void BuildTurretEffects()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Build();//前置修正
        }
        foreach (var skill in TurretSkills)
        {
            skill.BuildEnd();//后置修正
        }
    }


    public virtual void SetQualityValue()
    {
        ClearBasicIntensify();
        InitAttack = m_Att.TurretLevels[Quality - 1].AttackDamage;
        InitSpeed = m_Att.TurretLevels[Quality - 1].AttackSpeed;
        InitRange = m_Att.TurretLevels[Quality - 1].AttackRange;
        InitCriticalRate = m_Att.TurretLevels[Quality - 1].CriticalRate;
        InitSputteringRange = m_Att.TurretLevels[Quality - 1].SputteringRange;
        InitSlowRate = m_Att.TurretLevels[Quality - 1].SlowRate;
    }

    public void StartTurnSkills()
    {
        foreach (var skill in TurretSkills)
        {
            skill.StartTurn();
        }
    }

    public void ClearBasicIntensify()
    {
        //基础加成
        RotSpeed = 10;
        BaseAttackIntensify = 0;
        BaseSpeedIntensify = 0;
        BaseRangeIntensify = 0;
        BaseCriticalRateIntensify = 0;
        BaseCriticalPercentageIntensify = 0;
        BaseSputteringRangeIntensify = 0;
        BaseSputteringPercentageIntensify = 0;
        BaseSlowRateIntensify = 0;
        BaseTargetCountIntensify = 0;
    }

    public void ClearTurnIntensify()
    {
        //回合固定加成
        TurnFixAttack = 0;
        TurnFixSpeed = 0;
        TurnFixRange = 0;
        TurnFixSputteringRange = 0;
        TurnFixSputteringPercentage = 0;
        TurnFixCriticalRate = 0;
        TurnFixCriticalPercentage = 0;
        TurnFixSlowRate = 0;
        TurnFixTargetCount = 0;

        //回合百分比加成
        TurnAttackIntensify = 1;
        TurnSpeedIntensify = 1;
        TurnCriticalRateIntensify = 1;
        TurnCriticalPercentageIntensify = 1;
        TurnSputteringRangeIntensify = 1;
        TurnSputteringPercentageIntensify = 1;
        TurnSlowRateIntensify = 1;

        //回合结束效果
        foreach (TurretSkill skill in TurretSkills)
        {
            skill.EndTurn();
        }
    }
}

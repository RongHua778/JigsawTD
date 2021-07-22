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
    //�ֶ�
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

    //��������
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

    //��������
    private float initSputteringPercentage = 0.5f;//�����˺���
    private float initCriticalPercentage = 1.5f;//�����˺���
    private int initTargetCount = 1;//Ŀ����
    private float rotSpeed = 10f;//����ת��
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    private float upgradeDiscount = 0;//�����ۿ�
    public float UpgradeDiscount { get => upgradeDiscount; set => upgradeDiscount = value; }

    //�����ӳ�
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
    public float BaseCriticalPercentageIntensify { get => baseCriticalPercentageIntensify; set => baseCriticalPercentageIntensify = value; }
    public float BaseSlowRateIntensify { get => baseSlowRateIntensify; set => baseSlowRateIntensify = value; }
    public float BaseSputteringPercentageIntensify { get => baseSputteringPercentageIntensify; set => baseSputteringPercentageIntensify = value; }
    public float BaseSputteringRangeIntensify { get => baseSputteringRangeIntensify; set => baseSputteringRangeIntensify = value; }
    public int BaseTargetCountIntensify { get => baseTargetCountIntensify; set => baseTargetCountIntensify = value; }

    #region ���мӳɶ�������
    private float allAttackIntensifyModify = 1;
    private float allSpeedIntensifyModify = 1;
    private float allCriticalIntensifyModify = 1;
    private float baseSputteringRangeIntensifyModify = 1;
    private float baseSputteringPercentageIntensifyModify = 1;
    private float baseSlowRateIntensifyModify = 1;
    private float baseCriticalPercentageIntensifyModify = 1;
    public float AllAttackIntensifyModify { get => allAttackIntensifyModify; set => allAttackIntensifyModify = value; }
    public float AllSpeedIntensifyModify { get => allSpeedIntensifyModify; set => allSpeedIntensifyModify = value; }
    public float AllCriticalIntensifyModify { get => allCriticalIntensifyModify; set => allCriticalIntensifyModify = value; }
    public float BaseSputteringRangeIntensifyModify { get => baseSputteringRangeIntensifyModify; set => baseSputteringRangeIntensifyModify = value; }
    public float BaseSputteringPercentageIntensifyModify { get => baseSputteringPercentageIntensifyModify; set => baseSputteringPercentageIntensifyModify = value; }
    public float BaseSlowRateIntensifyModify { get => baseSlowRateIntensifyModify; set => baseSlowRateIntensifyModify = value; }
    public float BaseCriticalPercentageIntensifyModify { get => baseCriticalPercentageIntensifyModify; set => baseCriticalPercentageIntensifyModify = value; }
    #endregion

    //���ջ�������
    private int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }
    public float BaseAttack { get => InitAttack * (1 + BaseAttackIntensify * AllAttackIntensifyModify); }
    public float BaseSpeed { get => InitSpeed * (1 + BaseSpeedIntensify * AllSpeedIntensifyModify); }
    public int BaseRange { get => InitRange + BaseRangeIntensify; }
    public float BaseCriticalRate { get => InitCriticalRate + BaseCriticalRateIntensify * AllCriticalIntensifyModify; }
    public float BaseCriticalPercentage { get => initCriticalPercentage + BaseCriticalPercentageIntensify * BaseCriticalPercentageIntensifyModify; }
    public float BaseSputteringRange { get => InitSputteringRange + BaseSputteringRangeIntensify * BaseSputteringRangeIntensifyModify; }
    public float BaseSputteringPercentage { get => initSputteringPercentage + BaseSputteringPercentageIntensify * BaseSputteringPercentageIntensifyModify; }
    public float BaseSlowRate { get => InitSlowRate + BaseSlowRateIntensify * BaseSlowRateIntensifyModify; }
    public int BaseTargetCount { get => initTargetCount + BaseTargetCountIntensify; }

    //ս��������
    public float FinalAttack { get => BaseAttack * (1 + (TurnAttackIntensify - 1) * AllAttackIntensifyModify) + TurnFixAttack * AllAttackIntensifyModify; }
    public float FinalSpeed { get => BaseSpeed * (1 + (TurnSpeedIntensify - 1) * AllSpeedIntensifyModify) + TurnFixSpeed * AllSpeedIntensifyModify; }
    public int FinalRange { get => BaseRange + TurnFixRange; }
    public float FinalCriticalRate { get => BaseCriticalRate * (1 + (TurnCriticalRateIntensify - 1) * AllCriticalIntensifyModify) + TurnFixCriticalRate * AllCriticalIntensifyModify; }
    public float FinalCriticalPercentage { get => (BaseCriticalPercentage + FinalCriticalRate) * TurnCriticalRateIntensify + TurnFixCriticalPercentage; }
    public float FinalSputteringRange { get => BaseSputteringRange * TurnSputteringRangeIntensify + TurnFixSputteringRange; }
    public float FinalSputteringPercentage { get => BaseSputteringPercentage * TurnSputteringPercentageIntensify + TurnFixSputteringPercentage; }
    public float FinalSlowRate { get => BaseSlowRate * TurnSlowRateIntensify + TurnFixSlowRate; }
    public int FinalTargetCount { get => BaseTargetCount + TurnFixTargetCount; }


    #region ս���й̶��ӳ�
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

    #region ս���аٷֱȼӳ�
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

    public TurretSkill TurretSkill { get; set; }
    public List<TurretSkill> TurretSkills = new List<TurretSkill>();



    public void BuildTurretEffects()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Build();//ǰ������
        }
    }



    public virtual void SetQualityValue()
    {
        //ClearBasicIntensify();
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

    public virtual void ClearBasicIntensify()
    {
        UpgradeDiscount = 0;
        //�����ӳ�
        BaseAttackIntensify = 0;
        BaseSpeedIntensify = 0;
        BaseRangeIntensify = 0;
        BaseCriticalRateIntensify = 0;
        BaseCriticalPercentageIntensify = 0;
        BaseSputteringRangeIntensify = 0;
        BaseSputteringPercentageIntensify = 0;
        BaseSlowRateIntensify = 0;
        BaseTargetCountIntensify = 0;

        //�����ӳɶ�������
        AllAttackIntensifyModify = 1;
        AllSpeedIntensifyModify = 1;
        AllCriticalIntensifyModify = 1;
        BaseCriticalPercentageIntensifyModify = 1;
        BaseSputteringRangeIntensifyModify = 1;
        BaseSputteringPercentageIntensifyModify = 1;
        BaseSlowRateIntensifyModify = 1;

    }

    public void ClearTurnIntensify()
    {
        //�غϹ̶��ӳ�
        TurnFixAttack = 0;
        TurnFixSpeed = 0;
        TurnFixRange = 0;
        TurnFixSputteringRange = 0;
        TurnFixSputteringPercentage = 0;
        TurnFixCriticalRate = 0;
        TurnFixCriticalPercentage = 0;
        TurnFixSlowRate = 0;
        TurnFixTargetCount = 0;

        //�غϰٷֱȼӳ�
        TurnAttackIntensify = 1;
        TurnSpeedIntensify = 1;
        TurnCriticalRateIntensify = 1;
        TurnCriticalPercentageIntensify = 1;
        TurnSputteringRangeIntensify = 1;
        TurnSputteringPercentageIntensify = 1;
        TurnSlowRateIntensify = 1;

        //�غϽ���Ч��
        foreach (TurretSkill skill in TurretSkills)
        {
            skill.EndTurn();
        }
    }
}

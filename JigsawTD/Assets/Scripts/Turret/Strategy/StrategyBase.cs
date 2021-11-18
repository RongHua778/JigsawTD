using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum StrategyType
{
    Element, Composite
}

public class StrategyBase
{

    public Blueprint CompositeBluePrint;
    //字段
    public TurretContent m_Turret;
    public TurretAttribute m_Att;
    private int quality = 0;
    public int Quality { get => quality; set => quality = value; }


    public StrategyBase(TurretAttribute attribute, int quality)
    {
        m_Att = attribute;
        this.Quality = quality;
        this.RangeType = attribute.RangeType;
        SetQualityValue();//属性设置
    }


    //基础属性
    private float initAttack;
    private float initSpeed;
    private int initRange;
    private float initCriticalRate;
    private float initSputteringRange;
    private float initSlowRate;
    public float InitAttack { get => initAttack; set => initAttack = value; }
    public float InitFireRate { get => initSpeed; set => initSpeed = value; }
    public int InitRange { get => initRange; set => initRange = value; }
    public float InitCriticalRate { get => initCriticalRate; set => initCriticalRate = value; }
    public float InitSplashRange { get => initSputteringRange; set => initSputteringRange = value; }
    public float InitSlowRate { get => initSlowRate; set => initSlowRate = value; }

    //二级属性
    private int elementSkillSlot = 2;//元素技能槽数
    public int ElementSKillSlot { get => elementSkillSlot; set => elementSkillSlot = value; }
    private RangeType rangeType = RangeType.Circle;
    public RangeType RangeType
    {
        get => rangeType;
        set
        {
            rangeType = value;
            switch (rangeType)
            {
                case RangeType.Circle:
                case RangeType.HalfCircle:
                    CheckAngle = 10f;
                    RotSpeed = 10f;
                    break;
                case RangeType.Line:
                    CheckAngle = 45f;
                    RotSpeed = 0f;
                    break;
            }
        }
    }
    private float initSplashPercentage = 0.5f;//溅射伤害率
    private float initCriticalPercentage = 1.5f;//暴击伤害率
    private int initTargetCount = 1;//目标数
    private float rotSpeed = 10f;//炮塔转速
    private float checkAngle = 10f;//攻击检测角度
    private float timeModify = 1;//回合内时间加成修正
    public float TimeModify { get => timeModify; set => timeModify = value; }
    private int shootTriggerCount = 1;//攻击特效触发次数
    public int ShootTriggerCount { get => shootTriggerCount; set => shootTriggerCount = value; }
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    public float CheckAngle { get => checkAngle; set => checkAngle = value; }
    private float upgradeDiscount = 0;//升级折扣
    public float UpgradeDiscount { get => Mathf.Max(0, upgradeDiscount); set => upgradeDiscount = value; }

    private int forbidRange;
    public int ForbidRange { get => forbidRange; set => forbidRange = value; }

    //基础加成
    private float initAttackModify = 1;
    private float initFireRateModify = 1;
    private int initRangeModify = 1;
    private float initCriticalRateModify = 1;
    private float initSlowRateModify = 1;
    private float initSplashRangeModify = 1;
    public float InitAttackModify { get => initAttackModify; set => initAttackModify = value; }
    public float InitFirerateModify { get => initFireRateModify; set => initFireRateModify = value; }
    public int InitRangeModify { get => initRangeModify; set => initRangeModify = value; }
    public float InitCriticalRateModify { get => initCriticalRateModify; set => initCriticalRateModify = value; }
    public float InitSlowRateModify { get => initSlowRateModify; set => initSlowRateModify = value; }
    public float InitSplashRangeModify { get => initSplashRangeModify; set => initSplashRangeModify = value; }

    private int baseTargetCountIntensify;
    public int BaseTargetCountIntensify { get => baseTargetCountIntensify; set => baseTargetCountIntensify = value; }

    #region 元素加成
    private float comAttackIntensify;
    private float comSpeedIntensify;
    private int comRangeIntensify;
    private float comCriticalIntensify;
    private float comSputteringRangeIntensify;
    private float comSlowIntensify;
    public float ComAttackIntensify { get => comAttackIntensify; set => comAttackIntensify = value; }
    public float ComSpeedIntensify { get => comSpeedIntensify; set => comSpeedIntensify = value; }
    public int ComRangeIntensify { get => comRangeIntensify; set => comRangeIntensify = value; }
    public float ComCriticalIntensify { get => comCriticalIntensify; set => comCriticalIntensify = value; }
    public float ComSplashRangeIntensify { get => comSputteringRangeIntensify; set => comSputteringRangeIntensify = value; }
    public float ComSlowIntensify { get => comSlowIntensify; set => comSlowIntensify = value; }

    #endregion

    #region 所有加成二次修正
    private float allAttackIntensifyModify = 1;
    private float allSpeedIntensifyModify = 1;
    private float allCriticalIntensifyModify = 1;
    private float baseSputteringRangeIntensifyModify = 1;
    private float baseSputteringPercentageIntensifyModify = 1;
    private float baseSlowRateIntensifyModify = 1;
    private float baseCriticalPercentageIntensifyModify = 1;
    private float poloIntensifyModify = 1;
    public float AllAttackIntensifyModify { get => allAttackIntensifyModify; set => allAttackIntensifyModify = value; }
    public float AllSpeedIntensifyModify { get => allSpeedIntensifyModify; set => allSpeedIntensifyModify = value; }
    public float AllCriticalIntensifyModify { get => allCriticalIntensifyModify; set => allCriticalIntensifyModify = value; }
    public float BaseSplashRangeIntensifyModify { get => baseSputteringRangeIntensifyModify; set => baseSputteringRangeIntensifyModify = value; }
    public float BaseSputteringPercentageIntensifyModify { get => baseSputteringPercentageIntensifyModify; set => baseSputteringPercentageIntensifyModify = value; }
    public float BaseSlowRateIntensifyModify { get => baseSlowRateIntensifyModify; set => baseSlowRateIntensifyModify = value; }
    public float BaseCriticalPercentageIntensifyModify { get => baseCriticalPercentageIntensifyModify; set => baseCriticalPercentageIntensifyModify = value; }
    public float PoloIntensifyModify { get => poloIntensifyModify; set => poloIntensifyModify = value; }
    #endregion

    //最终基础属性
    private int turnDamage;
    public int TurnDamage { get => turnDamage; set => turnDamage = value; }
    private int totalDamage;
    public int TotalDamage { get => totalDamage; set => totalDamage = value; }
    public float BaseAttack { get => InitAttack * InitAttackModify * (1 + ComAttackIntensify * AllAttackIntensifyModify); }
    public float BaseSpeed { get => InitFireRate * InitFirerateModify * (1 + ComSpeedIntensify * AllSpeedIntensifyModify); }
    public int BaseRange { get => InitRange * InitRangeModify + ComRangeIntensify; }
    public float BaseCriticalRate { get => InitCriticalRate * InitCriticalRateModify + ComCriticalIntensify * AllCriticalIntensifyModify; }
    public float BaseCriticalPercentage { get => initCriticalPercentage; }
    public float BaseSplashRange { get => InitSplashRange * InitSplashRangeModify + ComSplashRangeIntensify * BaseSplashRangeIntensifyModify; }
    public float BaseSplashPercentage { get => initSplashPercentage; }
    public float BaseSlowRate { get => InitSlowRate * InitSlowRateModify + ComSlowIntensify * BaseSlowRateIntensifyModify; }
    public int BaseTargetCount { get => initTargetCount + BaseTargetCountIntensify; }

    //战斗中属性
    public float FinalAttack { get => (BaseAttack * (1 + (TurnAttackIntensify - 1) * AllAttackIntensifyModify) + TurnFixAttack * AllAttackIntensifyModify) * AttackAdjust; }
    public float FinalFireRate { get => Mathf.Min(30, (BaseSpeed * (1 + (TurnSpeedIntensify - 1) * AllSpeedIntensifyModify) + TurnFixSpeed * AllSpeedIntensifyModify) * SpeedAdjust); }//速度上限30
    public int FinalRange { get => BaseRange + TurnFixRange; }
    public float FinalCriticalRate { get => (BaseCriticalRate * (1 + (TurnCriticalRateIntensify - 1) * AllCriticalIntensifyModify) + TurnFixCriticalRate * AllCriticalIntensifyModify) * CritialAdjust; }
    public float FinalCriticalPercentage { get => ((BaseCriticalPercentage + FinalCriticalRate) * TurnCriticalRateIntensify + TurnFixCriticalPercentage) * CritialAdjust; }

    public float FinalSplashRange { get => (BaseSplashRange * TurnSplashRangeIntensify + TurnFixSplashRange) * SplashAdjust; }
    public float FinalSplashPercentage { get => BaseSplashPercentage * TurnSplashPercentageIntensify + TurnFixSplashPercentage; }
    public float FinalSlowRate { get => (BaseSlowRate * TurnSlowRateIntensify + TurnFixSlowRate) * SlowAdjust; }
    public int FinalTargetCount { get => BaseTargetCount + TurnFixTargetCount; }

    #region 全局修正
    private float attackAdjust = 1;
    private float speedAdjust = 1;
    private float critialAdjust = 1;
    private float slowAdjust = 1;
    private float splashAdjust = 1;
    public float AttackAdjust { get => attackAdjust; set => attackAdjust = value; }
    public float SpeedAdjust { get => speedAdjust; set => speedAdjust = value; }
    public float CritialAdjust { get => critialAdjust; set => critialAdjust = value; }
    public float SlowAdjust { get => slowAdjust; set => slowAdjust = value; }
    public float SplashAdjust { get => splashAdjust; set => splashAdjust = value; }
    #endregion

    #region 战斗中固定加成
    private float turnFixAttack;
    private float turnFixSpeed;
    private int turnFixRange;
    private float turnFixSplashRange;
    private float turnFixSplashPercentage;
    private float turnFixCriticalRate;
    private float turnFixCriticalPercentage;
    private float turnFixSlowRate;
    private int turnFixTargetCount;
    public float TurnFixAttack { get => turnFixAttack; set => turnFixAttack = value; }
    public float TurnFixSpeed { get => turnFixSpeed; set => turnFixSpeed = value; }
    public int TurnFixRange { get => turnFixRange; set => turnFixRange = value; }
    public float TurnFixSplashRange { get => turnFixSplashRange; set => turnFixSplashRange = value; }
    public float TurnFixSplashPercentage { get => turnFixSplashPercentage; set => turnFixSplashPercentage = value; }
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
    public float TurnSplashRangeIntensify { get => turnSputteringRangeIntensify; set => turnSputteringRangeIntensify = value; }
    public float TurnSplashPercentageIntensify { get => turnSputteringPercentageIntensify; set => turnSputteringPercentageIntensify = value; }
    public float TurnSlowRateIntensify { get => turnSlowRateIntensify; set => turnSlowRateIntensify = value; }
    #endregion


    #region 下级属性
    public float NextAttack { get => m_Att.TurretLevels[Quality].AttackDamage * InitAttackModify * (1 + ComAttackIntensify); }
    public float NextSpeed { get => m_Att.TurretLevels[Quality].AttackSpeed * InitFirerateModify * (1 + ComSpeedIntensify); }
    public float NextSplashRange { get => m_Att.TurretLevels[Quality].SplashRange * InitSplashRangeModify + ComSplashRangeIntensify; }
    public float NextCriticalRate { get => m_Att.TurretLevels[Quality].CriticalRate * InitCriticalRateModify + ComCriticalIntensify; }
    public float NextSlowRate { get => m_Att.TurretLevels[Quality].SlowRate * InitSlowRateModify + ComSlowIntensify; }
    #endregion

    public TurretSkill TurretSkill { get; set; }

    public List<TurretSkill> TurretSkills = new List<TurretSkill>();


    public void GetComIntensify(List<int> elements, bool add = true)
    {
        foreach (var element in elements)
        {
            StaticData.ElementDIC[(ElementType)element].GetComIntensify(this, add);
        }
    }

    public void GetTurretSkills()//首次获取被动技能效果
    {
        TurretSkills.Clear();

        TurretSkill effect = TurretEffectFactory.GetInitialSkill((int)m_Att.TurretSkill);//自带技能
        TurretSkill = effect;
        TurretSkill.strategy = this;
        TurretSkills.Add(effect);
        TurretSkill.Build();

    }

    public void AddElementSkill(ElementSkill skill)
    {
        skill.strategy = this;
        TurretSkills.Add(skill);
        skill.Build();
    }

    public void OnEquipSkill()
    {
        foreach (var skill in TurretSkills.ToList())
        {
            skill.OnEquip();
        }
    }

    public virtual void SetQualityValue()
    {
        InitAttack = m_Att.TurretLevels[Quality - 1].AttackDamage;
        InitFireRate = m_Att.TurretLevels[Quality - 1].AttackSpeed;
        InitRange = m_Att.TurretLevels[Quality - 1].AttackRange;
        InitCriticalRate = m_Att.TurretLevels[Quality - 1].CriticalRate;
        InitSplashRange = m_Att.TurretLevels[Quality - 1].SplashRange;
        InitSlowRate = m_Att.TurretLevels[Quality - 1].SlowRate;
        //ForbidRange = m_Att.TurretLevels[Quality - 1].ForbidRange;
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
        //基础加成
        InitAttackModify = 0;
        InitFirerateModify = 0;
        InitRangeModify = 0;
        InitCriticalRateModify = 0;
        InitSplashRangeModify = 0;
        InitSlowRateModify = 0;
        BaseTargetCountIntensify = 0;

        //基础加成二次修正
        AllAttackIntensifyModify = 1;
        AllSpeedIntensifyModify = 1;
        AllCriticalIntensifyModify = 1;
        BaseCriticalPercentageIntensifyModify = 1;
        BaseSplashRangeIntensifyModify = 1;
        BaseSputteringPercentageIntensifyModify = 1;
        BaseSlowRateIntensifyModify = 1;
        PoloIntensifyModify = 1;

    }

    public void ClearTurnAnalysis()
    {
        TurnDamage = 0;
    }

    public void ClearTurnIntensify()
    {
        ////回合时间修正
        //TimeModify = 1;

        //回合固定加成
        TurnFixAttack = 0;
        TurnFixSpeed = 0;
        TurnFixRange = 0;
        TurnFixSplashRange = 0;
        TurnFixSplashPercentage = 0;
        TurnFixCriticalRate = 0;
        TurnFixCriticalPercentage = 0;
        TurnFixSlowRate = 0;
        TurnFixTargetCount = 0;

        //回合百分比加成
        TurnAttackIntensify = 1;
        TurnSpeedIntensify = 1;
        TurnCriticalRateIntensify = 1;
        TurnCriticalPercentageIntensify = 1;
        TurnSplashRangeIntensify = 1;
        TurnSplashPercentageIntensify = 1;
        TurnSlowRateIntensify = 1;

        //回合结束效果
        foreach (TurretSkill skill in TurretSkills)
        {
            skill.EndTurn();
        }
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
        for (int i = 0; i < TurretSkills.Count; i++)
        {
            TurretSkills[i].Composite();
        }
    }

    public void EnterSkill(IDamageable target)
    {
        for (int i = 0; i < TurretSkills.Count; i++)
        {
            TurretSkills[i].OnEnter(target);
        }
    }

    public void ExitSkill(IDamageable target)
    {
        for (int i = 0; i < TurretSkills.Count; i++)
        {
            TurretSkills[i].OnExit(target);
        }
    }
}

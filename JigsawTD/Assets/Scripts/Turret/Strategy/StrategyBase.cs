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
    //�ֶ�
    public TurretContent m_Turret;
    public TurretAttribute m_Att;
    private int quality = 0;
    public int Quality { get => quality; set => quality = value; }


    public StrategyBase(TurretAttribute attribute, int quality, Blueprint comBlueprint = null)
    {
        m_Att = attribute;
        this.Quality = quality;
        CompositeBluePrint = comBlueprint;
        this.RangeType = attribute.RangeType;
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
    private int elementSkillSlot = 2;//Ԫ�ؼ��ܲ���
    public int ElementSKillSlot { get => elementSkillSlot; set => elementSkillSlot = value; }

    public RangeType RangeType = RangeType.Circle;
    private float initSputteringPercentage = 0.5f;//�����˺���
    private float initCriticalPercentage = 1.5f;//�����˺���
    private int initTargetCount = 1;//Ŀ����
    private float rotSpeed = 10f;//����ת��
    private float timeModify = 1;//�غ���ʱ��ӳ�����
    public float TimeModify { get => timeModify; set => timeModify = value; }
    private int shootTriggerCount = 1;//������Ч��������
    public int ShootTriggerCount { get => shootTriggerCount; set => shootTriggerCount = value; }
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    private float upgradeDiscount = 0;//�����ۿ�
    public float UpgradeDiscount { get => Mathf.Max(0, upgradeDiscount); set => upgradeDiscount = value; }

    private int forbidRange;
    public int ForbidRange { get => forbidRange; set => forbidRange = value; }

    //�����ӳ�
    private float initAttackIntensify;
    private float initSpeedIntensify;
    private int initRangeIntensify;
    private float initCriticalRateIntensify;
    private float initCriticalPercentageIntensify;
    private float initSlowRateIntensify;
    private float initSputteringRangeIntensify;
    private float initSputteringPercentageIntensify;
    public float InitAttackIntensify { get => initAttackIntensify; set => initAttackIntensify = value; }
    public float InitSpeedIntensify { get => initSpeedIntensify; set => initSpeedIntensify = value; }
    public int InitRangeIntensify { get => initRangeIntensify; set => initRangeIntensify = value; }
    public float InitCriticalRateIntensify { get => initCriticalRateIntensify; set => initCriticalRateIntensify = value; }
    public float InitCriticalPercentageIntensify { get => initCriticalPercentageIntensify; set => initCriticalPercentageIntensify = value; }
    public float InitSlowRateIntensify { get => initSlowRateIntensify; set => initSlowRateIntensify = value; }
    public float InitSputteringRangeIntensify { get => initSputteringRangeIntensify; set => initSputteringRangeIntensify = value; }
    public float InitSputteringPercentageIntensify { get => initSputteringPercentageIntensify; set => initSputteringPercentageIntensify = value; }

    private int baseTargetCountIntensify;
    public float BaseAttackIntensify { get => InitAttackIntensify + ComAttackIntensify; }
    public float BaseSpeedIntensify { get => InitSpeedIntensify + ComSpeedIntensify; }
    public int BaseRangeIntensify { get => InitRangeIntensify + ComRangeIntensify; }
    public float BaseCriticalRateIntensify { get => initCriticalRateIntensify + ComCriticalIntensify; }
    public float BaseCriticalPercentageIntensify { get => InitCriticalPercentageIntensify; }
    public float BaseSlowRateIntensify { get => InitSlowRateIntensify + ComSlowIntensify; }
    public float BaseSputteringPercentageIntensify { get => InitSputteringPercentageIntensify; }
    public float BaseSputteringRangeIntensify { get => InitSputteringRangeIntensify + ComSputteringRangeIntensify; }
    public int BaseTargetCountIntensify { get => baseTargetCountIntensify; set => baseTargetCountIntensify = value; }

    #region Ԫ�ؼӳ�
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
    public float ComSputteringRangeIntensify { get => comSputteringRangeIntensify; set => comSputteringRangeIntensify = value; }
    public float ComSlowIntensify { get => comSlowIntensify; set => comSlowIntensify = value; }

    #endregion

    #region ���мӳɶ�������
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
    public float BaseSputteringRangeIntensifyModify { get => baseSputteringRangeIntensifyModify; set => baseSputteringRangeIntensifyModify = value; }
    public float BaseSputteringPercentageIntensifyModify { get => baseSputteringPercentageIntensifyModify; set => baseSputteringPercentageIntensifyModify = value; }
    public float BaseSlowRateIntensifyModify { get => baseSlowRateIntensifyModify; set => baseSlowRateIntensifyModify = value; }
    public float BaseCriticalPercentageIntensifyModify { get => baseCriticalPercentageIntensifyModify; set => baseCriticalPercentageIntensifyModify = value; }
    public float PoloIntensifyModify { get => poloIntensifyModify; set => poloIntensifyModify = value; }
    #endregion

    //���ջ�������
    private int turnDamage;
    public int TurnDamage { get => turnDamage; set => turnDamage = value; }
    private int totalDamage;
    public int TotalDamage { get => totalDamage; set => totalDamage = value; }
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
    public float FinalAttack { get => (BaseAttack * (1 + (TurnAttackIntensify - 1) * AllAttackIntensifyModify) + TurnFixAttack * AllAttackIntensifyModify) * AttackAdjust; }
    public float FinalSpeed { get => Mathf.Min(30, (BaseSpeed * (1 + (TurnSpeedIntensify - 1) * AllSpeedIntensifyModify) + TurnFixSpeed * AllSpeedIntensifyModify) * SpeedAdjust); }//�ٶ�����30
    public int FinalRange { get => BaseRange + TurnFixRange; }
    public float FinalCriticalRate { get => (BaseCriticalRate * (1 + (TurnCriticalRateIntensify - 1) * AllCriticalIntensifyModify) + TurnFixCriticalRate * AllCriticalIntensifyModify) * CritialAdjust; }
    public float FinalCriticalPercentage { get => ((BaseCriticalPercentage + FinalCriticalRate) * TurnCriticalRateIntensify + TurnFixCriticalPercentage) * CritialAdjust; }

    public float FinalSputteringRange { get => (BaseSputteringRange * TurnSputteringRangeIntensify + TurnFixSputteringRange) * SplashAdjust; }
    public float FinalSputteringPercentage { get => BaseSputteringPercentage * TurnSputteringPercentageIntensify + TurnFixSputteringPercentage; }
    public float FinalSlowRate { get => (BaseSlowRate * TurnSlowRateIntensify + TurnFixSlowRate) * SlowAdjust; }
    public int FinalTargetCount { get => BaseTargetCount + TurnFixTargetCount; }

    //ȫ������ֵ
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


    #region �¼�����
    public float NextAttack { get => m_Att.TurretLevels[Quality].AttackDamage * (1 + BaseAttackIntensify); }
    public float NextSpeed { get => m_Att.TurretLevels[Quality].AttackSpeed * (1 + BaseSpeedIntensify); }
    public float NextSputteringRange { get => m_Att.TurretLevels[Quality].SputteringRange + BaseSputteringRangeIntensify; }
    public float NextCriticalRate { get => m_Att.TurretLevels[Quality].CriticalRate + BaseCriticalRateIntensify; }
    public float NextSlowRate { get => m_Att.TurretLevels[Quality].SlowRate + BaseSlowRateIntensify; }
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

    public void GetTurretSkills()//�״λ�ȡ������Ч��
    {
        TurretSkills.Clear();

        TurretSkill effect = TurretEffectFactory.GetInitialSkill((int)m_Att.TurretSkill);//�Դ�����
        TurretSkill = effect;
        TurretSkill.strategy = this;
        TurretSkills.Add(effect);
        TurretSkill.Build();

        //Ԫ����ϼ���
        List<int> elements = new List<int>();
        foreach (var com in CompositeBluePrint.Compositions)
        {
            elements.Add(com.elementRequirement);
        }
        ElementSkill effect2 = TurretEffectFactory.GetElementSkill(elements);
        AddElementSkill(effect2);

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
        InitSpeed = m_Att.TurretLevels[Quality - 1].AttackSpeed;
        InitRange = m_Att.TurretLevels[Quality - 1].AttackRange;
        InitCriticalRate = m_Att.TurretLevels[Quality - 1].CriticalRate;
        InitSputteringRange = m_Att.TurretLevels[Quality - 1].SputteringRange;
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
        //�����ӳ�
        InitAttackIntensify = 0;
        InitSpeedIntensify = 0;
        InitRangeIntensify = 0;
        InitCriticalRateIntensify = 0;
        InitCriticalPercentageIntensify = 0;
        InitSputteringRangeIntensify = 0;
        InitSputteringPercentageIntensify = 0;
        InitSlowRateIntensify = 0;
        BaseTargetCountIntensify = 0;

        //�����ӳɶ�������
        AllAttackIntensifyModify = 1;
        AllSpeedIntensifyModify = 1;
        AllCriticalIntensifyModify = 1;
        BaseCriticalPercentageIntensifyModify = 1;
        BaseSputteringRangeIntensifyModify = 1;
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
        ////�غ�ʱ������
        //TimeModify = 1;

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

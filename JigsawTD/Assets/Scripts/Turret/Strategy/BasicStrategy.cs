using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStrategy
{
    public virtual StrategyType strategyType => StrategyType.Element;
    public BasicStrategy(TurretAttribute attribute,int quality,TurretContent turret)
    {
        m_Att = attribute;
        Quality = quality;
        m_Turret = turret;
        GetTurretEffects();
    }

    public TurretContent m_Turret;
    public TurretAttribute m_Att;

    private int quality = 0;
    public int Quality { get => quality; set => quality = value; }
    //公开属性
    private int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }
    public virtual float AttackDamage { get => (m_Att.TurretLevels[Quality - 1].AttackDamage) * (1 + BaseAttackIntensify) * (1 + AttackIntensify); }
    public virtual int AttackRange { get => m_Att.TurretLevels[Quality - 1].AttackRange + RangeIntensify; }
    public int ForbidRange { get => m_Att.TurretLevels[Quality - 1].ForbidRange; }
    public virtual float AttackSpeed { get => (m_Att.TurretLevels[Quality - 1].AttackSpeed) * (1 + SpeedIntensify); }
    public float BulletSpeed { get => m_Att.BulletSpeed; }
    public virtual float SputteringRange { get => m_Att.TurretLevels[Quality - 1].SputteringRange + SputteringIntensify; }
    public float CriticalRate { get => m_Att.TurretLevels[Quality - 1].CriticalRate + CriticalIntensify; }
    public float SlowRate { get => m_Att.TurretLevels[Quality - 1].SlowRate + SlowIntensify; }

    //隐藏属性
    float sputteringRate = 0.5f;//溅射伤害率
    public float SputteringRate { get => sputteringRate; set => sputteringRate = value; }

    float criticalPercentage = 1.5f;//暴击伤害率
    public float CriticalPercentage { get => criticalPercentage; set => criticalPercentage = value; }

    int targetCount = 1;//目标数
    public int TargetCount { get => targetCount; set => targetCount = value; }

    //攻击力加成
    public virtual float AttackIntensify { get => TileAttackIntensify + TurnAttackIntensify; }//最终攻击加成

    float turnAttackIntensify;//回合临时加成
    public float TurnAttackIntensify { get => turnAttackIntensify; set => turnAttackIntensify = value; }

    float tileAttackIntensify;//地形加成
    public float TileAttackIntensify { get => tileAttackIntensify; set => tileAttackIntensify = value; }

    float baseAttackIntensify;//基础修正，只加成基础攻击
    public float BaseAttackIntensify { get => baseAttackIntensify; set => baseAttackIntensify = value; }
    //********************

    //范围加成
    public int RangeIntensify { get => TileRangeIntensify; }//最终范围加成
    int tileRangeIntensify;//地形加成
    public int TileRangeIntensify { get => tileRangeIntensify; set => tileRangeIntensify = value; }
    //********************

    //攻速加成
    public virtual float SpeedIntensify { get => TurnSpeedIntensify; }

    float turnSpeedIntensify;
    public float TurnSpeedIntensify { get => turnSpeedIntensify; set => turnSpeedIntensify = value; }
    //*********************

    //暴击率加成
    public virtual float CriticalIntensify { get; }
    //减速效果加成
    public virtual float SlowIntensify { get; }
    //溅射范围加成
    public virtual float SputteringIntensify { get; }

    public List<TurretSkillInfo> TurretEffectInfos => m_Att.TurretEffects;
    public List<TurretSkill> TurretEffects = new List<TurretSkill>();

    public void GetTurretEffects()//获取并激活效果,或更新效果
    {
        TurretEffects.Clear();
        ClearIntensify();
        foreach (TurretSkillInfo info in TurretEffectInfos)
        {
            TurretSkill effect = TurretEffectFactory.GetEffect((int)info.EffectName);
            //effect.strategy = this;
            effect.KeyValue = info.KeyValue;
            TurretEffects.Add(effect);
            effect.Build();
        }
    }

    public void ClearIntensify()
    {
        TileAttackIntensify = 0;
        BaseAttackIntensify = 0;
        TileRangeIntensify = 0;
        CriticalPercentage = 1.5f;
        SputteringRate = 0.5f;
        TargetCount = 1;
    }

    public void ClearTurnIntensify()
    {
        TurnSpeedIntensify = 0;
        TurnAttackIntensify = 0;
    }
}

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
    //��������
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

    //��������
    float sputteringRate = 0.5f;//�����˺���
    public float SputteringRate { get => sputteringRate; set => sputteringRate = value; }

    float criticalPercentage = 1.5f;//�����˺���
    public float CriticalPercentage { get => criticalPercentage; set => criticalPercentage = value; }

    int targetCount = 1;//Ŀ����
    public int TargetCount { get => targetCount; set => targetCount = value; }

    //�������ӳ�
    public virtual float AttackIntensify { get => TileAttackIntensify + TurnAttackIntensify; }//���չ����ӳ�

    float turnAttackIntensify;//�غ���ʱ�ӳ�
    public float TurnAttackIntensify { get => turnAttackIntensify; set => turnAttackIntensify = value; }

    float tileAttackIntensify;//���μӳ�
    public float TileAttackIntensify { get => tileAttackIntensify; set => tileAttackIntensify = value; }

    float baseAttackIntensify;//����������ֻ�ӳɻ�������
    public float BaseAttackIntensify { get => baseAttackIntensify; set => baseAttackIntensify = value; }
    //********************

    //��Χ�ӳ�
    public int RangeIntensify { get => TileRangeIntensify; }//���շ�Χ�ӳ�
    int tileRangeIntensify;//���μӳ�
    public int TileRangeIntensify { get => tileRangeIntensify; set => tileRangeIntensify = value; }
    //********************

    //���ټӳ�
    public virtual float SpeedIntensify { get => TurnSpeedIntensify; }

    float turnSpeedIntensify;
    public float TurnSpeedIntensify { get => turnSpeedIntensify; set => turnSpeedIntensify = value; }
    //*********************

    //�����ʼӳ�
    public virtual float CriticalIntensify { get; }
    //����Ч���ӳ�
    public virtual float SlowIntensify { get; }
    //���䷶Χ�ӳ�
    public virtual float SputteringIntensify { get; }

    public List<TurretSkillInfo> TurretEffectInfos => m_Att.TurretEffects;
    public List<TurretSkill> TurretEffects = new List<TurretSkill>();

    public void GetTurretEffects()//��ȡ������Ч��,�����Ч��
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

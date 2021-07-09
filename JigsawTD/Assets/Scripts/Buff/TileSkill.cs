using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileSkillName
{
    DoubleDustElement,DoubleFireElement,DoubleWaterElement,DoubleWoodElement,DoubleGoldElement
}

public abstract class TileSkill : TurretSkill
{
    public abstract TileSkillName TileSkillName { get; }
    public GameTile m_GameTile;
}

public class DoubleDustElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleDustElement;
    public override string SkillDescription => "����������ϳ����Ļ�������ӳ�Ч������";

    public override void Build()
    {
        strategy.BaseSputteringRangeIntensifyModify *= 2f;
    }
}

public class DoubleGoldElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleGoldElement;
    public override string SkillDescription => "�����������ϳ����Ļ��������ӳ�Ч������";
    public override void Build()
    {
        strategy.BaseAttackIntensifyModify *= 2f;
    }
}

public class DoubleWoodElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleWoodElement;
    public override string SkillDescription => "���ٻ������ϳ����Ļ������ټӳ�Ч������";
    public override void Build()
    {
        strategy.BaseSpeedIntensifyModify *= 2f;
    }
}

public class DoubleWaterElement: TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleWaterElement;
    public override string SkillDescription => "���ٻ������ϳ����Ļ������ټӳ�Ч������";
    public override void Build()
    {
        strategy.BaseSlowRateIntensifyModify *= 2f;
    }
}

public class DoubleFireElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleFireElement;
    public override string SkillDescription => "�����������ϳ����Ļ��������ӳ�Ч������";
    public override void Build()
    {
        strategy.BaseCriticalRateIntensifyModify *= 2f;
    }
}

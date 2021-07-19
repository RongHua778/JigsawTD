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
    public override string SkillDescription => "溅射基座：合成塔的土结构加成效果翻倍";

    public override void Build()
    {
        strategy.BaseSputteringRangeIntensifyModify *= 2f;
    }
}

public class DoubleGoldElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleGoldElement;
    public override string SkillDescription => "攻击基座：合成塔的金结构加成效果翻倍";
    public override void Build()
    {
        strategy.AllAttackIntensifyModify *= 2f;
    }
}

public class DoubleWoodElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleWoodElement;
    public override string SkillDescription => "攻速基座：合成塔的木结构加成效果翻倍";
    public override void Build()
    {
        strategy.AllSpeedIntensifyModify +=1;
    }
}

public class DoubleWaterElement: TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleWaterElement;
    public override string SkillDescription => "减速基座：合成塔的水结构加成效果翻倍";
    public override void Build()
    {
        strategy.BaseSlowRateIntensifyModify *= 2f;
    }
}

public class DoubleFireElement : TileSkill
{
    public override TileSkillName TileSkillName => TileSkillName.DoubleFireElement;
    public override string SkillDescription => "暴击基座：合成塔的火结构加成效果翻倍";
    public override void Build()
    {
        strategy.BaseCriticalRateIntensifyModify *= 2f;
    }
}

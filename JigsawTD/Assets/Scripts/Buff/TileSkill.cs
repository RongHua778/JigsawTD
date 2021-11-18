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

//public class DoubleDustElement : TileSkill
//{
//    public override TileSkillName TileSkillName => TileSkillName.DoubleDustElement;
//    public override string SkillDescription => "SPUTTERINGBASEINFO";

//    public override void Build()
//    {
//        base.Build();
//        strategy.InitSplashRangeModify += 1;
//    }
//}

//public class DoubleGoldElement : TileSkill
//{
//    public override TileSkillName TileSkillName => TileSkillName.DoubleGoldElement;
//    public override string SkillDescription => "ATTACKBASEINFO";
//    public override void Build()
//    {
//        base.Build();
//        strategy.AllAttackIntensifyModify *= 2f;
//    }
//}

//public class DoubleWoodElement : TileSkill
//{
//    public override TileSkillName TileSkillName => TileSkillName.DoubleWoodElement;
//    public override string SkillDescription => "SPEEDBASEINFO";
//    public override void Build()
//    {
//        base.Build();
//        strategy.AllSpeedIntensifyModify +=1;
//    }
//}

//public class DoubleWaterElement: TileSkill
//{
//    public override TileSkillName TileSkillName => TileSkillName.DoubleWaterElement;
//    public override string SkillDescription => "SLOWBASEINFO";
//    public override void Build()
//    {
//        base.Build();
//        strategy.InitSlowRateModify += 1;
//    }
//}

//public class DoubleFireElement : TileSkill
//{
//    public override TileSkillName TileSkillName => TileSkillName.DoubleFireElement;
//    public override string SkillDescription => "CRITICALBASEINFO";
//    public override void Build()
//    {
//        base.Build();
//        strategy.AllCriticalIntensifyModify *= 2f;
//    }
//}

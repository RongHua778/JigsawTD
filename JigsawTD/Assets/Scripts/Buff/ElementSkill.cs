using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ElementSkillInfo
{
    public List<int> Elements;
}
public abstract class ElementSkill : TurretSkill
{
    public abstract List<int> Elements { get; }
}

public class AAAHeavyCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 0 };
    public override TurretEffectName EffectName => TurretEffectName.AAA_HeavyCannon;
    public override string SkillDescription => "沉重炮口：炮塔转速大幅降低，所有基础攻击力提升效果翻倍";
    public override void BuildEnd()
    {
        strategy.BaseAttackIntensify *= 2;
        strategy.RotSpeed *= 0.05f;
    }
}

public class BBBOverloadCartridge : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 1 };
    public override TurretEffectName EffectName => TurretEffectName.BBB_OverloadCartridge;
    public override string SkillDescription => "过载弹夹：每回合开始前8秒，攻速提升100%";
    public override void StartTurn()
    {
        Duration += 8;
        strategy.TurnSpeedIntensify *= 2f;
    }

    public override void TickEnd()
    {
        strategy.TurnSpeedIntensify *= 0.5f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class CCCFrostCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "霜冻核心：合成时，获得1个混沌元素。";

    public override void Build()
    {
       
    }
}

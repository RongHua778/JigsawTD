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
    public override string SkillDescription => "沉重炮口：炮塔转速-95%，所有基础攻击力提升效果翻倍";
    public override void BuildEnd()
    {
        strategy.BaseAttackIntensify *= 2;
        strategy.RotSpeed *= 0.05f;
    }
}

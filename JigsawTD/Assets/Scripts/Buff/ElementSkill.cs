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
    public override string SkillDescription => "�����ڿڣ�����ת�ٴ�����ͣ����л�������������Ч������";
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
    public override string SkillDescription => "���ص��У�ÿ�غϿ�ʼǰ8�룬��������100%";
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
    public override string SkillDescription => "˪�����ģ��ϳ�ʱ�����1������Ԫ�ء�";

    public override void Build()
    {
       
    }
}

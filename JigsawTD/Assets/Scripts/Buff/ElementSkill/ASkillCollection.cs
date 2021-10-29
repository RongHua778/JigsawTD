using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : ElementSkill
{
    //所有攻击提升效果翻倍
    public override List<int> Elements => new List<int> { 0, 0, 0 };


    public override void Build()
    {
        base.Build();
        strategy.AllAttackIntensifyModify += 1;
    }
}

public class NearSpeed : ElementSkill
{
    //如果距离小于3，则攻速提升50%
    public override List<int> Elements => new List<int> { 0, 0, 1 };
    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = 0.6f * strategy.TimeModify;
                strategy.TurnSpeedIntensify += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnSpeedIntensify -= intensifyValue;
                isIntensified = false;
            }
        }
    }

    public override void EndTurn()
    {
        isIntensified = false;
        intensifyValue = 0;
    }
}

public class NearSlow : ElementSkill
{
    //如果距离小于3，则减速效果提高1
    public override List<int> Elements => new List<int> { 0, 0, 2 };

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SlowRate += (1 * strategy.TimeModify);
        }
    }
}

public class NearCritical : ElementSkill
{
    //如果距离小于3，则子弹暴击率提高35%
    public override List<int> Elements => new List<int> { 0, 0, 3 };

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.CriticalRate += 0.4f * strategy.TimeModify;

        }
    }
}
public class NearSplash : ElementSkill
{
    //如果距离小于3，则子弹暴击率提高35%
    public override List<int> Elements => new List<int> { 0, 0, 4 };

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SputteringRange += (0.6f * strategy.TimeModify);
        }
    }
}









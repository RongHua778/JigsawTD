using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : ElementSkill
{
    //所有攻击提升效果翻倍
    public override List<int> Elements => new List<int> { 0, 0, 0 };

    public override void Composite()
    {
        GameRes.TempGoldIntensify += 0.15f;

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
                intensifyValue = 0.5f * strategy.TimeModify;
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
    //如果距离小于3，则减速效果提高50%
    public override List<int> Elements => new List<int> { 0, 0, 2 };

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SlowRate *= (1 + 0.5f * strategy.TimeModify);
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
            bullet.CriticalRate += 0.35f * strategy.TimeModify;

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
            bullet.SputteringRange *= (1 + 0.5f * strategy.TimeModify);
        }
    }
}



//public class RandomAttack : ElementSkill
//{
//    //子弹攻击力在50%-200%之间随机浮动
//    public override List<int> Elements => new List<int> { 0, 0, 3 };
//    public override string SkillDescription => "RANDOMATTACK";

//    public override void Shoot(Bullet bullet = null)
//    {
//        bullet.Damage *= Random.Range(0.5f, 2f);
//    }
//}






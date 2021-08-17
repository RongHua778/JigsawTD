using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoloGetter : ElementSkill
{
    //受到的所有光环效果翻倍
    public override List<int> Elements => new List<int> { 4, 4, 4 };
    public override string SkillDescription => "POLOGETTER";

    public override void Build()
    {
        base.Build();
        strategy.PoloIntensifyModify += 1;
    }
}

public class NearSplash : ElementSkill
{
    //如果距离小于3，则溅射伤害提高100%
    public override List<int> Elements => new List<int> { 4, 4, 0 };
    public override string SkillDescription => "NEARSPLASH";

    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SputteringPercentage += 1f;
        }
    }

}

public class SplashSpeed : ElementSkill
{
    //每回合每次攻击提升0.03溅射，上限1.2
    public override List<int> Elements => new List<int> { 4, 4, 1 };
    public override string SkillDescription => "SPLASHSPEED";

    private float splashIncreased = 0;
    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        if (splashIncreased > 1.19f)
            return;
        splashIncreased += 0.03f*strategy.TimeModify;
        strategy.TurnFixCriticalPercentage += 0.03f * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        splashIncreased = 0;
    }
}

public class LateSplash : ElementSkill
{
    //开局30秒后溅射提高1
    public override List<int> Elements => new List<int> { 4, 4, 2 };
    public override string SkillDescription => "LATESPLASH";

    public override void StartTurn()
    {
        Duration += 30;
    }

    public override void TickEnd()
    {
        strategy.TurnFixSputteringRange += 0.8f * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class RandomSplash : ElementSkill
{
    //子弹溅射范围增加0-1的随机浮动
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override string SkillDescription => "RANDOMSPLASH";

    public override void PreHit(Bullet bullet = null)
    {
        bullet.SputteringRange += Random.Range(0, 1f);
    }
}

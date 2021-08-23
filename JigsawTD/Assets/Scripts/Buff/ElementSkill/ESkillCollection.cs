using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoloGetter : ElementSkill
{
    //�ܵ������й⻷Ч������
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
    //�������С��3�������˺����100%
    public override List<int> Elements => new List<int> { 4, 4, 0 };
    public override string SkillDescription => "NEARSPLASH";

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SputteringPercentage += 1f;
        }
    }

}

public class SplashSpeed : ElementSkill
{
    //ÿ�غ�ÿ�ι�������0.02����
    public override List<int> Elements => new List<int> { 4, 4, 1 };
    public override string SkillDescription => "SPLASHSPEED";

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSputteringRange += 0.02f * strategy.TimeModify;
    }


}

public class LateSplash : ElementSkill
{
    //ÿ�غϿ�ʼ�󣬽��䷶Χÿ������0.01
    public override List<int> Elements => new List<int> { 4, 4, 2 };
    public override string SkillDescription => "LATESPLASH";

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixSputteringRange += 0.01f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class RandomSplash : ElementSkill
{
    //�ӵ����䷶Χ����0-1���������
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override string SkillDescription => "RANDOMSPLASH";

    public override void PreHit(Bullet bullet = null)
    {
        bullet.SputteringRange += Random.Range(0, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashPolo : ElementSkill
{
    //����ÿ���������������0.3����
    public override List<int> Elements => new List<int> { 4, 4, 4 };
    public override string SkillDescription => "SPLASHPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseSputteringRangeIntensify -= 0.3f * strategy.PoloIntensifyModify;
        }
        intensifiedStrategies.Clear();
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
            {
                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
                strategy.BaseSputteringRangeIntensify += 0.3f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class NearSplash : ElementSkill
{
    //�������С��3�������˺����100%
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
    //ÿ�غ�ÿ�ι�������0.06���䣬����1.2
    public override List<int> Elements => new List<int> { 4, 4, 1 };
    public override string SkillDescription => "SPLASHSPEED";

    private float splashIncreased = 0;
    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        if (splashIncreased > 1.19f)
            return;
        splashIncreased += 0.06f;
        strategy.TurnFixCriticalPercentage += 0.06f;
    }

    public override void EndTurn()
    {
        splashIncreased = 0;
    }
}

public class LateSplash : ElementSkill
{
    //����30��������1.2
    public override List<int> Elements => new List<int> { 4, 4, 2 };
    public override string SkillDescription => "LATESPLASH";

    public override void StartTurn()
    {
        Duration += 30;
    }

    public override void TickEnd()
    {
        strategy.TurnFixSputteringRange += 1.2f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class RandomSplash : ElementSkill
{
    //�ӵ����䷶Χ����0-1.5���������
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override string SkillDescription => "RANDOMSPLASH";

    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        bullet.SputteringRange += Random.Range(0, 1.5f);
    }
}

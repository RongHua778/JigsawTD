using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateIntensify : ElementSkill
{
    //所有30秒后发生的属性提升翻倍
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override string SkillDescription => "LATEINTENSIFY";
    public override void StartTurn()
    {
        Duration = 30;
    }

    public override void TickEnd()
    {
        strategy.TimeModify += 2;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class LateAttack : ElementSkill
{
    //战斗开始后，攻击力每秒提升2%
    public override List<int> Elements => new List<int> { 2, 2, 0 };

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnAttackIntensify += 0.02f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}
public class LateSpeed : ElementSkill
{
    //每回合开始后攻速每秒提升2%
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override string SkillDescription => "LATESPEED";

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnSpeedIntensify += 0.02f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class LateCritical : ElementSkill
{
    //每回合开始后，暴击率每秒提升1%
    public override List<int> Elements => new List<int> { 2, 2, 3 };

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixCriticalRate += 0.01f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class LateSplash : ElementSkill
{
    //每回合开始后，溅射范围每秒提升0.01
    public override List<int> Elements => new List<int> { 2, 2, 4 };

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
public class SlowPolo : ElementSkill
{
    //相邻防御塔减速提高0.5
    public override List<int> Elements => new List<int> { 2, 2, 4 };

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitSlowRateIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.InitSlowRateIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}







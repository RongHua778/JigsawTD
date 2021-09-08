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


public class AttackPolo : ElementSkill
{
    //相邻防御塔攻击力提高50%
    public override List<int> Elements => new List<int> { 4, 4, 0 };

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitAttackIntensify -= 0.3f * strategy.PoloIntensifyModify;
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
                strategy.InitAttackIntensify += 0.3f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }

}

public class SpeedPolo : ElementSkill
{
    //相邻防御塔攻速提高50%
    public override List<int> Elements => new List<int> { 4, 4, 1 };

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitSpeedIntensify -= 0.3f * strategy.PoloIntensifyModify;
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
                strategy.InitSpeedIntensify += 0.3f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class SlowAdjacent : ElementSkill
{
    //相邻每个防御塔提高自身0.5减速
    public override List<int> Elements => new List<int> { 4, 4, 2 };

    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.InitSlowRateIntensify -= 0.5f * adjacentTurretCount;//修复回初始值
        adjacentTurretCount = 0;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
                adjacentTurretCount++;
        }
        strategy.InitSlowRateIntensify += 0.5f * adjacentTurretCount;
    }
}

public class CriticalPolo : ElementSkill
{
    //相邻防御塔提高20%暴击
    public override List<int> Elements => new List<int> { 4, 4, 3 };

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitCriticalRateIntensify -= 0.2f * strategy.PoloIntensifyModify;
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
                strategy.InitCriticalRateIntensify += 0.2f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

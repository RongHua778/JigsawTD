using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPolo : ElementSkill
{
    //相邻每个防御塔提高自身25%暴击
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    public override string SkillDescription => "CRITICALPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseCriticalPercentageIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.BaseCriticalPercentageIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class RandomCritical : ElementSkill
{
    //暴击伤害增加-100%-400%的随机浮动
    public override List<int> Elements => new List<int> { 3, 3, 0 };
    public override string SkillDescription => "RANDOMCRITICAL";

    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        bullet.CriticalPercentage += Random.Range(-1f, 4f);
    }
}

public class CriticalSpeed : ElementSkill
{
    //每回合每次攻击提升5%暴击率，上限100%
    public override List<int> Elements => new List<int> { 3, 3, 1 };
    public override string SkillDescription => "CRITICALSPEED";

    private float criticalIncreased = 0;
    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        if (criticalIncreased > 0.99f)
            return;
        criticalIncreased += 0.05f;
        strategy.TurnFixCriticalPercentage += 0.05f;
    }

    public override void EndTurn()
    {
        criticalIncreased = 0;
    }
}

public class LateCritical : ElementSkill
{
    //开局30秒后暴击率提高50%
    public override List<int> Elements => new List<int> { 3, 3, 2 };
    public override string SkillDescription => "LATECRITICAL";

    public override void StartTurn()
    {
        Duration += 30;
    }

    public override void TickEnd()
    {
        strategy.TurnFixCriticalPercentage += 0.5f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class CriticalAdjacent : ElementSkill
{
    //相邻每个防御塔提高自身25%暴击率
    public override List<int> Elements => new List<int> { 3, 3, 4 };
    public override string SkillDescription => "CRITICALADJACENT";

    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.BaseCriticalPercentageIntensify -= 0.25f * adjacentTurretCount;//修复回初始值
        adjacentTurretCount = 0;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
                adjacentTurretCount++;
        }
        strategy.BaseCriticalPercentageIntensify += 0.25f * adjacentTurretCount;
    }
}

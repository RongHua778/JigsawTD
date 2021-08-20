using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTarget : ElementSkill
{
    //额外攻击2个目标
    public override List<int> Elements => new List<int> { 1, 1, 1 };
    public override string SkillDescription => "MULTITARGET";

    public override void Build()
    {
        base.Build();
        strategy.BaseTargetCountIntensify += 2;
    }
}



public class SpeedTarget : ElementSkill
{
    //攻击特效:对同一目标进行攻击时，每次攻击攻速提升10%，上限300%，切换目标重置
    public override List<int> Elements => new List<int> { 1, 1, 0 };
    public override string SkillDescription => "SPEEDTARGET";

    private Enemy lastTarget;
    private float speedIncreased;

    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        if (target != lastTarget)
        {
            lastTarget = target;
            strategy.TurnSpeedIntensify -= speedIncreased;
            speedIncreased = 0;
            return;
        }
        if (speedIncreased < 2.99f)
        {
            speedIncreased += 0.1f * strategy.TimeModify;
            strategy.TurnSpeedIntensify += 0.1f * strategy.TimeModify;
        }
    }

    public override void EndTurn()
    {
        lastTarget = null;
        speedIncreased = 0;
    }

}

public class LateSpeed : ElementSkill
{
    //开局30秒后攻速提高100%
    public override List<int> Elements => new List<int> { 1, 1, 2 };
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

public class SpeedCritical : ElementSkill
{
    //暴击后，使接下来2秒攻速提升50%
    public override List<int> Elements => new List<int> { 1, 1, 3 };
    public override string SkillDescription => "SPEEDCRITICAL";

    private float speedIncreased;
    private float timeCounter;
    private bool isIntensify;

    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            timeCounter = 2f;
            if (!isIntensify)
            {
                isIntensify = true;
                speedIncreased = 0.6f * strategy.TimeModify;
                strategy.TurnSpeedIntensify += 0.6f * strategy.TimeModify;
            }
        }
    }
    public override void Tick(float delta)
    {
        base.Tick(delta);
        if (timeCounter > 0)
        {
            timeCounter -= delta;
            if (timeCounter <= 0)
            {
                isIntensify = false;
                strategy.TurnSpeedIntensify -= speedIncreased;
            }
        }
        
    }

    public override void EndTurn()
    {
        speedIncreased = 0;
        isIntensify = false;
    }

}

public class SpeedPolo : ElementSkill
{
    //相邻防御塔攻速提高50%
    public override List<int> Elements => new List<int> { 1, 1, 4 };
    public override string SkillDescription => "SPEEDPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitSpeedIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.InitSpeedIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}
//public class SpeedAdjacent : ElementSkill
//{
//    //相邻每个防御塔提高自身50%攻速
//    public override List<int> Elements => new List<int> { 1, 1, 4 };
//    public override string SkillDescription => "SPEEDADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseSpeedIntensify -= 0.5f * adjacentTurretCount;//修复回初始值
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseSpeedIntensify += 0.5f * adjacentTurretCount;
//    }
//}

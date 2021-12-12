using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RangeSplash : ElementSkill
{
    //范围内每个敌人+0.2溅射范围
    public override List<int> Elements => new List<int> { 4, 4, 4 };

    public override void OnEnter(IDamageable target)
    {
        strategy.TurnFixSplashRange += 0.2f * strategy.TimeModify;
    }
    public override void OnExit(IDamageable target)
    {
        strategy.TurnFixSplashRange -= 0.2f * strategy.TimeModify;
    }
    //public override void PreHit(Bullet bullet = null)
    //{
    //    base.PreHit(bullet);
    //    bullet.Damage *= (1 + bullet.SputteredCount * 0.2f);
    //}
    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    Duration += 20;
    //}
    //public override void TickEnd()
    //{
    //    base.TickEnd();
    //    strategy.TimeModify = 1;
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    Duration = 0;
    //    strategy.TimeModify = 2;
    //}

}

public class CloseSplash : ElementSkill
{
    //近战溅射0.75
    public override List<int> Elements => new List<int> { 4, 4, 0 };

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SputteringRange += (0.75f * strategy.TimeModify);
        }
    }
}
public class HitSplash : ElementSkill
{
    //每击溅射0.03
    public override List<int> Elements => new List<int> { 4, 4, 1 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSplashRange += 0.03f * strategy.TimeModify;
    }
}
public class TimeSplash : ElementSkill
{
    //每秒+0.02溅射
    public override List<int> Elements => new List<int> { 4, 4, 2 };

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixSplashRange += 0.02f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}
public class EarlyCritical : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 3 };

    float targetValue;
    float currentValue;
    float counter = 2;

    public override void Build()
    {
        base.Build();
        strategy.ComSplashRangeIntensify += 0.25f;
    }
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 999;
    }
    public override void Tick(float delta)
    {
        base.Tick(delta);
        counter += delta;
        if (counter > 2f)
        {
            counter = 0;
            targetValue = Random.Range(0.25f, 2.5f);
            DOTween.To(() => currentValue, x => currentValue = x, targetValue, 2);
        }
        strategy.SplashAdjust = currentValue;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
        strategy.SplashAdjust = 1;
        currentValue = 1;
        counter = 2;
    }
}
//public class PoloGetter : ElementSkill
//{
//    //受到的所有光环效果翻倍
//    public override List<int> Elements => new List<int> { 4, 4, 4 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSputteringRangeIntensify -= 0.5f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSputteringRangeIntensify += 0.5f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}


//public class AttackPolo : ElementSkill
//{
//    //相邻防御塔攻击力提高50%
//    public override List<int> Elements => new List<int> { 4, 4, 0 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitAttackIntensify -= 0.3f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitAttackIntensify += 0.3f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }

//}

//public class SpeedPolo : ElementSkill
//{
//    //相邻防御塔攻速提高50%
//    public override List<int> Elements => new List<int> { 4, 4, 1 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSpeedIntensify -= 0.3f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSpeedIntensify += 0.3f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}

//public class SlowAdjacent : ElementSkill
//{
//    //相邻每个防御塔提高0.5减速
//    public override List<int> Elements => new List<int> { 4, 4, 2 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSlowRateIntensify -= 0.5f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSlowRateIntensify += 0.5f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}

//public class CriticalPolo : ElementSkill
//{
//    //相邻防御塔提高20%暴击
//    public override List<int> Elements => new List<int> { 4, 4, 3 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitCriticalRateIntensify -= 0.2f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitCriticalRateIntensify += 0.2f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}

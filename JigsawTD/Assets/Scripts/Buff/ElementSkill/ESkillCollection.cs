using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RangeSplash : ElementSkill
{
    //��Χ��ÿ������+0.2���䷶Χ
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
    //��ս����0.75
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
    //ÿ������0.03
    public override List<int> Elements => new List<int> { 4, 4, 1 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSplashRange += 0.03f * strategy.TimeModify;
    }
}
public class TimeSplash : ElementSkill
{
    //ÿ��+0.02����
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
//    //�ܵ������й⻷Ч������
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
//    //���ڷ��������������50%
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
//    //���ڷ������������50%
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
//    //����ÿ�����������0.5����
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
//    //���ڷ��������20%����
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

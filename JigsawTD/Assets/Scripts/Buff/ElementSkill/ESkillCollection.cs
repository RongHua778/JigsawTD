using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LateIntensify : ElementSkill
{
    //�ӵ�ÿ��0.1���䷶Χ����ʹ�����˺����8%
    public override List<int> Elements => new List<int> { 4, 4, 4 };

    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
    }
    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TimeModify = 1;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
        strategy.TimeModify = 2;
    }

}

public class EarlyAttack : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 0 };

    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 1f * strategy.TimeModify;
        strategy.TurnAttackIntensify += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnAttackIntensify -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
    }
}
public class EarlySpeed : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 1 };

    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 1f * strategy.TimeModify;
        strategy.TurnSpeedIntensify += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnSpeedIntensify -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
    }
}
public class EarlySlow : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 2 };

    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 1f * strategy.TimeModify;
        strategy.TurnFixSlowRate += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnFixSlowRate -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
    }
}
public class EarlyCritical : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 3 };

    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 0.75f * strategy.TimeModify;
        strategy.TurnFixCriticalRate += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnFixCriticalRate -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
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

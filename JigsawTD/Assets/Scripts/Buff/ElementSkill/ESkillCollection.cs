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


public class AttackPolo : ElementSkill
{
    //���ڷ��������������50%
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
    //���ڷ������������50%
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
    //����ÿ���������������0.5����
    public override List<int> Elements => new List<int> { 4, 4, 2 };

    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.InitSlowRateIntensify -= 0.5f * adjacentTurretCount;//�޸��س�ʼֵ
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
    //���ڷ��������20%����
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

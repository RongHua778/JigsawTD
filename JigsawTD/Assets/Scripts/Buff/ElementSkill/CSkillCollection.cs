using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExtraSkill : ElementSkill
{
    //获得2个额外的元素技能槽
    public override List<int> Elements => new List<int> { 2, 2, 2 };

    public override void Build()
    {
        base.Build();
        strategy.ElementSKillSlot += 2;
    }

}

public class CloseSlow : ElementSkill
{
    //近战减速1.5
    public override List<int> Elements => new List<int> { 2, 2, 0 };

    public override void Shoot(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SlowRate += (1.5f * strategy.TimeModify);
        }
    }
}
public class HitSlow : ElementSkill
{
    //每击减速0.04
    public override List<int> Elements => new List<int> { 2, 2, 1 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSlowRate += 0.05f * strategy.TimeModify;
    }

}

public class FlutSlow : ElementSkill
{
    //波动减速
    public override List<int> Elements => new List<int> { 2, 2, 3 };

    float targetValue;
    float currentValue;
    float counter = 2;

    public override void Build()
    {
        base.Build();
        strategy.ComSlowIntensify += 0.5f;
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
        strategy.SlowAdjust = currentValue;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
        strategy.SlowAdjust = 1;
        currentValue = 1;
        counter = 2;
    }
}

public class StartSlow : ElementSkill
{
    //开局减速4
    public override List<int> Elements => new List<int> { 2, 2, 4 };


    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 4f * strategy.TimeModify;
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
//public class SlowPolo : ElementSkill
//{
//    //相邻防御塔减速提高0.5
//    public override List<int> Elements => new List<int> { 2, 2, 4 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSlowRateModify -= 0.5f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSlowRateModify += 0.5f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}







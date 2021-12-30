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
    //近战减速
    public override List<int> Elements => new List<int> { 0, 0, 2 };
    public override float KeyValue => 1f * strategy.WaterCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WATER].Colorized((strategy == null ? 1f : KeyValue).ToString());
    public override ElementType IntensifyElement => ElementType.WATER;
    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = KeyValue;
                strategy.TurnFixSlowRate += intensifyValue;
                bullet.SlowRate += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnFixSlowRate -= intensifyValue;
                isIntensified = false;
            }
        }
    }

    public override void EndTurn()
    {
        isIntensified = false;
        intensifyValue = 0;
    }
}
public class HitSlow : ElementSkill
{
    //每击减速0.04
    public override List<int> Elements => new List<int> { 1, 1, 2 };
    public override float KeyValue => 0.02f * strategy.WaterCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WATER].Colorized(KeyValue.ToString());
    public override ElementType IntensifyElement => ElementType.WATER;

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSlowRate += KeyValue;
    }

}

public class LongSlow : ElementSkill
{
    //长距减速
    public override List<int> Elements => new List<int> { 3, 3, 2 };
    public override float KeyValue => 0.5f * strategy.WaterCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WATER].Colorized(KeyValue.ToString());
    public override ElementType IntensifyElement => ElementType.WATER;
    float intensifyValue;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSlowRate -= intensifyValue;
        float distance = bullet.GetTargetDistance();
        if (distance > 3)
        {
            intensifyValue = (bullet.GetTargetDistance() - 3) * KeyValue;
            strategy.TurnFixSlowRate += intensifyValue;
            bullet.SlowRate += intensifyValue;
        }
        else
        {
            intensifyValue = 0;
        }
    }

    public override void EndTurn()
    {
        intensifyValue = 0;
    }
}

public class StartSlow : ElementSkill
{
    //开局减速4
    public override List<int> Elements => new List<int> { 4, 4, 2 };

    public override float KeyValue => 1f * strategy.WaterCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WATER].Colorized(KeyValue.ToString());
    public override ElementType IntensifyElement => ElementType.WATER;
    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = KeyValue;
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







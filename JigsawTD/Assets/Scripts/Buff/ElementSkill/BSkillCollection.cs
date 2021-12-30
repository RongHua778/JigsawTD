using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SameTarget : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 1 };

    //public override void Build()
    //{
    //    base.Build();
    //    strategy.BaseTargetCountIntensify += 2;
    //}

    //public override void Shoot(Bullet bullet = null)
    //{
    //    bullet.Damage *= 0.65f;
    //}

    public override void StartTurn()
    {
        strategy.TurnFixTargetCount += strategy.TotalElementCount / 4;
    }
}
public class CloseSpeed : ElementSkill
{
    //距离小于3加75攻速
    public override List<int> Elements => new List<int> { 0, 0, 1 };
    public override float KeyValue => 0.5f * strategy.WoodCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WOOD].Colorized((strategy == null ? 0.5f : (KeyValue * 100)).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.WOOD;
    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = KeyValue;
                strategy.TurnFireRateIntensify += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnFireRateIntensify -= intensifyValue;
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

public class TimeSpeed : ElementSkill
{
    //每秒+2%攻速
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override float KeyValue => 0.01f * strategy.WoodCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WOOD].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.WOOD;
    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFireRateIntensify += KeyValue * delta;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class LongSpeed : ElementSkill
{
    //攻速波动
    public override List<int> Elements => new List<int> { 3, 3, 1 };
    public override float KeyValue => 0.25f * strategy.WoodCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WOOD].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.WOOD;
    float intensifyValue;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFireRateIntensify -= intensifyValue;
        float distance = bullet.GetTargetDistance();
        if (distance > 3)
        {
            intensifyValue = (bullet.GetTargetDistance() - 3) * KeyValue;
            strategy.TurnFireRateIntensify += intensifyValue;
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

public class StartSpeed : ElementSkill
{
    //开局+150%攻速
    public override List<int> Elements => new List<int> { 4, 4, 1 };

    public override float KeyValue => 0.5f * strategy.WoodCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.WOOD].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.WOOD;
    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = KeyValue;
        strategy.TurnFireRateIntensify += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnFireRateIntensify -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
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

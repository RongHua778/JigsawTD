using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RangeSplash : ElementSkill
{
    //范围内每个敌人+0.2溅射范围
    public override List<int> Elements => new List<int> { 4, 4, 4 };
    List<int> ElementsCount;
    int minValue;
    public override void StartTurn()
    {
        minValue = 999;
        ElementsCount = new List<int>();
        ElementsCount.Add(strategy.GoldCount);
        ElementsCount.Add(strategy.WoodCount);
        ElementsCount.Add(strategy.WaterCount);
        ElementsCount.Add(strategy.FireCount);
        ElementsCount.Add(strategy.DustCount);
        for (int i = 0; i < ElementsCount.Count; i++)
        {
            if (ElementsCount[i] != 0 && ElementsCount[i] < minValue)
            {
                minValue = ElementsCount[i];
            }
        }
        for (int i = 0; i < ElementsCount.Count; i++)
        {
            if (ElementsCount[i] == minValue)
            {
                switch (i)
                {
                    case 0:
                        strategy.TempGoldCount += 2;
                        break;
                    case 1:
                        strategy.TempWoodCount += 2;
                        break;
                    case 2:
                        strategy.TempWaterCount += 2;
                        break;
                    case 3:
                        strategy.TempFireCount += 2;
                        break;
                    case 4:
                        strategy.TempDustCount += 2;
                        break;
                }
            }
        }
    }

}

public class CloseSplash : ElementSkill
{
    //近战溅射0.75
    public override List<int> Elements => new List<int> { 0, 0, 4 };
    public override float KeyValue => 0.25f * strategy.DustCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.DUST].Colorized(KeyValue.ToString());
    public override ElementType IntensifyElement => ElementType.DUST;
    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = KeyValue;
                strategy.TurnFixSplashRange += intensifyValue;
                bullet.SplashRange += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnFixSplashRange -= intensifyValue;
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
public class HitSplash : ElementSkill
{
    //每击溅射0.01
    public override List<int> Elements => new List<int> { 1, 1, 4 };
    public override float KeyValue => 0.01f * strategy.DustCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.DUST].Colorized(KeyValue.ToString());
    public override ElementType IntensifyElement => ElementType.DUST;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSplashRange += KeyValue;
    }
}
public class TimeSplash : ElementSkill
{
    //每秒+0.02溅射
    public override List<int> Elements => new List<int> { 2, 2, 4 };
    public override float KeyValue => 0.005f * strategy.DustCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.DUST].Colorized((KeyValue).ToString());
    public override ElementType IntensifyElement => ElementType.DUST;
    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixSplashRange += KeyValue * delta;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}
public class LongSplash : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 4 };
    public override float KeyValue => 0.05f * strategy.DustCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.DUST].Colorized(KeyValue.ToString());
    public override ElementType IntensifyElement => ElementType.DUST;
    float intensifyValue;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSplashRange -= intensifyValue;
        float distance = bullet.GetTargetDistance();
        if (distance > 3)
        {
            intensifyValue = bullet.GetTargetDistance() * KeyValue;
            strategy.TurnFixSplashRange += intensifyValue;
            bullet.SplashRange += intensifyValue;
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

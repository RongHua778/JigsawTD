using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class RandomCritical : ElementSkill
{
    //暴击+25%，暴击伤害增加-100%-400%之间的随机波动
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    bool reRandom = false;
    int goldCount;
    int woodCount;
    int waterCount;
    int fireCount;
    int dustCount;

    public override void StartTurn()
    {
        int reRandomAmount = 0;
        if (strategy.TotalBaseCount != 0)
        {
            //记录全元素数量
            goldCount = strategy.BaseGoldCount;
            woodCount = strategy.BaseWoodCount;
            waterCount = strategy.BaseWaterCount;
            fireCount = strategy.BaseFireCount;
            dustCount = strategy.BaseDustCount;
            reRandomAmount = strategy.TotalBaseCount;
            //清空全元素
            strategy.BaseGoldCount = 0;
            strategy.BaseWoodCount = 0;
            strategy.BaseWaterCount = 0;
            strategy.BaseFireCount = 0;
            strategy.BaseDustCount = 0;
            reRandom = true;
        }
        reRandomAmount += 3;
        strategy.GainRandomTempElement(reRandomAmount);

    }

    public override void EndTurn()
    {
        if (reRandom)
        {
            strategy.BaseGoldCount = goldCount;
            strategy.BaseWoodCount = woodCount;
            strategy.BaseWaterCount = waterCount;
            strategy.BaseFireCount = fireCount;
            strategy.BaseDustCount = dustCount;
            reRandom = false;
        }
    }

}

public class CloseCritical : ElementSkill
{
    //近战暴击50%
    public override List<int> Elements => new List<int> { 0, 0, 3 };
    public override float KeyValue => 0.5f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = KeyValue;
                strategy.TurnFixCriticalPercentage += intensifyValue;
                bullet.CriticalPercentage += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnFixCriticalPercentage -= intensifyValue;
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

public class HitCritical : ElementSkill
{
    //每击暴伤50%
    public override List<int> Elements => new List<int> { 1, 1, 3 };
    public override float KeyValue => 0.02f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixCriticalPercentage += KeyValue;
    }
}

public class SlowCritical : ElementSkill
{
    //每秒+1.5%暴击率
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override float KeyValue => 0.01f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixCriticalPercentage += KeyValue * delta;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class StartCritical : ElementSkill
{
    //开局100%暴击
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override float KeyValue => 0.5f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = KeyValue;
        strategy.TurnFixCriticalPercentage += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnFixCriticalPercentage -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
    }

}

//public class CopySkill : ElementSkill
//{
//    //装备：复制第一个元素技能

//    public override List<int> Elements => new List<int> { 3, 3, 3 };
//    public override string SkillDescription => "COPYSKILL";

//    public override void OnEquip()
//    {
//        ElementSkill skill = strategy.TurretSkills[1] as ElementSkill;
//        if (skill.Elements.SequenceEqual(Elements))
//        {
//            Debug.Log("两个重复的复制技能");
//            return;
//        }
//        //strategy.GetComIntensify(Elements, false);
//        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(skill.Elements);
//        strategy.TurretSkills.Remove(this);
//        newSkill.Composite();//触发合成效果
//        strategy.AddElementSkill(newSkill);
//    }
//}
//public class AttackCritical : ElementSkill
//{
//    public override List<int> Elements => new List<int> { 3, 3, 0 };
//    private float attackIntensified;
//    private float timeCounter;
//    private bool isIntensify;

//    public override void StartTurn()
//    {
//        Duration += 999;
//    }
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            timeCounter = 2f;
//            if (!isIntensify)
//            {
//                isIntensify = true;
//                attackIntensified = 0.6f * strategy.TimeModify;
//                strategy.TurnAttackIntensify += 0.6f * strategy.TimeModify;
//            }
//        }
//    }
//    public override void Tick(float delta)
//    {
//        base.Tick(delta);
//        if (timeCounter >= 0)
//        {
//            timeCounter -= delta;
//            if (timeCounter <= 0)
//            {
//                isIntensify = false;
//                strategy.TurnAttackIntensify -= attackIntensified;
//            }
//        }

//    }

//    public override void EndTurn()
//    {
//        attackIntensified = 0;
//        isIntensify = false;
//    }

//}

//public class SpeedCritical : ElementSkill
//{
//    //暴击后，使接下来1秒攻速提升50%
//    public override List<int> Elements => new List<int> { 3, 3, 1 };

//    private float speedIncreased;
//    private float timeCounter;
//    private bool isIntensify;

//    public override void StartTurn()
//    {
//        Duration += 999;
//    }
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            timeCounter = 2f;
//            if (!isIntensify)
//            {
//                isIntensify = true;
//                speedIncreased = 0.5f * strategy.TimeModify;
//                strategy.TurnSpeedIntensify += 0.5f * strategy.TimeModify;
//            }
//        }
//    }
//    public override void Tick(float delta)
//    {
//        base.Tick(delta);
//        if (timeCounter >= 0)
//        {
//            timeCounter -= delta;
//            if (timeCounter <= 0)
//            {
//                isIntensify = false;
//                strategy.TurnSpeedIntensify -= speedIncreased;
//            }
//        }

//    }

//    public override void EndTurn()
//    {
//        speedIncreased = 0;
//        isIntensify = false;
//    }

//}

//public class CriticalSlow : ElementSkill
//{
//    //暴击造成的减速效果翻倍
//    public override List<int> Elements => new List<int> { 3, 3, 2 };
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SlowRate *= 2f;
//        }
//    }
//}

//public class CriticalSplash : ElementSkill
//{
//    //暴击造成的减速效果翻倍
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SputteringRange *= 2f;
//        }
//    }
//}








//public class CriticalAdjacent : ElementSkill
//{
//    //相邻每个防御塔提高自身25%暴击率
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override string SkillDescription => "CRITICALADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseCriticalPercentageIntensify -= 0.25f * adjacentTurretCount;//修复回初始值
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseCriticalPercentageIntensify += 0.25f * adjacentTurretCount;
//    }
//}

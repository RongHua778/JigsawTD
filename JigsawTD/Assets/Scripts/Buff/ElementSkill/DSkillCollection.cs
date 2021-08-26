using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CopySkill : ElementSkill
{
    //装备：复制第一个元素技能
    
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    public override string SkillDescription => "COPYSKILL";

    public override void OnEquip()
    {
        ElementSkill skill = strategy.TurretSkills[1] as ElementSkill;
        if (skill.Elements.SequenceEqual(Elements))
        {
            Debug.Log("两个重复的复制技能");
            return;
        }
        strategy.GetComIntensify(Elements, false);
        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(skill.Elements);
        strategy.TurretSkills.Remove(this);
        newSkill.Composite();//触发合成效果
        strategy.AddElementSkill(newSkill);
        
    }
}
public class CriticalPolo : ElementSkill
{
    //相邻防御塔提高25%暴击
    public override List<int> Elements => new List<int> { 3, 3, 4 };
    public override string SkillDescription => "CRITICALPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitCriticalRateIntensify -= 0.25f * strategy.PoloIntensifyModify;
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
                strategy.InitCriticalRateIntensify += 0.25f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class RandomCritical : ElementSkill
{
    //暴击伤害增加-100%-400%的随机浮动
    public override List<int> Elements => new List<int> { 3, 3, 0 };
    public override string SkillDescription => "RANDOMCRITICAL";

    public override void Shoot(Bullet bullet = null)
    {
        bullet.CriticalPercentage += Random.Range(-1f, 4f);
    }
}

public class CriticalSpeed : ElementSkill
{
    //攻击特效每回合每次攻击提升2.5%暴击率
    public override List<int> Elements => new List<int> { 3, 3, 1 };
    public override string SkillDescription => "CRITICALSPEED";

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixCriticalRate += 0.025f * strategy.TimeModify;
    }
}

public class LateCritical : ElementSkill
{
    //开局30秒后暴击率提高40%
    public override List<int> Elements => new List<int> { 3, 3, 2 };
    public override string SkillDescription => "LATECRITICAL";

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixCriticalRate += 0.01f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

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

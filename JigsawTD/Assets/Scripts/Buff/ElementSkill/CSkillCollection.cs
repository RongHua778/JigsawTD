using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateIntensify : ElementSkill
{
    //所有20秒后发生的属性提升翻倍
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override string SkillDescription => "LATEINTENSIFY";
    public override void StartTurn()
    {
        Duration = 20;
    }

    public override void TickEnd()
    {
        strategy.TimeModify += 1;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class SlowPolo : ElementSkill
{
    //相邻防御塔减速提高0.5
    public override List<int> Elements => new List<int> { 2, 2, 4 };
    public override string SkillDescription => "SLOWPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitSlowRateIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.InitSlowRateIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class SlowAttack : ElementSkill
{
    //对距离小于3的敌人造成减速效果翻倍
    public override List<int> Elements => new List<int> { 2, 2, 0 };
    public override string SkillDescription => "SLOWATTACK";
    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() <= 3)
        {
            bullet.SlowRate *= 2;
        }
    }
}

public class SlowSpeed : ElementSkill
{
    //攻击造成目标2%当前生命值的伤害
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override string SkillDescription => "SLOWSPEED";
    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        float damageIncreased = target.DamageStrategy.CurrentHealth * 0.02f;
        bullet.Damage += damageIncreased;
    }
}

public class CriticalSlow : ElementSkill
{
    //暴击造成的减速效果翻倍
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override string SkillDescription => "CRITICALSLOW";
    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            bullet.SlowRate *= 2f;
        }
    }
}

//public class SlowAdjacent : ElementSkill
//{
//    //相邻每个防御塔提高自身0.5减速
//    public override List<int> Elements => new List<int> { 2, 2, 4 };
//    public override string SkillDescription => "SLOWADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseSlowRateIntensify -= 0.5f * adjacentTurretCount;//修复回初始值
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseSlowRateIntensify += 0.5f * adjacentTurretCount;
//    }
//}

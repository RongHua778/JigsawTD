using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ElementSkillInfo
{
    public List<int> Elements;
}
public abstract class ElementSkill : TurretSkill
{
    public abstract List<int> Elements { get; }
}

public class AAAHeavyCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 0 };
    public override string SkillDescription => "沉重炮口：炮塔转速大幅降低，所有攻击力提升效果翻倍";
    public override void Build()
    {
        strategy.AllAttackIntensifyModify += 1;
        strategy.RotSpeed = 0.5f;
    }
}

public class BBBOverloadCartridge : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 1 };
    public override string SkillDescription => "过载弹夹：每回合开始前10秒，攻速提升200%";
    public override void StartTurn()
    {
        Duration += 10;
        strategy.TurnSpeedIntensify *= 3f;
    }

    public override void TickEnd()
    {
        strategy.TurnSpeedIntensify /= 3f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class CCCFrostCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override string SkillDescription => "霜冻核心：合成后，获得1个万能元素。";

    public override void Composite()
    {
        GameManager.Instance.GetPerfectElement(1);
    }

}

public class DDDRestlessGunpowder : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    public override string SkillDescription => "躁动火药：暴击伤害增加-100%到400%的随机浮动";

    public override void Shoot()
    {
        bullet.CriticalPercentage += Random.Range(-1f, 4f);
    }
}

public class EEENuclearShell : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 4 };
    public override string SkillDescription => "核能炮弹：溅射范围随子弹飞行距离提升0.3/格";

    public override void Shoot()
    {
        bullet.SputteringRange += bullet.GetTargetDistance() * 0.3f;
    }
}

public class AABChargedBase : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 1 };
    public override string SkillDescription => "充能基座：战斗开始前15秒，攻击力提升100%";

    public override void StartTurn()
    {
        Duration += 15;
        strategy.TurnAttackIntensify *= 2f;
    }

    public override void TickEnd()
    {
        strategy.TurnAttackIntensify /= 2f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class AACLonggerCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 2 };
    public override string SkillDescription => "加长炮管：防御塔攻击距离+2";

    public override void Build()
    {
        strategy.BaseRangeIntensify += 2;
    }

    //public override void PreHit()
    //{
    //    bullet.Damage *= (1 + bullet.SlowRate);
    //}
}

public class AADMetalBullet : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 3 };
    public override string SkillDescription => "合金子弹：每次暴击都会提升本回合攻击力10%，最多提高200%";

    private float attackIncreased;
    public override void PreHit()
    {
        if (bullet.isCritical)
        {
            if (attackIncreased > 1.95f)
                return;
            attackIncreased += 0.1f;
            strategy.TurnAttackIntensify += 0.1f;
        }
    }

    public override void EndTurn()
    {
        attackIncreased = 0;
    }
}

public class AAEDetectRadar : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 4 };
    public override string SkillDescription => "侦测雷达：相邻每有1个防御塔，就提升50%基础攻击力";

    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.BaseAttackIntensify -= 0.5f * adjacentTurretCount;//修复回初始值
        adjacentTurretCount = 0;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
                adjacentTurretCount++;
        }
        strategy.BaseAttackIntensify += 0.5f * adjacentTurretCount;
    }
}

public class ABCReinforceBase : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 2 };
    public override string SkillDescription => "加固底座：防御塔升级费用-50%";

    public override void Build()
    {
        strategy.UpgradeDiscount += 0.5f;
    }
}

public class ABDInvestment : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 3 };
    public override string SkillDescription => "量化投资：获得的金币提高15%";

    public override void Composite()
    {
        StaticData.OverallMoneyIntensify += 0.15f;
    }
}

public class ABEResourcesAllocation : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 4 };
    public override string SkillDescription => "资源配置：每5回合，获得1次额外抽取次数";

    private int turn;
    public override void EndTurn()
    {
        turn++;
        if (turn > 4)
        {
            turn = 0;
            GameManager.Instance.GainDraw(1);
        }
    }
}

public class ACDResourcesRecycle : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 2, 3 };
    public override string SkillDescription => "资源回收：合成时，获得3次额外抽取次数";

    public override void Composite()
    {
        GameManager.Instance.GainDraw(3);
    }
}

public class ACELonggerBarrel : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 2, 4 };
    public override string SkillDescription => "紧急重构：使你下一个购买的配方变为强化配方";

    public override void Composite()
    {
        StaticData.NextBuyIntensifyBlueprint++;
    }
}

public class ADEHardCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 3, 4 };
    public override string SkillDescription => "强击核心：相邻防御塔的基础攻击力提升50%";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseAttackIntensify -= 0.5f;
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
                strategy.BaseAttackIntensify += 0.5f;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class BBAProcessImprovement : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 0 };
    public override string SkillDescription => "改良流程：在每回合开始20秒后，攻速提升60%";

    public override void StartTurn()
    {
        Duration = 20;
    }

    public override void TickEnd()
    {
        strategy.TurnSpeedIntensify *= 1.6f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class BBCPreciseStrike : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 2 };
    public override string SkillDescription => "精准打击：攻击造成目标4%当前生命值的额外伤害（对BOSS为1%）";

    public override void Hit(Enemy target)
    {
        float realDamage;
        float extraDamage = target.CurrentHealth * (target.IsBoss ? 0.01f : 0.04f);
        target.ApplyDamage(extraDamage, out realDamage);
        strategy.DamageAnalysis += (int)realDamage;
    }

}

public class BBDBirdShoot : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 3 };
    public override string SkillDescription => "小鸟盲射：每次攻击后，提升5%暴击率，上限为200%";

    float criticalRateIncreased;
    public override void Shoot()
    {
        if (criticalRateIncreased > 1.99f)
            return;
        strategy.TurnFixCriticalRate += 0.05f;
        criticalRateIncreased += 0.05f;
    }

    public override void EndTurn()
    {
        criticalRateIncreased = 0;
    }
}

public class BBETinyCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 4 };
    public override string SkillDescription => "小型炮口：所有攻速提升效果翻倍，不可造成暴击";

    public override void Build()
    {
        strategy.AllSpeedIntensifyModify += 1;
    }

    public override void Shoot()
    {
        bullet.CriticalRate = 0;
    }
}

public class BCDMoneyFactory : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 2, 3 };
    public override string SkillDescription => "铸币工厂：防御塔每次造成暴击，都会使你获得1金币";

    public override void Hit(Enemy target)
    {
        if (bullet.isCritical)
        {
            GameManager.Instance.GainMoney(1);
        }
    }
}


public class BCERepairFactory : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 2, 4 };
    public override string SkillDescription => "维修工厂：相邻陷阱效果提升100%";

    private List<TrapContent> intensifiedTraps = new List<TrapContent>();
    public override void Detect()
    {
        foreach (var trap in intensifiedTraps)
        {
            trap.TrapIntensify -= 1f;
        }
        intensifiedTraps.Clear();
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TrapMask));
            if (hit != null)
            {
                TrapContent trap = hit.GetComponent<TrapContent>();
                trap.TrapIntensify += 1f;
                intensifiedTraps.Add(trap);
            }
        }
    }


}

public class ADESpeedCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 3, 4 };
    public override string SkillDescription => "加速核心：相邻防御塔的基础攻速提升50%";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseSpeedIntensify -= 0.5f;
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
                strategy.BaseSpeedIntensify += 0.5f;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class CCABlueprint : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 0 };
    public override string SkillDescription => "科技蓝图：合成后，随机获得一个强化配方";

    public override void Composite()
    {
        GameManager.Instance.GetRandomBluePrint(true);
    }
}
public class CCBFrostCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override string SkillDescription => "寒冰核心：相邻防御塔+0.5减速";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseSlowRateIntensify -= 0.5f;
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
                strategy.BaseSlowRateIntensify += 0.5f;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class CCDUnstableShaft : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override string SkillDescription => "不稳定轴：暴击造成的减速效果翻倍";

    public override void Hit(Enemy target)
    {
        if (bullet.isCritical)
        {
            bullet.SlowRate *= 2;
        }
    }
}

public class CCEIceBomb : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 4 };
    public override string SkillDescription => "加装炮口：造成的伤害减少50%，额外攻击2个目标";

    public override void Build()
    {
        strategy.BaseTargetCountIntensify += 2;
    }

    public override void Hit(Enemy target)
    {
        bullet.Damage *= 0.5f;
    }
}

public class CDETargetCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 3, 4 };
    public override string SkillDescription => "瞄准核心：相邻防御塔+1范围";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseRangeIntensify -= 1;
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
                strategy.BaseRangeIntensify += 1;
                strategy.m_Turret.GenerateRange();
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class DDASealedCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 0 };
    public override string SkillDescription => "尘封大炮：如果暴击率大于100%，则造成的伤害提高100%";

    public override void PreHit()
    {
        if (bullet.CriticalRate > 1)
        {
            bullet.Damage *= 2f;
        }
    }
}

public class DDBFireSuppression : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 1 };
    public override string SkillDescription => "火力压制：战斗开始前15秒，暴击率提升100%";

    public override void StartTurn()
    {
        Duration = 15f;
        strategy.TurnFixCriticalRate += 1f;
    }

    public override void TickEnd()
    {
        strategy.TurnFixCriticalRate -= 1f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class DDCVentureInvestment : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 2 };
    public override string SkillDescription => "烈焰核心：相邻防御塔的暴击率+30%";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseCriticalRateIntensify -= 0.3f;
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
                strategy.BaseCriticalRateIntensify += 0.3f;
                strategy.m_Turret.GenerateRange();
                intensifiedStrategies.Add(strategy);
            }
        }
    }

}

public class DDERemoteGuidence : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 4 };
    public override string SkillDescription => "远程制导：当攻击距离大于3的敌人时，暴击伤害提高100%";

    public override void Shoot()
    {
        if (bullet.GetTargetDistance() > 3f)
        {
            bullet.CriticalPercentage += 1f;
        }
    }

}

public class EEAPowerfulSputtering : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 0 };
    public override string SkillDescription => "强力溅射：攻击距离小于3的敌人时，溅射伤害提高100%";

    public override void Shoot()
    {
        if (bullet.GetTargetDistance() < 3f)
        {
            bullet.SputteringPercentage += 1f;
        }
    }

}
public class EEBHeatingBarrel : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 1 };
    public override string SkillDescription => "加热炮管：每次攻击后提升0.1溅射范围，上限1.5";

    float sputteringRangeIncreased;
    public override void Shoot()
    {
        if (sputteringRangeIncreased < 1.5f)
        {
            sputteringRangeIncreased += 0.1f;
            strategy.TurnFixSputteringRange += 0.1f;
        }
    }

    public override void EndTurn()
    {
        sputteringRangeIncreased = 0;
    }

}

public class EECConcretePouring : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 2 };
    public override string SkillDescription => "稳固核心：相邻防御塔的溅射范围+0.5";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseSputteringRangeIntensify -= 0.5f;
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
                strategy.BaseSputteringRangeIntensify += 0.5f;
                strategy.m_Turret.GenerateRange();
                intensifiedStrategies.Add(strategy);
            }
        }
    }

}

public class EEDWantonBombing : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override string SkillDescription => "狂轰滥炸：暴击造成的溅射伤害提高100%";

    public override void PreHit()
    {
        if (bullet.isCritical)
        {
            bullet.SputteringPercentage += 1f;
        }
    }

}



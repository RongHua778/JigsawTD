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
    public override string SkillDescription => "AAA";
    public override void Build()
    {
        strategy.AllAttackIntensifyModify *=2;
        strategy.RotSpeed = 0.5f;
    }
}

public class BBBOverloadCartridge : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 1 };
    public override string SkillDescription => "BBB";
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
    public override string SkillDescription => "CCC";
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

public class DDDRestlessGunpowder : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    public override string SkillDescription => "DDD";

    public override void Shoot(Bullet bullet = null)
    {
        bullet.CriticalPercentage += Random.Range(-1f, 4f);
    }
}

public class EEENuclearShell : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 4 };
    public override string SkillDescription => "EEE";

    public override void Shoot(Bullet bullet = null)
    {
        bullet.SputteringRange += bullet.GetTargetDistance() * 0.3f;
    }
}

public class AABChargedBase : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 1 };
    public override string SkillDescription => "AAB";

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
    public override string SkillDescription => "AAC";

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
    public override string SkillDescription => "AAD";

    private float attackIncreased;
    public override void PreHit(Bullet bullet = null)
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
    public override string SkillDescription => "AAE";

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
    public override string SkillDescription => "ABC";

    public override void Build()
    {
        strategy.UpgradeDiscount += 0.5f;
    }
}

public class ABDInvestment : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 3 };
    public override string SkillDescription => "ABD";

    public override void Composite()
    {
        StaticData.OverallMoneyIntensify += 0.15f;
    }
}

public class ABEResourcesAllocation : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 4 };
    public override string SkillDescription => "ABE";

    public override void Draw()
    {
        strategy.BaseAttackIntensify += 0.1f;
    }
    //private int turn;
    //public override void EndTurn()
    //{
    //    turn++;
    //    if (turn > 4)
    //    {
    //        turn = 0;
    //        GameManager.Instance.GainDraw(1);
    //    }
    //}
}

public class ACDResourcesRecycle : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 2, 3 };
    public override string SkillDescription => "ACD";

    public override void Composite()
    {
        GameManager.Instance.SetBuyShapeCostDiscount(0.3f);
    }
}

public class ACELonggerBarrel : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 2, 4 };
    public override string SkillDescription => "ACE";

    public override void Composite()
    {
        StaticData.NextBuyIntensifyBlueprint++;
    }
}

public class ADEHardCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 3, 4 };
    public override string SkillDescription => "ADE";
    public override void Composite()
    {
        GameManager.Instance.GetPerfectElement(1);
    }


}

public class BBAProcessImprovement : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 0 };
    public override string SkillDescription => "BBA";

    public override void StartTurn()
    {
        Duration = 30;
    }

    public override void TickEnd()
    {
        strategy.TurnSpeedIntensify += 1;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class BBCPreciseStrike : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 2 };
    public override string SkillDescription => "BBC";

    public override void Hit(Enemy target, Bullet bullet = null)
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
    public override string SkillDescription => "BBD";

    private float speedIncreased;
    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical && speedIncreased < 1.99f)
        {
            strategy.TurnSpeedIntensify += 0.1f;
            speedIncreased += 0.1f;
        }
    }

    public override void EndTurn()
    {
        speedIncreased = 0;
    }
}

public class BBETinyCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 4 };
    public override string SkillDescription => "BBE";

    public override void Build()
    {
        strategy.AllSpeedIntensifyModify += 1;
    }

    public override void Shoot(Bullet bullet = null)
    {
        bullet.CriticalRate = 0;
    }
}

public class BCDMoneyFactory : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 2, 3 };
    public override string SkillDescription => "BCD";

    public override void Hit(Enemy target, Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            GameManager.Instance.GainMoney(2);
        }
    }
}


public class BCERepairFactory : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 2, 4 };
    public override string SkillDescription => "BCE";

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
    public override string SkillDescription => "BDE";

    public override void Composite()
    {
        int money = Random.Range(100, 301);
        GameManager.Instance.GainMoney(money);
        GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("GETMONEY") + money);
    }

}

public class CCABlueprint : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 0 };
    public override string SkillDescription => "CCA";

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
public class CCBFrostCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override string SkillDescription => "CCB";

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

public class CCDUnstableShaft : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override string SkillDescription => "CCD";


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

public class CCEIceBomb : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 4 };
    public override string SkillDescription => "CCE";

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

public class CDETargetCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 3, 4 };
    public override string SkillDescription => "CDE";

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
    public override string SkillDescription => "DDA";

    public override void PreHit(Bullet bullet = null)
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
    public override string SkillDescription => "DDB";

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
    public override string SkillDescription => "DDC";

    public override void Hit(Enemy target, Bullet bullet = null)
    {
        bullet.CriticalPercentage += bullet.SlowRate;
    }

}

public class DDERemoteGuidence : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 4 };
    public override string SkillDescription => "DDE";

    public override void Shoot(Bullet bullet = null)
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
    public override string SkillDescription => "EEA";

    public override void Shoot(Bullet bullet = null)
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
    public override string SkillDescription => "EEB";


    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.BaseSputteringRangeIntensify -= 0.3f * adjacentTurretCount;//修复回初始值
        adjacentTurretCount = 0;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
                adjacentTurretCount++;
        }
        strategy.BaseSputteringRangeIntensify += 0.3f * adjacentTurretCount;
    }

}

public class EECConcretePouring : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 2 };
    public override string SkillDescription => "EEC";

    public override void Build()
    {
        strategy.BaseTargetCountIntensify += 2;
    }

    public override void Hit(Enemy target, Bullet bullet = null)
    {
        bullet.Damage *= 0.5f;
    }

}

public class EEDWantonBombing : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override string SkillDescription => "EED";

    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            bullet.SputteringPercentage += 1f;
        }
    }

}



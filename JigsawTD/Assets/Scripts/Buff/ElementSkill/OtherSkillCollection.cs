using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelDiscount : ElementSkill
{
    //升级防御塔费用-50%
    public override List<int> Elements => new List<int> { 0, 1, 2 };
    public override string SkillDescription => "LEVELDISCOUNT";

    public override void Build()
    {
        strategy.UpgradeDiscount += 0.5f;
    }
    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.75f;
    }
}

public class NextIntensify : ElementSkill
{
    //造成的伤害-25%，使下一个合成塔暴击+50%
    public override List<int> Elements => new List<int> { 0, 1, 3 };
    public override string SkillDescription => "NEXTINTENSIFY";

    public override void Composite()
    {
        StaticData.NextCompositeCallback = CompositeCallback;
    }

    public void CompositeCallback(StrategyBase strategy)
    {
        strategy.InitCriticalRateIntensify += 0.5f;
    }
    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.75f;
    }
}



public class FreeDraw : ElementSkill
{
    //造成的伤害-25%，接下来两次抽取模块免费
    public override List<int> Elements => new List<int> { 0, 1, 4 };
    public override string SkillDescription => "FreeDraw";

    public override void Composite()
    {
        GameManager.Instance.SetFreeShapeCount(2);
    }
}

public class TurretLevelUp : ElementSkill
{
    //提升1级防御塔等级
    public override List<int> Elements => new List<int> { 0, 2, 3 };
    public override string SkillDescription => "TURRETLEVELUP";

    public override void Build()
    {
        base.Build();
        strategy.Quality++;
        strategy.SetQualityValue();
    }
}

public class SystemDiscount : ElementSkill
{
    //造成的伤害-25%，使模块升级费用-50%
    public override List<int> Elements => new List<int> { 0, 2, 4 };
    public override string SkillDescription => "SYSTEMDISCOUNT";

    public override void Composite()
    {
        GameManager.Instance.SetModuleSystemDiscount(0.5f);
    }
    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.75f;
    }
}
public class RandomSkill : ElementSkill
{
    //变化为1个随机技能
    public override List<int> Elements => new List<int> { 0, 3, 4 };
    public override string SkillDescription => "RANDOMSKILL";

    public override void Build()
    {

    }
    public override void Composite()
    {
        List<int> newElements = new List<int> { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(newElements);
        strategy.TurretSkills.Remove(this);
        newSkill.Composite();//触发合成效果
        strategy.AddElementSkill(newSkill);
    }

}



public class FreeGround : ElementSkill
{
    //造成的伤害-25%，使接下来3次购买地板价格变为0
    public override List<int> Elements => new List<int> { 1, 2, 3 };
    public override string SkillDescription => "FREEGROUND";

    public override void Composite()
    {
        StaticData.FreeGroundTileCount += 3;
    }

    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.75f;
    }

}

public class PerfectElement : ElementSkill
{
    //造成的伤害-25%，合成时获得1个万能元素
    public override List<int> Elements => new List<int> { 1, 2, 4 };
    public override string SkillDescription => "PERFECTELEMENT";

    public override void Composite()
    {
        GameManager.Instance.GetPerfectElement(1);
    }

    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.75f;
    }
}



public class TrapIntensify : ElementSkill
{
    //相邻陷阱的效果提升100%
    public override List<int> Elements => new List<int> { 1, 3, 4 };
    public override string SkillDescription => "TRAPINTENSIFY";

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

public class PortalHit : ElementSkill
{
    //攻击有5%概率将敌人传送到3格前的位置
    public override List<int> Elements => new List<int> { 2, 3, 4 };
    public override string SkillDescription => "PORTALHIT";

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        if (target.DamageStrategy.IsEnemy) 
        {
            if (Random.value > 0.95f)
                ((Enemy)target).Flash(3);
        }
    }

}

public class LateDamage : ElementSkill
{
    //每回合开始后，每秒造成的伤害提高1%，上限100%
    public override List<int> Elements => new List<int> { 6, 6, 6 };
    public override string SkillDescription => "LATEDAMAGE";

    private float interval;
    private float damageIncreased = 0;

    public override void Tick(float delta)
    {
        base.Tick(delta);
        interval += delta;
        if (interval > 1f)
        {
            interval = 0;
            if (damageIncreased < 0.99f)
                damageIncreased += 0.01f;
        }
    }

    public override void Shoot(Bullet bullet = null)
    {
        bullet.Damage *= (1 + damageIncreased);
    }
    public override void EndTurn()
    {
        damageIncreased = 0;
        interval = 0;
    }

}

public class CircleRange : ElementSkill
{
    //使防御塔攻击范围变为圆型
    public override List<int> Elements => new List<int> { 6, 7, 8 };
    public override string SkillDescription => "CIRCLERANGE";

    public override void Build()
    {
        strategy.RangeType = RangeType.Circle;
        strategy.m_Turret.GenerateRange();
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelDiscount : ElementSkill
{
    //升级防御塔费用-50%
    public override List<int> Elements => new List<int> { 0, 1, 2 };

    public override void Build()
    {
        base.Build();
        strategy.UpgradeDiscount += 0.5f;
    }

}

public class CircleRange : ElementSkill
{
    //使防御塔攻击范围变为圆型
    public override List<int> Elements => new List<int> { 0, 1, 3 };

    public override void Build()
    {
        base.Build();
        if (strategy.RangeType != RangeType.Circle)//Rotary会在Initialize时将Checkangle改为360度，所以需要加入一个判断，避免再次修改攻击范围类型使其checkangle变回10度，回旋塔同理
            strategy.RangeType = RangeType.Circle;
    }

    public override void OnEquip()
    {
        base.OnEquip();
        strategy.Turret.GenerateRange();
    }

}

//public class NextIntensify : ElementSkill
//{
//    //造成的伤害-25%，使下一个合成塔暴击+40%
//    public override List<int> Elements => new List<int> { 0, 1, 3 };
//    public override string SkillDescription => "NEXTINTENSIFY";

//    public override void Composite()
//    {
//        GameRes.NextCompositeCallback = CompositeCallback;
//    }

//    public void CompositeCallback(StrategyBase strategy)
//    {
//        strategy.InitCriticalRateIntensify += 0.4f;
//    }

//}



public class ExtraTarget : ElementSkill
{
    //攻击-30%，目标数+1
    public override List<int> Elements => new List<int> { 0, 1, 4 };

    public override void Build()
    {
        base.Build();
        strategy.ComAttackIntensify -= 0.3f;
        strategy.BaseTargetCountIntensify += 1;
    }
}

public class HeavyCannon : ElementSkill
{
    //对生命值大于50%的单位造成额外50%伤害
    public override List<int> Elements => new List<int> { 0, 2, 3 };

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        if (target.DamageStrategy.CurrentHealth / target.DamageStrategy.MaxHealth > 0.5f)
        {
            float realDamage;
            target.DamageStrategy.ApplyDamage(bullet.Damage * 0.5f, out realDamage, false);
            strategy.TurnDamage += (int)realDamage;
            strategy.TotalDamage += (int)realDamage;
        }
    }
}

public class IceShell : ElementSkill
{
    //对减速敌人造成额外50%伤害
    public override List<int> Elements => new List<int> { 0, 2, 4 };

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        base.Hit(target, bullet);
        if (target.DamageStrategy.IsEnemy)
        {
            if (((Enemy)target).SlowRate > 0)
            {
                float realDamage;
                target.DamageStrategy.ApplyDamage(bullet.Damage * 0.5f, out realDamage, false);
                strategy.TurnDamage += (int)realDamage;
                strategy.TotalDamage += (int)realDamage;
            }
        }
    }
    //public override void Composite()
    //{
    //    if (GameRes.SystemLevel < StaticData.Instance.SystemMaxLevel)
    //    {
    //        GameRes.SystemUpgradeCost = Mathf.RoundToInt(GameRes.SystemUpgradeCost * 0.5f);
    //    }
    //}

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
        strategy.AddElementSkill(newSkill);
        newSkill.Composite();//触发合成效果

    }

}



public class FreeGround : ElementSkill
{
    //攻击距离+1，如果攻击距离大于6，则额外+1
    public override List<int> Elements => new List<int> { 1, 2, 3 };

    public override void Build()
    {
        base.Build();
        if (strategy.FinalRange > 6)
        {
            strategy.ComRangeIntensify += 2;
        }
        else
        {
            strategy.ComRangeIntensify += 1;
        }

    }

    public override void OnEquip()
    {
        base.OnEquip();
        strategy.Turret.GenerateRange();
    }



}

public class PerfectElement : ElementSkill
{
    //防御塔不会被冻结
    public override List<int> Elements => new List<int> { 1, 2, 4 };

    public override void Build()
    {
        base.Build();
        strategy.UnFrozable = true;
    }


}



public class TrapIntensify : ElementSkill
{
    //相邻陷阱的效果提升100%
    public override List<int> Elements => new List<int> { 1, 3, 4 };

    //private List<TrapContent> intensifiedTraps = new List<TrapContent>();
    //public override void Detect()
    //{
    //    foreach (var trap in intensifiedTraps)
    //    {
    //        trap.TrapIntensify -= 1f;
    //    }
    //    intensifiedTraps.Clear();
    //    List<Vector2Int> points = StaticData.GetCirclePoints(1);
    //    foreach (var point in points)
    //    {
    //        Vector2 pos = point + (Vector2)strategy.Turret.transform.position;
    //        Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TrapMask));
    //        if (hit != null)
    //        {
    //            TrapContent trap = hit.GetComponent<TrapContent>();
    //            trap.TrapIntensify += 1f;
    //            intensifiedTraps.Add(trap);
    //        }
    //    }
    //}
    public override void Composite()
    {
        GameRes.BuildCost= Mathf.RoundToInt(GameRes.BuildCost * 0.5f);
    }
}

public class PortalHit : ElementSkill
{
    //攻击有5%概率获得2金币
    public override List<int> Elements => new List<int> { 2, 3, 4 };
    public override string SkillDescription => "PORTALHIT";

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        if (Random.value > 0.92f)
            //((Enemy)target).Flash(2);
            StaticData.Instance.GainMoneyEffect(((Enemy)target).transform.position, 2);
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


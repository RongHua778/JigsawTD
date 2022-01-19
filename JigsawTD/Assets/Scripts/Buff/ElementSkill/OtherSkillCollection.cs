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



public class ExtraElement : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 4 };

    public override void StartTurn()
    {
        strategy.GainRandomTempElement(strategy.TurretSkills.Count-1);
    }
}

public class HeavyCannon : ElementSkill
{
    //额外一个技能槽
    public override List<int> Elements => new List<int> { 0, 2, 3 };

    public override void Build()
    {
        base.Build();
        strategy.ElementSKillSlot++;
    }
}

public class FreeGround : ElementSkill
{
    //攻击距离+1，如果攻击距离大于6，则额外+1
    public override List<int> Elements => new List<int> { 0, 2, 4 };

    public override void Build()
    {
        base.Build();
        strategy.BaseRangeIntensify += 1;

    }

    public override void OnEquip()
    {
        base.OnEquip();
        strategy.Turret.GenerateRange();
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
        strategy.AddElementSkill(newSkill);
        newSkill.Composite();//触发合成效果

    }

}





public class PerfectElement : ElementSkill
{
    //防御塔不会被冻结
    public override List<int> Elements => new List<int> { 1, 2, 3 };

    public override void Build()
    {
        base.Build();
        strategy.UnFrozable = true;
    }


}
public class IceShell : ElementSkill
{
    //施加的冻结层数提高100%
    public override List<int> Elements => new List<int> { 1, 2, 4 };

    //public override void Composite()
    //{
    //    base.Composite();
    //    GameRes.PerfectElementCount++;
    //}
    //public override void Build()
    //{
    //    base.Build();
    //    strategy.BaseSplashPercentageIntensify += 0.35f;
    //}

    public override void PreHit(Bullet bullet = null)
    {
        if(bullet.Target!=null&& bullet.Target.Enemy.DamageStrategy.IsEnemy)
        {
            if (((BasicEnemyStrategy)bullet.Target.Enemy.DamageStrategy).IsFrosted)
            {
                bullet.Damage *= 2f;
            }
        }
    }


}




public class PortalHit : ElementSkill
{
    //赏金大炮 该他每回合第一次暴击时，获得暴击伤害除以50的金币
    //暴击时造成的溅射伤害翻倍
    public override List<int> Elements => new List<int> { 1, 3, 4 };
    public override string SkillDescription => "PORTALHIT";

    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            bullet.SplashPercentage *= 2f;
        }
    }

    //public override void PreHit(Bullet bullet = null)
    //{
    //    base.PreHit(bullet);
    //}

    //private bool firstCrit;

    ////public override void Hit(IDamage target, Bullet bullet = null)
    ////{
    ////    //if (Random.value > 0.9f)
    ////    //    //((Enemy)target).Flash(2);
    ////    //    StaticData.Instance.GainMoneyEffect(((ReusableObject)target).transform.position, 2);

    ////}

    //public override void StartTurn()
    //{
    //    firstCrit = false;
    //}
    //public override void PreHit(Bullet bullet = null)
    //{
    //    if (!firstCrit && bullet.isCritical)
    //    {
    //        int gainCoin = Mathf.RoundToInt(bullet.Damage * bullet.CriticalPercentage / 50);
    //        GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("GETMONEY") + gainCoin);
    //        GameObject coinEffect = StaticData.Instance.GainMoneyEffect(bullet.transform.position, gainCoin);
    //        coinEffect.transform.localScale = Vector2.one * Mathf.Clamp(1 * (Mathf.Log10(gainCoin) + 1), 1f, 5f);
    //        firstCrit = true;
    //    }
    //}

}
public class TrapIntensify : ElementSkill
{
    //相邻陷阱的效果提升100%
    public override List<int> Elements => new List<int> { 2, 3, 4 };

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
    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            bullet.SlowRate *= 2;
            bullet.SplashRange *= 2;
        }
    }
}




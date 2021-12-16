using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelDiscount : ElementSkill
{
    //��������������-50%
    public override List<int> Elements => new List<int> { 0, 1, 2 };

    public override void Build()
    {
        base.Build();
        strategy.UpgradeDiscount += 0.5f;
    }

}

public class CircleRange : ElementSkill
{
    //ʹ������������Χ��ΪԲ��
    public override List<int> Elements => new List<int> { 0, 1, 3 };

    public override void Build()
    {
        base.Build();
        if (strategy.RangeType != RangeType.Circle)//Rotary����Initializeʱ��Checkangle��Ϊ360�ȣ�������Ҫ����һ���жϣ������ٴ��޸Ĺ�����Χ����ʹ��checkangle���10�ȣ�������ͬ��
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
//    //��ɵ��˺�-25%��ʹ��һ���ϳ�������+40%
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
        strategy.GainRandomTempElement(strategy.TurretSkills.Count - 1);
    }
}

public class HeavyCannon : ElementSkill
{
    //������ֵdi��25%�ĵ�λ��ɶ���50%�˺�
    public override List<int> Elements => new List<int> { 0, 2, 3 };

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        if (target.DamageStrategy.CurrentHealth / target.DamageStrategy.MaxHealth < 0.25f)
        {
            float realDamage;
            target.DamageStrategy.ApplyDamage(bullet.Damage * 0.5f, out realDamage, false);
            strategy.TurnDamage += (int)realDamage;
            strategy.TotalDamage += (int)realDamage;
        }
    }
}

public class FreeGround : ElementSkill
{
    //��������+1����������������6�������+1
    public override List<int> Elements => new List<int> { 0, 2, 4 };

    public override void Build()
    {
        base.Build();
        if (strategy.FinalRange >= 6)
        {
            strategy.BaseRangeIntensify += 2;
        }
        else
        {
            strategy.BaseRangeIntensify += 1;
        }

    }

    public override void OnEquip()
    {
        base.OnEquip();
        strategy.Turret.GenerateRange();
    }



}
public class RandomSkill : ElementSkill
{
    //�仯Ϊ1���������
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
        newSkill.Composite();//�����ϳ�Ч��

    }

}





public class PerfectElement : ElementSkill
{
    //���������ᱻ����
    public override List<int> Elements => new List<int> { 1, 2, 3 };

    public override void Build()
    {
        base.Build();
        strategy.UnFrozable = true;
    }


}
public class IceShell : ElementSkill
{
    //ʩ�ӵĶ���������100%
    public override List<int> Elements => new List<int> { 1, 2, 4 };

    public override void Build()
    {
        base.Build();
        Debug.Log("��δʵ�ֶ��Ṧ��");
    }
    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        base.Hit(target, bullet);
        //if (target.DamageStrategy.IsEnemy)
        //{
        //    if (((Enemy)target).SlowRate > 0)
        //    {
        //        float realDamage;
        //        target.DamageStrategy.ApplyDamage(bullet.Damage * 0.5f, out realDamage, false);
        //        strategy.TurnDamage += (int)realDamage;
        //        strategy.TotalDamage += (int)realDamage;
        //    }
        //}
    }

}




public class PortalHit : ElementSkill
{
    //������5%���ʻ��2���
    public override List<int> Elements => new List<int> { 1, 3, 4 };
    public override string SkillDescription => "PORTALHIT";

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        if (Random.value > 0.92f)
            //((Enemy)target).Flash(2);
            StaticData.Instance.GainMoneyEffect(((ReusableObject)target).transform.position, 2);
    }

}
public class TrapIntensify : ElementSkill
{
    //���������Ч������100%
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




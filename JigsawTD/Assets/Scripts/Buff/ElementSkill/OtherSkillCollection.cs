using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelDiscount : ElementSkill
{
    //��������������-50%
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
    //��ɵ��˺�-25%��ʹ��һ���ϳ�������+50%
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
    //��ɵ��˺�-25%�����������γ�ȡģ�����
    public override List<int> Elements => new List<int> { 0, 1, 4 };
    public override string SkillDescription => "FreeDraw";

    public override void Composite()
    {
        GameManager.Instance.SetFreeShapeCount(2);
    }
}

public class TurretLevelUp : ElementSkill
{
    //����1���������ȼ�
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
    //��ɵ��˺�-25%��ʹģ����������-50%
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
        newSkill.Composite();//�����ϳ�Ч��
        strategy.AddElementSkill(newSkill);
    }

}



public class FreeGround : ElementSkill
{
    //��ɵ��˺�-25%��ʹ������3�ι���ذ�۸��Ϊ0
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
    //��ɵ��˺�-25%���ϳ�ʱ���1������Ԫ��
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
    //���������Ч������100%
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
    //������5%���ʽ����˴��͵�3��ǰ��λ��
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
    //ÿ�غϿ�ʼ��ÿ����ɵ��˺����1%������100%
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
    //ʹ������������Χ��ΪԲ��
    public override List<int> Elements => new List<int> { 6, 7, 8 };
    public override string SkillDescription => "CIRCLERANGE";

    public override void Build()
    {
        strategy.RangeType = RangeType.Circle;
        strategy.m_Turret.GenerateRange();
    }

}
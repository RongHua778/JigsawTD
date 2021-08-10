using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HurtBullet : ElementSkill
{
    //���ӵ��˺��ĵ����ܵ��˺����25%������2��
    public override List<int> Elements => new List<int> { 0, 1, 2 };
    public override string SkillDescription => "HURTBULLET";

    public override void Hit(Enemy target, Bullet bullet = null)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.DamageIntensify, 0.25f, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class RandomIntensify : ElementSkill
{
    //ÿ�غϿ�ʼ����������Ч��֮һ����������80%����������80%������������50%��
    public override List<int> Elements => new List<int> { 0, 1, 3 };
    public override string SkillDescription => "RANDOMINTENSIFY";

    public override void StartTurn()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                strategy.TurnAttackIntensify += 0.8f;
                break;
            case 1:
                strategy.TurnSpeedIntensify += 0.8f;
                break;
            case 2:
                strategy.TurnFixCriticalRate += 0.5f;
                break;
        }
    }
}

public class LevelDiscount : ElementSkill
{
    //��������������-50%
    public override List<int> Elements => new List<int> { 0, 1, 4 };
    public override string SkillDescription => "LEVELDISCOUNT";

    public override void Build()
    {
        strategy.UpgradeDiscount += 0.5f;
    }
}

public class CopySkill : ElementSkill
{
    //������һ��Ԫ�ؼ���
    public override List<int> Elements => new List<int> { 0, 2, 3 };
    public override string SkillDescription => "COPYSKILL";

    public override void OnEquip()
    {
        ElementSkill skill = strategy.TurretSkills[SkillIndex == 1 ? 2 : 1] as ElementSkill;
        if (skill.Elements.SequenceEqual(Elements))
        {
            Debug.Log("�����ظ��ĸ��Ƽ���");
            return;
        }
        strategy.GetComIntensify(this, false);
        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(skill.Elements);
        strategy.TurretSkills.Remove(this);
        strategy.AddElementSkill(newSkill);
    }
}




public class ModuleLevelUp : ElementSkill
{
    //����1��ģ��ȼ�
    public override List<int> Elements => new List<int> { 0, 2, 4 };
    public override string SkillDescription => "MODULELEVELUP";

    public override void Composite()
    {
        GameManager.Instance.ModuleLevelUp();
    }

}

public class PoloIntensify : ElementSkill
{
    //�ܵ��⻷�ļӳ�Ч������100%
    public override List<int> Elements => new List<int> { 0, 3, 4 };
    public override string SkillDescription => "POLOINTENSIFY";

    public override void Build()
    {
        base.Build();
        strategy.PoloIntensifyModify += 1f;
    }
}

public class RandomTrap : ElementSkill
{
    //���һ���������
    public override List<int> Elements => new List<int> { 1, 2, 3 };
    public override string SkillDescription => "RANDOMTRAP";

    public override void Composite()
    {
        Debug.Log("���һ���������");
    }

}

public class CircleRange : ElementSkill
{
    //ʹ������������Χ��ΪԲ��
    public override List<int> Elements => new List<int> { 1, 2, 4 };
    public override string SkillDescription => "CIRCLERANGE";

    public override void Build()
    {
        strategy.RangeType = RangeType.Circle;
    }

    public override void OnEquip()
    {
        strategy.m_Turret.GenerateRange();
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

    public override void Hit(Enemy target, Bullet bullet = null)
    {
        if (Random.value > 0.95f)
            target.Flash(3);
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

    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        bullet.Damage *= (1 + damageIncreased);
    }
    public override void EndTurn()
    {
        damageIncreased = 0;
        interval = 0;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //ÿ�غϿ�ʼ����������Ч��֮һ���غϹ�������80%���غϹ�������80%���غϱ���������40%��
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
                strategy.TurnCriticalPercentageIntensify += 0.4f;
                break;
        }
    }
}

public class PoloIntensify : ElementSkill
{
    //�ܵ��⻷�ļӳ�Ч������100%
    public override List<int> Elements => new List<int> { 0, 1, 3 };
    public override string SkillDescription => "POLOINTENSIFY";

    public override void Build()
    {
        strategy.PoloIntensifyModify += 1f;
    }
}

public class LateDamage : ElementSkill
{
    //ÿ�غϿ�ʼ��ÿ����ɵ��˺����1%������100%
    public override List<int> Elements => new List<int> { 0, 2, 3 };
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

public class LateSplashDamage : ElementSkill
{
    //����30������˺����100%
    public override List<int> Elements => new List<int> { 0, 2, 4 };
    public override string SkillDescription => "LATESLASHDAMAGE";

    public override void StartTurn()
    {
        Duration = 30;
    }

    public override void TickEnd()
    {
        strategy.TurnSputteringPercentageIntensify += 1;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class ADE : ElementSkill
{
    //����30������˺����100%
    public override List<int> Elements => new List<int> { 0, 3, 4 };
    public override string SkillDescription => "LATESLASHDAMAGE";



}

public class BCD : ElementSkill
{
    //����30������˺����100%
    public override List<int> Elements => new List<int> { 1, 2, 3 };
    public override string SkillDescription => "LATESLASHDAMAGE";

}

public class BCE : ElementSkill
{
    //����30������˺����100%
    public override List<int> Elements => new List<int> { 1, 2, 4 };
    public override string SkillDescription => "LATESLASHDAMAGE";

}

public class BDE : ElementSkill
{
    //����30������˺����100%
    public override List<int> Elements => new List<int> { 1, 3, 4 };
    public override string SkillDescription => "LATESLASHDAMAGE";

}

public class CDE : ElementSkill
{
    //����30������˺����100%
    public override List<int> Elements => new List<int> { 2, 3, 4 };
    public override string SkillDescription => "LATESLASHDAMAGE";

}
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
    public override TurretEffectName EffectName => TurretEffectName.AAA_HeavyCannon;
    public override string SkillDescription => "�����ڿڣ�����ת�ٴ�����ͣ����л�������������Ч������";
    public override void Build()
    {
        strategy.BaseAttackIntensifyModify += 1;
        strategy.RotSpeed *= 0.05f;
    }
}

public class BBBOverloadCartridge : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 1 };
    public override TurretEffectName EffectName => TurretEffectName.BBB_OverloadCartridge;
    public override string SkillDescription => "���ص��У�ÿ�غϿ�ʼǰ8�룬��������150%";
    public override void StartTurn()
    {
        Duration += 8;
        strategy.TurnSpeedIntensify *= 2.5f;
    }

    public override void TickEnd()
    {
        strategy.TurnSpeedIntensify /= 2.5f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class CCCFrostCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "˪�����ģ��ϳɺ󣬻��1������Ԫ�ء�";

    public override void Composite()
    {
        GameManager.Instance.GetPerfectElement(1);
    }

}

public class DDDRestlessGunpowder : ElementSkill
{
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "�궯��ҩ�������˺�����-100%��300%���������";

    public override void Shoot()
    {
        bullet.CriticalPercentage += Random.Range(-1, 3);
    }
}

public class EEENuclearShell : ElementSkill
{
    public override List<int> Elements => new List<int> { 4, 4, 4 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "�����ڵ������䷶Χ���ӵ����о�������0.3/��";

    public override void Shoot()
    {
        bullet.SputteringRange += bullet.GetTargetDistance() * 0.3f;
    }
}

public class AABChargedBase : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 1 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "���ܻ�����ս����ʼǰ15�룬����������50%";

    public override void StartTurn()
    {
        Duration += 15;
        strategy.TurnAttackIntensify *= 1.5f;
    }

    public override void TickEnd()
    {
        strategy.TurnAttackIntensify /= 1.5f;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class AACColdBullet : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 2 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "��ȴ�ӵ�������ÿ��0.1���پ���ɶ���5%�˺�";

    public override void Hit(Enemy target)
    {
        float increaseDamage = target.SlowRate * 0.5f;
        bullet.Damage *= (1 + increaseDamage);
        Debug.Log("coldbulletdamage=" + bullet.Damage);
    }
}

public class AADMetalBullet : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 0, 3 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "�Ͻ��ӵ���ÿ�α��������������غϹ�����10%��������200%";

    private float attackIncreased;
    public override void Hit(Enemy target)
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
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "����״����ÿ��1����������������20%����������";

    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.BaseAttackIntensify -= 0.2f * adjacentTurretCount;//�޸��س�ʼֵ
        adjacentTurretCount = 0;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
                adjacentTurretCount++;
        }
        strategy.BaseAttackIntensify += 0.2f * adjacentTurretCount;
    }
}

public class ABCReinforceBase : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 2 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "�ӹ̵�������������������-50%";

    public override void Build()
    {
        strategy.UpgradeDiscount += 0.5f;
    }
}

public class ABDInvestment : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 3 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "����Ͷ�ʣ���õĽ�����10%";

    public override void Composite()
    {
        StaticData.OverallMoneyIntensify += 0.1f;
    }
}

public class ABEResourcesAllocation : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 1, 4 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "��Դ���ã�ÿ5�غϣ����1�ζ����ȡ������";

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
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "��Դ���գ��ϳ�ʱ�����2�ζ����ȡ������";

    public override void Composite()
    {
        GameManager.Instance.GainDraw(2);
    }
}

public class ACELonggerBarrel : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 2, 4 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "�ӳ��ڹܣ�����-50%����������+2��";

    public override void Build()
    {
        strategy.TurnSpeedIntensify *= 0.5f;
        strategy.BaseRangeIntensify += 2;
    }

    public override void EndTurn()//�غϽ���ʱ���ᱻ�ع�1
    {
        strategy.TurnSpeedIntensify *= 0.5f;
    }
}

public class ADEHardCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 0, 3, 4 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "ǿ�����ģ����ڷ������Ļ�������������30%";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseAttackIntensify -= 0.3f;
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
                strategy.BaseAttackIntensify += 0.3f;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class BBAProcessImprovement : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 0 };
    public override TurretEffectName EffectName => TurretEffectName.BBB_OverloadCartridge;
    public override string SkillDescription => "��������:��ÿ�غϿ�ʼ30��󣬹�������60%";

    public override void StartTurn()
    {
        Duration += 30;
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
    public override TurretEffectName EffectName => TurretEffectName.BBB_OverloadCartridge;
    public override string SkillDescription => "��׼���:�������Ŀ��4%��ǰ����ֵ�Ķ����˺�����BOSSΪ1%��";

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
    public override TurretEffectName EffectName => TurretEffectName.BBB_OverloadCartridge;
    public override string SkillDescription => "С��ä��:ÿ�ι���������4%�����ʣ�����Ϊ80%";

    float criticalRateIncreased;
    public override void Shoot()
    {
        if (criticalRateIncreased > 0.79f)
            return;
        strategy.TurnFixCriticalRate += 0.04f;
        criticalRateIncreased += 0.04f;
    }

    public override void EndTurn()
    {
        criticalRateIncreased = 0;
    }
}

public class BBETinyCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 1, 4 };
    public override TurretEffectName EffectName => TurretEffectName.BBB_OverloadCartridge;
    public override string SkillDescription => "С���ڿ�:�����������100%��������ɱ���";

    public override void Build()
    {
        strategy.BaseSpeedIntensify += 1f;
    }

    public override void Shoot()
    {
        bullet.CriticalRate = 0;
    }
}

public class BCDHardCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 2, 3 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "ά�޹�������������Ч������50%";

    private List<TrapContent> intensifiedTraps = new List<TrapContent>();
    public override void Detect()
    {
        foreach (var trap in intensifiedTraps)
        {
            trap.TrapIntensify -= 0.5f;
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
                trap.TrapIntensify += 0.5f;
                intensifiedTraps.Add(trap);
            }
        }
    }
}


public class BCEExtraCannon : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 2, 4 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "��װ�ڿڣ��˺�����50%�����⹥��1��Ŀ��";

    public override void Build()
    {
        strategy.BaseTargetCountIntensify += 1;
    }

    public override void StartTurn()
    {
        strategy.TurnAttackIntensify *= 0.5f;
    }

}

public class ADESpeedCore : ElementSkill
{
    public override List<int> Elements => new List<int> { 1, 3, 4 };
    public override TurretEffectName EffectName => TurretEffectName.CCC_FrostCore;
    public override string SkillDescription => "��׼���ģ����ڷ������Ļ�����������30%";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseSpeedIntensify -= 0.3f;
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
                strategy.BaseSpeedIntensify += 0.3f;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

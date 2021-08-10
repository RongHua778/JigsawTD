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
    public int SkillIndex;
    public abstract List<int> Elements { get; }

    public override void Build()
    {
        base.Build();
        strategy.GetComIntensify(this);
    }
}


//public class Skill001 : ElementSkill
//{
//    //����ת��-95%�������ӳɷ���
//    public override List<int> Elements => new List<int> { 0, 0, 0 };
//    public override string SkillDescription => "AAA";
//    public override void Build()
//    {
//        strategy.AllAttackIntensifyModify *=2;
//        strategy.RotSpeed = 0.5f;
//    }
//}

//public class BBBOverloadCartridge : ElementSkill
//{
//    //����ǰ10�룬�������200%
//    public override List<int> Elements => new List<int> { 1, 1, 1 };
//    public override string SkillDescription => "BBB";
//    public override void StartTurn()
//    {
//        Duration += 10;
//        strategy.TurnSpeedIntensify *= 3f;
//    }

//    public override void TickEnd()
//    {
//        strategy.TurnSpeedIntensify /= 3f;
//    }

//    public override void EndTurn()
//    {
//        Duration = 0;
//    }

//}





//public class EEENuclearShell : ElementSkill
//{
//    //�ӵ����䷶Χ����о������
//    public override List<int> Elements => new List<int> { 4, 4, 4 };
//    public override string SkillDescription => "EEE";

//    public override void Shoot(Bullet bullet = null, Enemy target = null)
//    {
//        bullet.SputteringRange += bullet.GetTargetDistance() * 0.3f;
//    }
//}



//public class AACLonggerCannon : ElementSkill
//{
//    //��������+2
//    public override List<int> Elements => new List<int> { 0, 0, 2 };
//    public override string SkillDescription => "AAC";

//    public override void Build()
//    {
//        strategy.BaseRangeIntensify += 2;
//    }

//}

//public class AADMetalBullet : ElementSkill
//{
//    //ÿ�α�����߱��غϹ�����10%
//    public override List<int> Elements => new List<int> { 0, 0, 3 };
//    public override string SkillDescription => "AAD";

//    private float attackIncreased;
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            if (attackIncreased > 1.95f)
//                return;
//            attackIncreased += 0.1f;
//            strategy.TurnAttackIntensify += 0.1f;
//        }
//    }

//    public override void EndTurn()
//    {
//        attackIncreased = 0;
//    }
//}



//public class ABCReinforceBase : ElementSkill
//{
//    //�����۸�-50%
//    public override List<int> Elements => new List<int> { 0, 1, 2 };
//    public override string SkillDescription => "ABC";

//    public override void Build()
//    {
//        strategy.UpgradeDiscount += 0.5f;
//    }
//}

//public class ABDInvestment : ElementSkill
//{
//    //��ý�Ǯ���15%
//    public override List<int> Elements => new List<int> { 0, 1, 3 };
//    public override string SkillDescription => "ABD";

//    public override void Composite()
//    {
//        StaticData.OverallMoneyIntensify += 0.15f;
//    }
//}

//public class ABEResourcesAllocation : ElementSkill
//{
//    //ÿ�γ�ȡģ��ʹ�������������10%
//    public override List<int> Elements => new List<int> { 0, 1, 4 };
//    public override string SkillDescription => "ABE";

//    public override void Draw()
//    {
//        strategy.BaseAttackIntensify += 0.1f;
//    }

//}

//public class ACDResourcesRecycle : ElementSkill
//{
//    //ʹ��ǰ��ȡģ��۸�-30%
//    public override List<int> Elements => new List<int> { 0, 2, 3 };
//    public override string SkillDescription => "ACD";

//    public override void Composite()
//    {
//        GameManager.Instance.SetBuyShapeCostDiscount(0.3f);
//    }
//}

//public class ACELonggerBarrel : ElementSkill
//{
//    //�´ι�����䷽��Ϊǿ���䷽
//    public override List<int> Elements => new List<int> { 0, 2, 4 };
//    public override string SkillDescription => "ACE";

//    public override void Composite()
//    {
//        StaticData.NextBuyIntensifyBlueprint++;
//    }
//}

//public class ADEHardCore : ElementSkill
//{
//    //���1������Ԫ��
//    public override List<int> Elements => new List<int> { 0, 3, 4 };
//    public override string SkillDescription => "ADE";
//    public override void Composite()
//    {
//        GameManager.Instance.GetPerfectElement(1);
//    }


//}



//public class BBCPreciseStrike : ElementSkill
//{
//    //�ӵ���Ŀ�����4%��ǰ����ֵ���˺�����BOSSΪ1%

//    public override List<int> Elements => new List<int> { 1, 1, 2 };
//    public override string SkillDescription => "BBC";

//    public override void Hit(Enemy target, Bullet bullet = null)
//    {
//        float realDamage;
//        float extraDamage = target.CurrentHealth * (target.IsBoss ? 0.01f : 0.04f);
//        target.ApplyDamage(extraDamage, out realDamage);
//        strategy.DamageAnalysis += (int)realDamage;
//    }

//}



//public class BBETinyCannon : ElementSkill
//{
//    //�ӵ��޷���ɱ��������й��ټӳɷ���
//    public override List<int> Elements => new List<int> { 1, 1, 4 };
//    public override string SkillDescription => "BBE";

//    public override void Build()
//    {
//        strategy.AllSpeedIntensifyModify += 1;
//    }

//    public override void Shoot(Bullet bullet = null, Enemy target = null)
//    {
//        bullet.CriticalRate = 0;
//    }
//}

//public class BCDMoneyFactory : ElementSkill
//{
//    //ÿ�α������2���
//    public override List<int> Elements => new List<int> { 1, 2, 3 };
//    public override string SkillDescription => "BCD";

//    public override void Hit(Enemy target, Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            GameManager.Instance.GainMoney(2);
//        }
//    }
//}


//public class BCERepairFactory : ElementSkill
//{
//    //���ڵ�����Ч�����100%
//    public override List<int> Elements => new List<int> { 1, 2, 4 };
//    public override string SkillDescription => "BCE";

//    private List<TrapContent> intensifiedTraps = new List<TrapContent>();
//    public override void Detect()
//    {
//        foreach (var trap in intensifiedTraps)
//        {
//            trap.TrapIntensify -= 1f;
//        }
//        intensifiedTraps.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TrapMask));
//            if (hit != null)
//            {
//                TrapContent trap = hit.GetComponent<TrapContent>();
//                trap.TrapIntensify += 1f;
//                intensifiedTraps.Add(trap);
//            }
//        }
//    }
//}

//public class ADESpeedCore : ElementSkill
//{
//    //������100-300���
//    public override List<int> Elements => new List<int> { 1, 3, 4 };
//    public override string SkillDescription => "BDE";

//    public override void Composite()
//    {
//        int money = Random.Range(100, 301);
//        GameManager.Instance.GainMoney(money);
//        GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("GETMONEY") + money);
//    }

//}




//public class CDETargetCore : ElementSkill
//{
//    //���ڷ�������������+1
//    public override List<int> Elements => new List<int> { 2, 3, 4 };
//    public override string SkillDescription => "CDE";

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.BaseRangeIntensify -= 1;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.BaseRangeIntensify += 1;
//                strategy.m_Turret.GenerateRange();
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}

//public class DDASealedCannon : ElementSkill
//{
//    //��������ʴ���100%������ɵ��˺����100%
//    public override List<int> Elements => new List<int> { 3, 3, 0 };
//    public override string SkillDescription => "DDA";

//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.CriticalRate > 1)
//        {
//            bullet.Damage *= 2f;
//        }
//    }
//}



//public class DDERemoteGuidence : ElementSkill
//{
//    //����������3���򱩻��˺��ṩ100%
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override string SkillDescription => "DDE";

//    public override void Shoot(Bullet bullet = null, Enemy target = null)
//    {
//        if (bullet.GetTargetDistance() > 3f)
//        {
//            bullet.CriticalPercentage += 1f;
//        }
//    }

//}


//public class EEBHeatingBarrel : ElementSkill
//{
//    //����ÿ���������������0.3����
//    public override List<int> Elements => new List<int> { 4, 4, 1 };
//    public override string SkillDescription => "EEB";


//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseSputteringRangeIntensify -= 0.3f * adjacentTurretCount;//�޸��س�ʼֵ
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseSputteringRangeIntensify += 0.3f * adjacentTurretCount;
//    }

//}

//public class EECConcretePouring : ElementSkill
//{
//    //��ɵ��˺�-50%�����⹥��2��Ŀ��
//    public override List<int> Elements => new List<int> { 4, 4, 2 };
//    public override string SkillDescription => "EEC";

//    public override void Build()
//    {
//        strategy.BaseTargetCountIntensify += 2;
//    }

//    public override void Hit(Enemy target, Bullet bullet = null)
//    {
//        bullet.Damage *= 0.5f;
//    }

//}

//public class EEDWantonBombing : ElementSkill
//{
//    //������ɵĽ����˺����100%
//    public override List<int> Elements => new List<int> { 4, 4, 3 };
//    public override string SkillDescription => "EED";

//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SputteringPercentage += 1f;
//        }
//    }

//}



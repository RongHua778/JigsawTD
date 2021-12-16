using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class RandomCritical : ElementSkill
{
    //����+25%�������˺�����-100%-400%֮����������
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    bool reRandom = false;
    int goldCount;
    int woodCount;
    int waterCount;
    int fireCount;
    int dustCount;

    public override void StartTurn()
    {
        int reRandomAmount = 0;
        if (strategy.TotalBaseCount != 0)
        {
            //��¼ȫԪ������
            goldCount = strategy.BaseGoldCount;
            woodCount = strategy.BaseWoodCount;
            waterCount = strategy.BaseWaterCount;
            fireCount = strategy.BaseFireCount;
            dustCount = strategy.BaseDustCount;
            reRandomAmount = strategy.TotalBaseCount;
            //���ȫԪ��
            strategy.BaseGoldCount = 0;
            strategy.BaseWoodCount = 0;
            strategy.BaseWaterCount = 0;
            strategy.BaseFireCount = 0;
            strategy.BaseDustCount = 0;
            reRandom = true;
        }
        reRandomAmount += 3;
        strategy.GainRandomTempElement(reRandomAmount);

    }

    public override void EndTurn()
    {
        if (reRandom)
        {
            strategy.BaseGoldCount = goldCount;
            strategy.BaseWoodCount = woodCount;
            strategy.BaseWaterCount = waterCount;
            strategy.BaseFireCount = fireCount;
            strategy.BaseDustCount = dustCount;
            reRandom = false;
        }
    }

}

public class CloseCritical : ElementSkill
{
    //��ս����50%
    public override List<int> Elements => new List<int> { 0, 0, 3 };
    public override float KeyValue => 0.5f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = KeyValue;
                strategy.TurnFixCriticalPercentage += intensifyValue;
                bullet.CriticalPercentage += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnFixCriticalPercentage -= intensifyValue;
                isIntensified = false;
            }
        }
    }

    public override void EndTurn()
    {
        isIntensified = false;
        intensifyValue = 0;
    }
}

public class HitCritical : ElementSkill
{
    //ÿ������50%
    public override List<int> Elements => new List<int> { 1, 1, 3 };
    public override float KeyValue => 0.02f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixCriticalPercentage += KeyValue;
    }
}

public class SlowCritical : ElementSkill
{
    //ÿ��+1.5%������
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override float KeyValue => 0.01f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixCriticalPercentage += KeyValue * delta;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class StartCritical : ElementSkill
{
    //����100%����
    public override List<int> Elements => new List<int> { 4, 4, 3 };
    public override float KeyValue => 0.5f * strategy.FireCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.FIRE].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.FIRE;
    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = KeyValue;
        strategy.TurnFixCriticalPercentage += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnFixCriticalPercentage -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
    }

}

//public class CopySkill : ElementSkill
//{
//    //װ�������Ƶ�һ��Ԫ�ؼ���

//    public override List<int> Elements => new List<int> { 3, 3, 3 };
//    public override string SkillDescription => "COPYSKILL";

//    public override void OnEquip()
//    {
//        ElementSkill skill = strategy.TurretSkills[1] as ElementSkill;
//        if (skill.Elements.SequenceEqual(Elements))
//        {
//            Debug.Log("�����ظ��ĸ��Ƽ���");
//            return;
//        }
//        //strategy.GetComIntensify(Elements, false);
//        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(skill.Elements);
//        strategy.TurretSkills.Remove(this);
//        newSkill.Composite();//�����ϳ�Ч��
//        strategy.AddElementSkill(newSkill);
//    }
//}
//public class AttackCritical : ElementSkill
//{
//    public override List<int> Elements => new List<int> { 3, 3, 0 };
//    private float attackIntensified;
//    private float timeCounter;
//    private bool isIntensify;

//    public override void StartTurn()
//    {
//        Duration += 999;
//    }
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            timeCounter = 2f;
//            if (!isIntensify)
//            {
//                isIntensify = true;
//                attackIntensified = 0.6f * strategy.TimeModify;
//                strategy.TurnAttackIntensify += 0.6f * strategy.TimeModify;
//            }
//        }
//    }
//    public override void Tick(float delta)
//    {
//        base.Tick(delta);
//        if (timeCounter >= 0)
//        {
//            timeCounter -= delta;
//            if (timeCounter <= 0)
//            {
//                isIntensify = false;
//                strategy.TurnAttackIntensify -= attackIntensified;
//            }
//        }

//    }

//    public override void EndTurn()
//    {
//        attackIntensified = 0;
//        isIntensify = false;
//    }

//}

//public class SpeedCritical : ElementSkill
//{
//    //������ʹ������1�빥������50%
//    public override List<int> Elements => new List<int> { 3, 3, 1 };

//    private float speedIncreased;
//    private float timeCounter;
//    private bool isIntensify;

//    public override void StartTurn()
//    {
//        Duration += 999;
//    }
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            timeCounter = 2f;
//            if (!isIntensify)
//            {
//                isIntensify = true;
//                speedIncreased = 0.5f * strategy.TimeModify;
//                strategy.TurnSpeedIntensify += 0.5f * strategy.TimeModify;
//            }
//        }
//    }
//    public override void Tick(float delta)
//    {
//        base.Tick(delta);
//        if (timeCounter >= 0)
//        {
//            timeCounter -= delta;
//            if (timeCounter <= 0)
//            {
//                isIntensify = false;
//                strategy.TurnSpeedIntensify -= speedIncreased;
//            }
//        }

//    }

//    public override void EndTurn()
//    {
//        speedIncreased = 0;
//        isIntensify = false;
//    }

//}

//public class CriticalSlow : ElementSkill
//{
//    //������ɵļ���Ч������
//    public override List<int> Elements => new List<int> { 3, 3, 2 };
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SlowRate *= 2f;
//        }
//    }
//}

//public class CriticalSplash : ElementSkill
//{
//    //������ɵļ���Ч������
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SputteringRange *= 2f;
//        }
//    }
//}








//public class CriticalAdjacent : ElementSkill
//{
//    //����ÿ���������������25%������
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override string SkillDescription => "CRITICALADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseCriticalPercentageIntensify -= 0.25f * adjacentTurretCount;//�޸��س�ʼֵ
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseCriticalPercentageIntensify += 0.25f * adjacentTurretCount;
//    }
//}

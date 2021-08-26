using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CopySkill : ElementSkill
{
    //װ�������Ƶ�һ��Ԫ�ؼ���
    
    public override List<int> Elements => new List<int> { 3, 3, 3 };
    public override string SkillDescription => "COPYSKILL";

    public override void OnEquip()
    {
        ElementSkill skill = strategy.TurretSkills[1] as ElementSkill;
        if (skill.Elements.SequenceEqual(Elements))
        {
            Debug.Log("�����ظ��ĸ��Ƽ���");
            return;
        }
        strategy.GetComIntensify(Elements, false);
        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(skill.Elements);
        strategy.TurretSkills.Remove(this);
        newSkill.Composite();//�����ϳ�Ч��
        strategy.AddElementSkill(newSkill);
        
    }
}
public class CriticalPolo : ElementSkill
{
    //���ڷ��������25%����
    public override List<int> Elements => new List<int> { 3, 3, 4 };
    public override string SkillDescription => "CRITICALPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitCriticalRateIntensify -= 0.25f * strategy.PoloIntensifyModify;
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
                strategy.InitCriticalRateIntensify += 0.25f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class RandomCritical : ElementSkill
{
    //�����˺�����-100%-400%���������
    public override List<int> Elements => new List<int> { 3, 3, 0 };
    public override string SkillDescription => "RANDOMCRITICAL";

    public override void Shoot(Bullet bullet = null)
    {
        bullet.CriticalPercentage += Random.Range(-1f, 4f);
    }
}

public class CriticalSpeed : ElementSkill
{
    //������Чÿ�غ�ÿ�ι�������2.5%������
    public override List<int> Elements => new List<int> { 3, 3, 1 };
    public override string SkillDescription => "CRITICALSPEED";

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixCriticalRate += 0.025f * strategy.TimeModify;
    }
}

public class LateCritical : ElementSkill
{
    //����30��󱩻������40%
    public override List<int> Elements => new List<int> { 3, 3, 2 };
    public override string SkillDescription => "LATECRITICAL";

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnFixCriticalRate += 0.01f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

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

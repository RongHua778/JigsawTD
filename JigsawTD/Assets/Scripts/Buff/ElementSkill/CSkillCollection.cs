using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateIntensify : ElementSkill
{
    //����20�������������������
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override string SkillDescription => "LATEINTENSIFY";
    public override void StartTurn()
    {
        Duration = 20;
    }

    public override void TickEnd()
    {
        strategy.TimeModify += 1;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class SlowPolo : ElementSkill
{
    //���ڷ������������0.5
    public override List<int> Elements => new List<int> { 2, 2, 4 };
    public override string SkillDescription => "SLOWPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitSlowRateIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.InitSlowRateIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class SlowAttack : ElementSkill
{
    //�Ծ���С��3�ĵ�����ɼ���Ч������
    public override List<int> Elements => new List<int> { 2, 2, 0 };
    public override string SkillDescription => "SLOWATTACK";
    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.GetTargetDistance() <= 3)
        {
            bullet.SlowRate *= 2;
        }
    }
}

public class SlowSpeed : ElementSkill
{
    //�������Ŀ��2%��ǰ����ֵ���˺�
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override string SkillDescription => "SLOWSPEED";
    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        float damageIncreased = target.DamageStrategy.CurrentHealth * 0.02f;
        bullet.Damage += damageIncreased;
    }
}

public class CriticalSlow : ElementSkill
{
    //������ɵļ���Ч������
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override string SkillDescription => "CRITICALSLOW";
    public override void PreHit(Bullet bullet = null)
    {
        if (bullet.isCritical)
        {
            bullet.SlowRate *= 2f;
        }
    }
}

//public class SlowAdjacent : ElementSkill
//{
//    //����ÿ���������������0.5����
//    public override List<int> Elements => new List<int> { 2, 2, 4 };
//    public override string SkillDescription => "SLOWADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseSlowRateIntensify -= 0.5f * adjacentTurretCount;//�޸��س�ʼֵ
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseSlowRateIntensify += 0.5f * adjacentTurretCount;
//    }
//}

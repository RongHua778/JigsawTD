using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiTarget : ElementSkill
{
    //��ɵ��˺�-35%�����⹥��2��Ŀ��
    public override List<int> Elements => new List<int> { 1, 1, 1 };

    //public override void Composite()
    //{
    //    base.Composite();
    //    GameRes.TempWoodIntensify += 0.15f;
    //}
    public override void Build()
    {
        base.Build();
        strategy.BaseTargetCountIntensify += 2;
    }

    public override void Shoot(Bullet bullet = null)
    {
        bullet.Damage *= 0.65f;
    }
}
public class AttackSpeed : ElementSkill
{
    //ÿ�غ�ÿ�ι�������5%������
    public override List<int> Elements => new List<int> { 1, 1, 0 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnAttackIntensify += 0.05f * strategy.TimeModify;
    }
}

public class SlowSpeed : ElementSkill
{
    //ÿ�ι�������0.08����
    public override List<int> Elements => new List<int> { 1, 1, 2 };
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSlowRate += 0.08f * strategy.TimeModify;
    }

}

public class CriticalSpeed : ElementSkill
{
    //ÿ�ι����������غ�3%������
    public override List<int> Elements => new List<int> { 1, 1, 3 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixCriticalRate += 0.03f * strategy.TimeModify;
    }
}

public class SplashSpeed : ElementSkill
{
    //ÿ�غ�ÿ�ι�������0.02����
    public override List<int> Elements => new List<int> { 1, 1, 4 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnFixSputteringRange += 0.02f * strategy.TimeModify;
    }


}










//public class SpeedAdjacent : ElementSkill
//{
//    //����ÿ���������������50%����
//    public override List<int> Elements => new List<int> { 1, 1, 4 };
//    public override string SkillDescription => "SPEEDADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseSpeedIntensify -= 0.5f * adjacentTurretCount;//�޸��س�ʼֵ
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseSpeedIntensify += 0.5f * adjacentTurretCount;
//    }
//}

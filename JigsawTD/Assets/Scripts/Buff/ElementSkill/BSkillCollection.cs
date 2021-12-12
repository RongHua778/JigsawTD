using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SameTarget : ElementSkill
{
    //��ɵ��˺�-35%�����⹥��2��Ŀ��
    public override List<int> Elements => new List<int> { 1, 1, 1 };

    //public override void Build()
    //{
    //    base.Build();
    //    strategy.BaseTargetCountIntensify += 2;
    //}

    //public override void Shoot(Bullet bullet = null)
    //{
    //    bullet.Damage *= 0.65f;
    //}

    private IDamageable lastTarget;
    private float speedIncreased;

    public override void Shoot(Bullet bullet = null)
    {
        IDamageable target = strategy.Turret.Target[0].Enemy;
        if (target != lastTarget)
        {
            lastTarget = target;
            strategy.TurnSpeedIntensify -= speedIncreased;
            speedIncreased = 0;
            return;
        }
        if (speedIncreased < 2.99f)
        {
            speedIncreased += 0.2f * strategy.TimeModify;
            strategy.TurnSpeedIntensify += 0.2f * strategy.TimeModify;
        }
    }

    public override void EndTurn()
    {
        lastTarget = null;
        speedIncreased = 0;
    }
}
public class CloseSpeed : ElementSkill
{
    //����С��3��75����
    public override List<int> Elements => new List<int> { 1, 1, 0 };

    float intensifyValue;
    bool isIntensified = false;
    public override void Shoot(Bullet bullet = null)
    {

        if (bullet.GetTargetDistance() < 3f)
        {
            if (!isIntensified)
            {
                intensifyValue = 0.75f * strategy.TimeModify;
                strategy.TurnSpeedIntensify += intensifyValue;
                isIntensified = true;
            }
        }
        else
        {
            if (isIntensified)
            {
                strategy.TurnSpeedIntensify -= intensifyValue;
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

public class TimeSpeed : ElementSkill
{
    //ÿ��+2%����
    public override List<int> Elements => new List<int> { 1, 1, 2 };

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnSpeedIntensify += 0.02f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }

}

public class FlutSpeed : ElementSkill
{
    //���ٲ���
    public override List<int> Elements => new List<int> { 1, 1, 3 };

    float targetValue;
    float currentValue;
    float counter = 2;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 999;
    }
    public override void Tick(float delta)
    {
        base.Tick(delta);
        counter += delta;
        if (counter > 2f)
        {
            counter = 0;
            targetValue = Random.Range(0.25f, 2.5f);
            DOTween.To(() => currentValue, x => currentValue = x, targetValue, 2);
        }
        strategy.SpeedAdjust = currentValue;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
        strategy.SpeedAdjust = 1;
        currentValue = 1;
        counter = 2;
    }
}

public class StartSpeed : ElementSkill
{
    //����+150%����
    public override List<int> Elements => new List<int> { 1, 1, 4 };

    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 1.5f * strategy.TimeModify;
        strategy.TurnSpeedIntensify += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnSpeedIntensify -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackSpeed : ElementSkill
{
    //ÿ�غ�ÿ�ι�������10%������������200%
    public override List<int> Elements => new List<int> { 0, 0, 1 };
    public override string SkillDescription => "ATTACKSPEED";

    //private float attackIncreased = 0;
    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {

        //attackIncreased += 0.05f * strategy.TimeModify;
        strategy.TurnAttackIntensify += 0.05f * strategy.TimeModify;
    }

    //public override void EndTurn()
    //{
    //    attackIncreased = 0;
    //}
}


public class LateAttack : ElementSkill
{
    //����30��󹥻����100%
    public override List<int> Elements => new List<int> { 0, 0, 2 };
    public override string SkillDescription => "LATEATTACK";

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnAttackIntensify += 0.02f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class RandomAttack : ElementSkill
{
    //�ӵ���������50%-200%֮���������
    public override List<int> Elements => new List<int> { 0, 0, 3 };
    public override string SkillDescription => "RANDOMATTACK";

    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        bullet.Damage *= Random.Range(0.5f, 2f);
    }
}

//public class AttackAdjacent : ElementSkill
//{
//    //����ÿ���������������50%������
//    public override List<int> Elements => new List<int> { 0, 0, 4 };
//    public override string SkillDescription => "ATTACKADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseAttackIntensify -= 0.5f * adjacentTurretCount;//�޸��س�ʼֵ
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseAttackIntensify += 0.5f * adjacentTurretCount;
//    }
//}
public class AttackPolo : ElementSkill
{
    //���ڷ��������������50%
    public override List<int> Elements => new List<int> { 0, 0, 4 };
    public override string SkillDescription => "ATTACKPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.InitAttackIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.InitAttackIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }

}

public class DoubleAttack : ElementSkill
{
    //���й�������Ч������
    public override List<int> Elements => new List<int> { 0, 0, 0 };
    public override string SkillDescription => "DOUBLEATTACK";
    public override void Build()
    {
        base.Build();
        strategy.AllAttackIntensifyModify += 1;
    }

}

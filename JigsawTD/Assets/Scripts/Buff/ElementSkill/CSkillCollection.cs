using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPolo : ElementSkill
{
    //���ڷ������������0.5
    public override List<int> Elements => new List<int> { 2, 2, 2 };
    public override string SkillDescription => "SLOWPOLO";

    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
    public override void Detect()
    {
        foreach (var strategy in intensifiedStrategies)
        {
            strategy.BaseSlowRateIntensify -= 0.5f * strategy.PoloIntensifyModify;
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
                strategy.BaseSlowRateIntensify += 0.5f * strategy.PoloIntensifyModify;
                intensifiedStrategies.Add(strategy);
            }
        }
    }
}

public class SlowAttack : ElementSkill
{
    //ÿ0.1�������10%�˺�
    public override List<int> Elements => new List<int> { 2, 2, 0 };
    public override string SkillDescription => "SLOWATTACK";
    public override void Hit(Enemy target, Bullet bullet = null)
    {
        bullet.Damage *= (1 + bullet.SlowRate);
    }
}

public class SlowSpeed : ElementSkill
{
    //ÿ�ι������������غ�0.1����,����Ϊ2
    public override List<int> Elements => new List<int> { 2, 2, 1 };
    public override string SkillDescription => "SLOWSPEED";
    private float slowIncreased = 0;
    public override void Shoot(Bullet bullet = null, Enemy target = null)
    {
        if (slowIncreased > 1.95f)
            return;
        slowIncreased += 0.1f;
        strategy.TurnFixSlowRate += 0.1f;
    }

    public override void EndTurn()
    {
        slowIncreased = 0;
    }
}

public class RandomSlow : ElementSkill
{
    //�ӵ�����0-1���������
    public override List<int> Elements => new List<int> { 2, 2, 3 };
    public override string SkillDescription => "RANDOMSLOW";
    public override void PreHit(Bullet bullet = null)
    {
        bullet.SlowRate += Random.Range(0, 1f);
    }
}

public class SlowAdjacent : ElementSkill
{
    //����ÿ���������������0.5����
    public override List<int> Elements => new List<int> { 0, 0, 4 };
    public override string SkillDescription => "SLOWADJACENT";

    private int adjacentTurretCount = 0;
    public override void Detect()
    {
        strategy.BaseSlowRateIntensify -= 0.5f * adjacentTurretCount;//�޸��س�ʼֵ
        adjacentTurretCount = 0;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
                adjacentTurretCount++;
        }
        strategy.BaseSlowRateIntensify += 0.5f * adjacentTurretCount;
    }
}

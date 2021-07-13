using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretSkillName
{
    J1SkillDistanceBaseDamage,
    S002SlowBullet,
    L1Skill,
    F1Skill,
    G1SkillMultiTarget,
    HISkillCountBaseSputteringPercentage,
    I1SkillSpeedIncreasedPerShoot,
    K1SkillDoubleCriticalPercentage,
    S009CurrentHealthBaseDamage,
    M1SkillDoubleSlowRate,
    S011IncreaseDamageBuff,
    S012SameTargetDamageIncrease,

    None
}


public abstract class TurretSkill
{
    public virtual TurretSkillName EffectName { get; }
    public virtual string SkillDescription { get; set; }
    public StrategyBase strategy;
    public Bullet bullet;
    public float KeyValue;
    public float Duration;
    public bool IsFinish = false;

    public virtual void Composite()
    {

    }

    public virtual void Detect()
    {

    }

    public virtual void Build()
    {

    }

    public virtual void Shoot()
    {

    }
    public virtual void PreHit()
    {

    }

    public virtual void Hit(Enemy target)
    {

    }

    public virtual void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            TickEnd();
            IsFinish = true;
        }
    }

    public virtual void TickEnd()
    {

    }

    public virtual void StartTurn()
    {

    }

    public virtual void EndTurn()
    {

    }
}

public abstract class InitialSkill : TurretSkill
{

}
public class I1SkillSpeedIncreasePerShoot : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.I1SkillSpeedIncreasedPerShoot;
    public override string SkillDescription => "��ת�ڻ�����Χ���������������ÿ�η�������0.1���٣�����Ϊ10";
    public override void Shoot()
    {
        if (strategy.TurnFixSpeed > 9.95f)
            return;
        strategy.TurnFixSpeed += 0.1f;
    }
}
public class J1SkillDistanceBaseDamage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.J1SkillDistanceBaseDamage;
    public override string SkillDescription => "�ӵ�����о�������30%/����˺����ѻ�������ÿ���ո�ʹ��Ч������10%";

    float increaseRate;

    public override void Detect()
    {
        increaseRate = 0f;
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.GroundTileMask));
            if (hit != null)
            {
                increaseRate += 0.1f;
            }
        }
    }

    public override void Shoot()
    {
        bullet.Damage *= (1 + (0.3f + increaseRate) * bullet.GetTargetDistance());
    }
}

public class G1SkillMultiTarget : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.G1SkillMultiTarget;
    public override string SkillDescription => "ɢ������ɵ��˺�����50%�����ǿ��Զ��⹥��3��Ŀ��";
    public override void Build()
    {
        strategy.BaseTargetCountIntensify += 3;
    }

    public override void StartTurn()
    {
        strategy.TurnAttackIntensify *= 0.5f;
    }

}
public class K1SkillDoubleCriticalPercentage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.K1SkillDoubleCriticalPercentage;
    public override string SkillDescription => "���ڵ����б����˺�����Ч������";

    public override void PreHit()
    {
        float increase = bullet.CriticalPercentage - 1.5f;
        bullet.CriticalPercentage += increase * 2f;
    }

}
public class H1SkillCountBaseSputteringPercentage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.HISkillCountBaseSputteringPercentage;
    public override string SkillDescription => "�Ȼ��ڵ��ڵ�ÿ���䵽1�����ˣ��ͻ�ʹ�����˺����30%";


    public override void PreHit()
    {
        bullet.SputteringPercentage += bullet.SputteredCount * 0.3f;
    }


}


public class SlowBullet : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.S002SlowBullet;

    private float duration = 2f;
    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, KeyValue, duration);
        target.Buffable.AddBuff(info);
    }

}

public class L1Skill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.L1Skill;
    public override string SkillDescription => "������ÿ�ι���������������10%�Ĺ�������������20��";

    private float intensifyIncreased = 0;
    public override void Shoot()
    {
        if (intensifyIncreased > 1.95f)
            return;
        intensifyIncreased += 0.1f;
        strategy.TurnAttackIntensify += 0.1f;
    }

    public override void EndTurn()
    {
        intensifyIncreased = 0;
    }
}

public class F1Skill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.F1Skill;
    public override string SkillDescription => "����ڵĹ�����Χ��ÿ��1�����ˣ���ɵ��˺������10%";

    public override void Shoot()
    {
        int count = strategy.m_Turret.targetList.Count;
        bullet.Damage *= (1 + count * 0.1f);
    }


}

public class CurrentHealthBaseDmage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.S009CurrentHealthBaseDamage;

    public override void Hit(Enemy target)
    {
        float realDamage;
        float extraDamage = target.CurrentHealth * (target.IsBoss ? 0.01f : 0.04f);
        target.ApplyDamage(extraDamage, out realDamage);
        strategy.DamageAnalysis += (int)realDamage;
        Debug.Log("DealHealthBaseDamage:" + extraDamage);
    }
}

public class M1SkillDoubleSlowRate : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.M1SkillDoubleSlowRate;

    public override string SkillDescription => "����һ���ƶ���ֱ��·��ĩ�˵�ѩ��ѩ��ʹ���˵ĵ�ǰ����Ч������";
    public override void Hit(Enemy target)
    {
        strategy.RotSpeed = 0;
        float increaseSlow = target.SlowRate * 2f;
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, increaseSlow, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class IncreaseDamageBuff : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.S011IncreaseDamageBuff;

    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.DamageIntensify, KeyValue, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class SameTargetDamageIncrease : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.S012SameTargetDamageIncrease;
    public float IncreaseDamage;
    public Enemy LastTarget;
    private float maxDamageIncrease = 500;

    public override void Hit(Enemy target)
    {

        if (target == LastTarget)
        {
            if (IncreaseDamage > maxDamageIncrease)
                return;
            IncreaseDamage += KeyValue;
            bullet.Damage += IncreaseDamage;
        }
        else
        {
            IncreaseDamage = 0;
            LastTarget = target;
        }
    }

    public override void EndTurn()
    {
        IncreaseDamage = 0;
    }
}

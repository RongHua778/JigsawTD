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
    //public Bullet bullet;
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

    public virtual void Shoot(Bullet bullet = null)
    {

    }
    public virtual void PreHit(Bullet bullet = null)
    {

    }

    public virtual void Hit(IDamageable target, Bullet bullet = null)
    {

    }

    public virtual void Draw()
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

    public virtual void OnEquip()
    {

    }
}

public abstract class InitialSkill : TurretSkill
{

}
public class I1SkillSpeedIncreasePerShoot : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.I1SkillSpeedIncreasedPerShoot;
    public override string SkillDescription => "I1SKILL";
    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        if (target.DamageStrategy.IsEnemy)
        {
            strategy.RotSpeed = 0;
            float increaseSlow = ((Enemy)target).SlowRate * 2f;
            BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, increaseSlow, 2f);
            ((Enemy)target).Buffable.AddBuff(info);
        }
    }
}
public class J1SkillDistanceBaseDamage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.J1SkillDistanceBaseDamage;
    public override string SkillDescription => "J1SKILL";

    private float attackIncreased;

    public override void Build()
    {
        base.Build();
        strategy.ForbidRange += 2;
    }


    public override void Detect()
    {
        strategy.InitAttackIntensify -= attackIncreased;
        attackIncreased = strategy.FinalRange * 0.1f;
        strategy.InitAttackIntensify += attackIncreased;
    }
}

public class G1SkillMultiTarget : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.G1SkillMultiTarget;
    public override string SkillDescription => "G1SKILL";
    public override void Build()
    {
        strategy.BaseTargetCountIntensify += 3;
    }

    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.5f;
    }

}
public class K1SkillDoubleCriticalPercentage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.K1SkillDoubleCriticalPercentage;
    public override string SkillDescription => "K1SKILL";

    public override void PreHit(Bullet bullet = null)
    {
        float increase = bullet.CriticalPercentage - 1.5f;
        bullet.CriticalPercentage += increase * 2f;
    }

}
public class H1SkillCountBaseSputteringPercentage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.HISkillCountBaseSputteringPercentage;
    public override string SkillDescription => "H1SKILL";


    public override void PreHit(Bullet bullet = null)
    {
        bullet.SputteringPercentage += bullet.SputteredCount * 0.25f;
    }


}


public class SlowBullet : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.S002SlowBullet;

    private float duration = 2f;
    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        target.DamageStrategy.ApplyBuff(EnemyBuffName.SlowDown, KeyValue, duration);
    }

}

public class L1Skill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.L1Skill;
    public override string SkillDescription => "L1SKILL";

    //private float intensifyIncreased = 0;
    public override void Shoot(Bullet bullet = null)
    {
        //if (intensifyIncreased > 1.95f)
        //    return;
        //intensifyIncreased += 0.1f * strategy.TimeModify;
        strategy.TurnFixAttack += 5 * GameEndUI.TotalComposite * strategy.TimeModify;
        //strategy.TurnAttackIntensify += 0.1f * strategy.TimeModify;
    }

    //public override void EndTurn()
    //{
    //    intensifyIncreased = 0;
    //}
}

public class F1Skill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.F1Skill;
    public override string SkillDescription => "F1SKILL";

    public override void PreHit(Bullet bullet = null)
    {
        int count = strategy.m_Turret.targetList.Count;
        bullet.Damage *= (1 + count * 0.1f);
    }
}

public class CurrentHealthBaseDmage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.S009CurrentHealthBaseDamage;

    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        float realDamage;
        float extraDamage = target.DamageStrategy.CurrentHealth * 0.04f;
        target.DamageStrategy.ApplyDamage(extraDamage, out realDamage, false);
        strategy.DamageAnalysis += (int)realDamage;
    }
}

public class M1SkillDoubleSlowRate : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.M1SkillDoubleSlowRate;

    public override string SkillDescription => "M1SKILL";

    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 1.1f;
    }
}


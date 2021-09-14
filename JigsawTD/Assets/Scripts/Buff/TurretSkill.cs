using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretSkillName
{
    SniperSkill,
    RapiderSkill,
    ConstructorSkill,
    ScatterSkill,
    MortarSkill,
    RotarySkill,
    UltraSkill,
    SnowSkill,
    CooporativeSkill,
    BoomerrangSkill,
    SuperSkill,
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
public class RotarySkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.RotarySkill;
    public override string SkillDescription => "ROTARYSKILL";

    public override void Build()
    {
        base.Build();
        strategy.AllSpeedIntensifyModify += 1;
    }

}
public class SniperSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.SniperSkill;
    public override string SkillDescription => "SNIPERSKILL";

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

public class ScatterSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.ScatterSkill;
    public override string SkillDescription => "SCATTERSKILL";
    public override void Build()
    {
        strategy.BaseTargetCountIntensify += 3;
    }

    public override void PreHit(Bullet bullet = null)
    {
        bullet.Damage *= 0.5f;
    }

}
public class UltraSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.UltraSkill;
    public override string SkillDescription => "ULTRASKILL";

    public override void PreHit(Bullet bullet = null)
    {
        float increase = bullet.CriticalPercentage - 1.5f;
        bullet.CriticalPercentage += increase * 2f;
    }

}
public class MortarSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.MortarSkill;
    public override string SkillDescription => "MORTARSKILL";


    public override void Build()
    {
        base.Build();
        strategy.InitSputteringPercentageIntensify += 1.5f;
    }

}



public class RapiderSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.RapiderSkill;
    public override string SkillDescription => "RAPIDERSKILL";

    //public override void Shoot(Bullet bullet = null)
    //{
    //    strategy.TurnFixAttack += GameEndUI.TotalComposite * strategy.TimeModify;
    //}

    private Enemy lastTarget;
    private float speedIncreased;

    public override void Shoot(Bullet bullet = null)
    {
        Enemy target = (Enemy)strategy.m_Turret.Target[0].Enemy;
        if (target != lastTarget)
        {
            lastTarget = target;
            strategy.TurnSpeedIntensify -= speedIncreased;
            speedIncreased = 0;
            return;
        }
        if (speedIncreased < 3.99f)
        {
            speedIncreased += 0.1f * strategy.TimeModify;
            strategy.TurnSpeedIntensify += 0.1f * strategy.TimeModify;
        }
    }

    public override void EndTurn()
    {
        lastTarget = null;
        speedIncreased = 0;
    }

}

public class ConstructorSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.ConstructorSkill;
    public override string SkillDescription => "CONSTRUCTORSKILL";

    public override void PreHit(Bullet bullet = null)
    {
        int count = strategy.m_Turret.targetList.Count;
        bullet.Damage *= (1 + count * 0.1f);
    }
}



public class SnowSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.SnowSkill;

    public override string SkillDescription => "SNOWSKILL";

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

public class CooporativeSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.CooporativeSkill;
    public override string SkillDescription => "COOPORATIVESKILL";

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnAttackIntensify += 0.01f * GameEndUI.TotalComposite * strategy.TimeModify;
    }
}

public class BoomerrangSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.BoomerrangSkill;
    public override string SkillDescription => "BOOMERRANGSKILL";

    public override void Build()
    {
        base.Build();
        strategy.AllSpeedIntensifyModify = 0;

    }
    public override void Hit(IDamageable target, Bullet bullet = null)
    {
        base.Hit(target, bullet);
        bullet.Damage *= 1.1f;
    }
}

public class SuperSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.SuperSkill;
    public override string SkillDescription => "SUPERSKILL";

    public override void StartTurn()
    {
        Duration = 30;
    }

    public override void TickEnd()
    {
        strategy.TimeModify += 2;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}


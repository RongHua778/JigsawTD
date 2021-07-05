using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretEffectName
{
    S001DistanceBaseDamage,
    S002SlowBullet,
    S003AttackIncreasePerShoot,
    S004EnemyCountAttackIncrease,
    S005MultiTarget,
    S006SputteringRateIncrease,
    S007SpeedIncreasePerShoot,
    S008ChangeCriticalPercentage,
    S009CurrentHealthBaseDamage,
    S010IncreaseSlowRate,
    S011IncreaseDamageBuff,
    S012SameTargetDamageIncrease,

    AAA_HeavyCannon,
    BBB_OverloadCartridge,
    CCC_FrostCore
}
[System.Serializable]
public class TurretSkillInfo
{
    public TurretEffectName EffectName;
    public float KeyValue;
    [TextArea(2, 3)]
    public string EffectDescription;
}

public abstract class TurretSkill
{
    public abstract TurretEffectName EffectName { get; }
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
public class SpeedIncreasePerShoot : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S007SpeedIncreasePerShoot;
    public override void Shoot()
    {
        if (strategy.TurnSpeedIntensify > KeyValue * 4.95f)
            return;
        strategy.TurnSpeedIntensify += KeyValue;
    }
}
public class DistanceBaseDamage : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S001DistanceBaseDamage;
    public override void Shoot()
    {
        bullet.Damage *= (1 + KeyValue * bullet.GetTargetDistance());
    }
}

public class MultiTarget : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S005MultiTarget;

    public override void Build()
    {
        strategy.BaseTargetCountIntensify += (int)KeyValue;
    }

    public override void StartTurn()
    {
        strategy.TurnAttackIntensify *= 0.5f;
    }

}
public class ChangeCriticalPercentage : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S008ChangeCriticalPercentage;

    public override void Build()
    {
        strategy.BaseCriticalPercentageIntensify += KeyValue;
    }

}
public class SputteringRateIncrease : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S006SputteringRateIncrease;

    public override void Build()
    {
        strategy.BaseSputteringPercentageIntensify += KeyValue;
    }

}


public class SlowBullet : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S002SlowBullet;

    private float duration = 2f;
    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, KeyValue, duration);
        target.Buffable.AddBuff(info);
    }

}

public class AttackIncreasePerShoot : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S003AttackIncreasePerShoot;

    private float intensifyIncreased = 0;
    public override void Shoot()
    {
        if (intensifyIncreased > KeyValue * 19.5f)
            return;
        intensifyIncreased += KeyValue;
        strategy.TurnAttackIntensify += KeyValue;
    }

    public override void EndTurn()
    {
        intensifyIncreased = 0;
    }
}

public class EnemyCountAttackIncrease : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S004EnemyCountAttackIncrease;



}

public class CurrentHealthBaseDmage : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S009CurrentHealthBaseDamage;

    public override void Hit(Enemy target)
    {
        float realDamage;
        float extraDamage = target.CurrentHealth * (target.IsBoss ? 0.01f : 0.04f);
        target.ApplyDamage(extraDamage, out realDamage);
        strategy.DamageAnalysis += (int)realDamage;
        Debug.Log("DealHealthBaseDamage:" + extraDamage);
    }
}

public class IncreaseSlowRate : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S010IncreaseSlowRate;

    public override void Hit(Enemy target)
    {
        float increaseSlow = target.SlowRate * KeyValue;
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, increaseSlow, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class IncreaseDamageBuff : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S011IncreaseDamageBuff;

    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.DamageIntensify, KeyValue, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class SameTargetDamageIncrease : InitialSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S012SameTargetDamageIncrease;
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

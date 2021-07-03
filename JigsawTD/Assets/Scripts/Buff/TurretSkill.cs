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
    S012SameTargetDamageIncrease
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
    public StrategyBase strategy;
    public Bullet bullet;
    public float KeyValue;

    public virtual void Build()
    {
        
    }

    public virtual void Shoot()
    {

    }

    public virtual void Hit(Enemy target)
    {

    }
}
public class SpeedIncreasePerShoot : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S007SpeedIncreasePerShoot;
    public override void Shoot()
    {
        if (strategy.TurnSpeedIntensify > KeyValue * 99.5f)
            return;
        strategy.TurnSpeedIntensify += KeyValue;
    }
}
public class DistanceBaseDamage : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S001DistanceBaseDamage;
    public override void Shoot()
    {
        bullet.Damage *= (1 + KeyValue * bullet.GetTargetDistance());
    }
}

public class MultiTarget : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S005MultiTarget;

    public override void Build()
    {
        strategy.BaseTargetCountIntensify += (int)KeyValue;
        Debug.LogWarning("未完成全局伤害减少内容");
        strategy.BaseAttackIntensify -= 0.5f;
    }

}
public class ChangeCriticalPercentage : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S008ChangeCriticalPercentage;

    public override void Build()
    {
        strategy.BaseCriticalPercentageIntensify += KeyValue;
    }

}
public class SputteringRateIncrease : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S006SputteringRateIncrease;

    public override void Build()
    {
        strategy.BaseSputteringPercentageIntensify += KeyValue;
    }

}


public class SlowBullet : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S002SlowBullet;

    public float Duration = 2f;
    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, KeyValue, Duration);
        target.Buffable.AddBuff(info);
    }

}

public class AttackIncreasePerShoot : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S003AttackIncreasePerShoot;

    public override void Shoot()
    {
        if (strategy.TurnAttackIntensify > KeyValue * 19.5f)
            return;
        strategy.TurnAttackIntensify += KeyValue;
    }
}

public class EnemyCountAttackIncrease : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S004EnemyCountAttackIncrease;



}

public class CurrentHealthBaseDmage : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S009CurrentHealthBaseDamage;

    public override void Hit(Enemy target)
    {
        float realDamage;
        float extraDamage = target.CurrentHealth * KeyValue;
        target.ApplyDamage(extraDamage,out realDamage);
        strategy.DamageAnalysis += (int)realDamage;
        Debug.Log("DealHealthBaseDamage:" + extraDamage);
    }
}

public class IncreaseSlowRate : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S010IncreaseSlowRate;

    public override void Hit(Enemy target)
    {
        float increaseSlow = target.SlowRate * KeyValue;
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, increaseSlow, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class IncreaseDamageBuff : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S011IncreaseDamageBuff;

    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.DamageIntensify, KeyValue, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class SameTargetDamageIncrease : TurretSkill
{
    public override TurretEffectName EffectName => TurretEffectName.S012SameTargetDamageIncrease;
    public float IncreaseDamage;
    public Enemy LastTarget;

    public override void Hit(Enemy target)
    {
        if (target == LastTarget)
        {
            IncreaseDamage += KeyValue;
            bullet.Damage += IncreaseDamage;
        }
        else
        {
            IncreaseDamage = 0;
            LastTarget = target;
        }
    }
}

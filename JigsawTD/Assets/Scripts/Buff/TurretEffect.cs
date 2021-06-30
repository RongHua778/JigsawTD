using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretEffectName
{
    DistanceBaseDamage,
    SlowBullet,
    AttackIncreasePerShoot,
    EnemyCountAttackIncrease,
    MultiTarget,
    SputteringRateIncrease,
    SpeedIncreasePerShoot,
    ChangeCriticalPercentage,
    CurrentHealthBaseDamage,
    IncreaseSlowRate,
    IncreaseDamageBuff,
    SameTargetDamageIncrease
}
[System.Serializable]
public class TurretEffectInfo
{
    public TurretEffectName EffectName;
    public float KeyValue;
    [TextArea(2, 3)]
    public string EffectDescription;
}
public abstract class TurretEffect
{
    public abstract TurretEffectName EffectName { get; }
    public BasicStrategy strategy;
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
public class SpeedIncreasePerShoot : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SpeedIncreasePerShoot;
    public override void Shoot()
    {
        if (strategy.TurnSpeedIntensify > KeyValue * 9.5f)
            return;
        strategy.TurnSpeedIntensify += KeyValue;
    }
}
public class DistanceBaseDamage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.DistanceBaseDamage;
    public override void Shoot()
    {
        bullet.Damage *= (1 + KeyValue * bullet.GetTargetDistance());
    }
}

public class MultiTarget : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.MultiTarget;

    public override void Build()
    {
        strategy.TargetCount += (int)KeyValue;
        strategy.BaseAttackIntensify -= 0.5f;
    }

}
public class ChangeCriticalPercentage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.ChangeCriticalPercentage;

    public override void Build()
    {
        strategy.CriticalPercentage += KeyValue;
    }

}
public class SputteringRateIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SputteringRateIncrease;

    public override void Build()
    {
        strategy.SputteringRate += KeyValue;
    }
    //public override void Shoot()
    //{
    //    bullet.SputteringRange += KeyValue * bullet.GetTargetDistance();
    //}
}


public class SlowBullet : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SlowBullet;

    public float Duration = 2f;
    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, KeyValue, Duration);
        target.Buffable.AddBuff(info);
    }

}

public class AttackIncreasePerShoot : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.AttackIncreasePerShoot;

    public override void Hit(Enemy target)
    {
        if (strategy.TurnAttackIntensify > KeyValue * 19.5f)
            return;
        strategy.TurnAttackIntensify += KeyValue;
    }
}

public class EnemyCountAttackIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.EnemyCountAttackIncrease;



}

public class CurrentHealthBaseDmage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.CurrentHealthBaseDamage;

    public override void Hit(Enemy target)
    {
        float realDamage;
        float extraDamage = target.CurrentHealth * KeyValue;
        target.ApplyDamage(extraDamage,out realDamage);
        strategy.DamageAnalysis += (int)realDamage;
        Debug.Log("DealHealthBaseDamage:" + extraDamage);
    }
}

public class IncreaseSlowRate : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.IncreaseSlowRate;

    public override void Hit(Enemy target)
    {
        float increaseSlow = target.SlowRate * KeyValue;
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, increaseSlow, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class IncreaseDamageBuff : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.IncreaseDamageBuff;

    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.DamageIntensify, KeyValue, 2f);
        target.Buffable.AddBuff(info);
    }
}

public class SameTargetDamageIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SameTargetDamageIncrease;
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

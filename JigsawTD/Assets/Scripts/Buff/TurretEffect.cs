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
    RangeBaseSputtering
}
[System.Serializable]
public class TurretEffectInfo
{
    public TurretEffectName EffectName;
    public float KeyValue;
}
public abstract class TurretEffect
{
    public abstract TurretEffectName EffectName { get; }
    public Turret turret;
    public Bullet bullet;
    public float KeyValue;

    public virtual void Build()
    {

    }

    public virtual void EnemyEnter()
    {

    }
    public virtual void EnemyExit()
    {

    }
    public virtual void Shoot()
    {

    }

    public virtual void Hit(Enemy target)
    {

    }
}

public class DistanceBaseDamage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.DistanceBaseDamage;

    public override void Shoot()
    {
        float distance = ((Vector2)bullet.transform.position - bullet.Target.Position).magnitude;
        bullet.Damage *= (1 + distance * KeyValue);
    }
}

public class MultiTarget : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.MultiTarget;

    public override void Build()
    {
        turret.TargetCount += (int)KeyValue;
    }

}

public class RangeBaseSputtering : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.RangeBaseSputtering;

    public override void Shoot()
    {
        bullet.SputteringRange += KeyValue * bullet.GetTargetDistance();
    }
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
        bullet.turretParent.TurnAdditionalAttack += (int)KeyValue;
    }
}

public class EnemyCountAttackIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.EnemyCountAttackIncrease;

    public override void EnemyEnter()
    {
        turret.AttackIntensify += KeyValue;
    }

    public override void EnemyExit()
    {
        turret.AttackIntensify -= KeyValue;
    }

}

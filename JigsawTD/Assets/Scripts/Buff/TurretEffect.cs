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
    RangeBaseSputtering,
    SpeedIncreasePerShoot,
    ChangeCriticalPercentage
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
    public abstract string EffectDescription { get; }
    public TurretContent turret;
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
    public override string EffectDescription => "极速过载：每次攻击后，提升本回合攻速" + KeyValue + "。";
    public override void Shoot()
    {
        if (turret.TurnAdditionalSpeed > KeyValue * 100)
            return;
        turret.TurnAdditionalSpeed += KeyValue;
    }
}
public class DistanceBaseDamage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.DistanceBaseDamage;
    public override string EffectDescription => "致命瞄准：基于子弹飞行距离提升" + KeyValue * 100 + "%伤害/米。";
    public override void Shoot()
    {
        bullet.Damage *= (1 + KeyValue * bullet.GetTargetDistance());
    }
}

public class MultiTarget : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.MultiTarget;
    public override string EffectDescription => "多重射击：可以同时攻击" + KeyValue + "个额外目标。";

    public override void Build()
    {
        turret.TargetCount += (int)KeyValue;
    }

}
public class ChangeCriticalPercentage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.ChangeCriticalPercentage;
    public override string EffectDescription => "多重射击：可以同时攻击" + KeyValue + "个额外目标。";

    public override void Build()
    {
        turret.CriticalPercentage += KeyValue;
    }

}
public class RangeBaseSputtering : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.RangeBaseSputtering;
    public override string EffectDescription => "定点轰炸：基于子弹飞行距离提升" + KeyValue + "溅射范围/米。";

    public override void Shoot()
    {
        bullet.SputteringRange += KeyValue * bullet.GetTargetDistance();
    }
}


public class SlowBullet : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SlowBullet;
    public override string EffectDescription => "减速攻击：攻击造成" + KeyValue + "%减速效果。";

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
    public override string EffectDescription => "愈战愈勇：每次攻击后，提升本回合" + KeyValue + "%攻击力。";

    public override void Hit(Enemy target)
    {
        if (bullet.turretParent.TurnAttackIntensify > KeyValue * 9.5f)
            return;
        bullet.turretParent.TurnAttackIntensify += KeyValue;
    }
}

public class EnemyCountAttackIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.EnemyCountAttackIncrease;
    public override string EffectDescription => "攻击范围内每有1个敌人，就提升" + KeyValue * 100 + "%攻击力。";



}

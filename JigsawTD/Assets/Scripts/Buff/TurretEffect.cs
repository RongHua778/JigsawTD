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
    CurrentHealthBaseDamage
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
    public virtual string EffectDescription { get; }
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
    public override string EffectDescription => "���ٹ��أ�ÿ�ι������������غϹ���" + KeyValue + "��";
    public override void Shoot()
    {
        if (turret.TurnSpeedIntensify > KeyValue * 9.5f)
            return;
        turret.TurnSpeedIntensify += KeyValue;
    }
}
public class DistanceBaseDamage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.DistanceBaseDamage;
    public override string EffectDescription => "������׼�������ӵ����о�������" + KeyValue * 100 + "%�˺�/�ס�";
    public override void Shoot()
    {
        bullet.Damage *= (1 + KeyValue * bullet.GetTargetDistance());
    }
}

public class MultiTarget : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.MultiTarget;
    public override string EffectDescription => "�������������ͬʱ����" + KeyValue + "������Ŀ�ꡣ";

    public override void Build()
    {
        turret.TargetCount += (int)KeyValue;
        turret.BaseAttackIntensify -= 0.5f;
    }

}
public class ChangeCriticalPercentage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.ChangeCriticalPercentage;
    public override string EffectDescription => "�������������ͬʱ����" + KeyValue + "������Ŀ�ꡣ";

    public override void Build()
    {
        turret.CriticalPercentage += KeyValue;
    }

}
public class SputteringRateIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SputteringRateIncrease;
    public override string EffectDescription => "�����ը�������˺����" + KeyValue + "%";

    public override void Build()
    {
        turret.SputteringRate += KeyValue;
    }
    //public override void Shoot()
    //{
    //    bullet.SputteringRange += KeyValue * bullet.GetTargetDistance();
    //}
}


public class SlowBullet : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.SlowBullet;
    public override string EffectDescription => "���ٹ������������" + KeyValue + "%����Ч����";

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
    public override string EffectDescription => "��ս���£�ÿ�ι������������غ�" + KeyValue + "%��������";

    public override void Hit(Enemy target)
    {
        if (bullet.turretParent.TurnAttackIntensify > KeyValue * 19.5f)
            return;
        bullet.turretParent.TurnAttackIntensify += KeyValue;
    }
}

public class EnemyCountAttackIncrease : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.EnemyCountAttackIncrease;
    public override string EffectDescription => "������Χ��ÿ��1�����ˣ�������" + KeyValue * 100 + "%��������";



}

public class CurrentHealthBaseDmage : TurretEffect
{
    public override TurretEffectName EffectName => TurretEffectName.CurrentHealthBaseDamage;

    public override void Hit(Enemy target)
    {
        float realDamage;
        float extraDamage = target.CurrentHealth * KeyValue;
        target.ApplyDamage(extraDamage,out realDamage);
        turret.DamageAnalysis += (int)realDamage;
        Debug.Log("DealHealthBaseDamage:" + extraDamage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackEffectName
{
    DistanceBaseDamage,SlowBullet
}
[System.Serializable]
public class AttackEffectInfo
{
    public AttackEffectName EffectName;
    public float KeyValue;
}
public abstract class AttackEffect
{
    public abstract AttackEffectName EffectName { get; }

    public Bullet bullet;
    public float KeyValue;

    public virtual void Shoot()
    {

    }

    public virtual void Hit(Enemy target)
    {

    }
}

public class DistanceBaseDamage : AttackEffect
{
    public override AttackEffectName EffectName => AttackEffectName.DistanceBaseDamage;

    public override void Shoot()
    {
        float distance = ((Vector2)bullet.transform.position - bullet.Target.Position).magnitude;
        bullet.Damage *= (1 + distance * KeyValue);
    }
}

public class SlowBullet : AttackEffect
{
    public override AttackEffectName EffectName => AttackEffectName.SlowBullet;
    public float Duration = 2f;
    public override void Hit(Enemy target)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, KeyValue, Duration);
        target.Buffable.AddBuff(info);
    }

}

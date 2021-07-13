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
    public override string SkillDescription => "轮转炮会向周围随机发射能量弹，每次发射提升0.1攻速，上限为10";
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
    public override string SkillDescription => "子弹随飞行距离提升30%/格的伤害，狙击塔相邻每个空格使该效果提升10%";

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
    public override string SkillDescription => "散射炮造成的伤害降低50%，但是可以额外攻击3个目标";
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
    public override string SkillDescription => "巨炮的所有暴击伤害提升效果翻倍";

    public override void PreHit()
    {
        float increase = bullet.CriticalPercentage - 1.5f;
        bullet.CriticalPercentage += increase * 2f;
    }

}
public class H1SkillCountBaseSputteringPercentage : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.HISkillCountBaseSputteringPercentage;
    public override string SkillDescription => "迫击炮的炮弹每溅射到1个敌人，就会使溅射伤害提高30%";


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
    public override string SkillDescription => "疾射炮每次攻击都会提升自身10%的攻击力，最多叠加20次";

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
    public override string SkillDescription => "组合炮的攻击范围内每有1个敌人，造成的伤害就提高10%";

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

    public override string SkillDescription => "发射一个移动到直线路径末端的雪球，雪球使敌人的当前减速效果翻倍";
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

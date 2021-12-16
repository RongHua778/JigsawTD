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
    CoreSkill,
    None
}


public abstract class TurretSkill
{
    public virtual TurretSkillName EffectName { get; }
    public virtual string SkillDescription { get; set; }
    public StrategyBase strategy;
    //public Bullet bullet;
    public virtual string DisplayValue { get; set; }
    public virtual float KeyValue { get; }
    public virtual ElementType IntensifyElement => ElementType.None;
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

    public virtual void OnEnter(IDamageable target)
    {

    }

    public virtual void OnExit(IDamageable target)
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

    public override void Shoot(Bullet bullet = null)
    {
        base.Shoot(bullet);
        float distance = bullet.GetTargetDistance();
        bullet.Damage *= (1 + distance * 0.5f);
    }

    //public override void Build()
    //{
    //    base.Build();
    //    strategy.ForbidRange += 2;
    //}


    //public override void Detect()
    //{
    //    strategy.InitAttackIntensify -= attackIncreased;
    //    attackIncreased = strategy.FinalRange * 0.1f;
    //    strategy.InitAttackIntensify += attackIncreased;
    //}
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

    //public override void PreHit(Bullet bullet = null)
    //{
    //    float increase = bullet.CriticalPercentage - 1.5f;
    //    bullet.CriticalPercentage += increase * 2f * strategy.TimeModify;
    //}

    public override void Shoot(Bullet bullet = null)
    {
        bullet.CriticalPercentage += Random.Range(-1f, 4f) * strategy.TimeModify;
    }

}
public class MortarSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.MortarSkill;
    public override string SkillDescription => "MORTARSKILL";


    public override void Shoot(Bullet bullet = null)
    {
        base.Shoot(bullet);
        bullet.SplashPercentage += (bullet.SplashRange / 0.1f) * 0.1f;
    }

}



public class RapiderSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.RapiderSkill;
    public override string SkillDescription => "RAPIDERSKILL";

    private IDamageable lastTarget;
    private float speedIncreased;

    public override void Shoot(Bullet bullet = null)
    {
        IDamageable target = strategy.Turret.Target[0].Enemy;
        if (target != lastTarget)
        {
            lastTarget = target;
            strategy.TurnFireRateIntensify -= speedIncreased;
            speedIncreased = 0;
            return;
        }
        if (speedIncreased < 1.99f)
        {
            speedIncreased += 0.2f * strategy.TimeModify;
            strategy.TurnFireRateIntensify += 0.2f * strategy.TimeModify;
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

    public override void OnEnter(IDamageable target)
    {
        strategy.TurnAttackIntensify += 0.1f * strategy.TimeModify;
    }
    public override void OnExit(IDamageable target)
    {
        strategy.TurnAttackIntensify -= 0.1f * strategy.TimeModify;
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
        strategy.TurnAttackIntensify += 0.02f * GameRes.TotalCooporative * strategy.TimeModify;
    }
}

public class BoomerrangSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.BoomerrangSkill;
    public override string SkillDescription => "BOOMERRANGSKILL";


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
        Duration += 30;
    }

    public override void TickEnd()
    {
        strategy.TurnFixAttack += strategy.FinalAttack * strategy.TimeModify;
        strategy.TurnFixSpeed += strategy.FinalFireRate * strategy.TimeModify;
        strategy.TurnFixCriticalRate += strategy.FinalCriticalRate * strategy.TimeModify;
        strategy.TurnFixSlowRate += strategy.FinalSlowRate * strategy.TimeModify;
        strategy.TurnFixSplashRange += strategy.FinalSplashRange * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class CoreSkill : InitialSkill
{
    public override TurretSkillName EffectName => TurretSkillName.CoreSkill;
    public override string SkillDescription => "CORESKILL";
    public override void StartTurn()
    {
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        StrategyBase turretStrategy;
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.Turret.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
            {
                turretStrategy = hit.GetComponent<TurretContent>().Strategy;
                float intensify = 0;
                switch (strategy.Quality)
                {
                    case 1:
                        intensify = 0.7f;
                        break;
                    case 2:
                        intensify = 1f;
                        break;
                    case 3:
                        intensify = 1.5f;
                        break;
                }
                strategy.InitAttack += turretStrategy.BaseAttack * intensify * strategy.TimeModify;
            }
        }
    }

    public override void EndTurn()
    {
        strategy.InitAttack = 0;
    }
}



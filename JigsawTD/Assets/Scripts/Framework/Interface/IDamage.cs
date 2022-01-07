using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    DamageStrategy DamageStrategy { get; set; }
    HealthBar HealthBar { get; set; }
}


public abstract class DamageStrategy
{
    public IDamage damageTarget;
    public Transform ModelTrans;

    protected float currentHealth;
    protected float maxHealth;
    private bool isDie;
    private int trapIntensify = 1;
    public float StunTime;
    public virtual int TrapIntensify
    {
        get => trapIntensify;
        set => trapIntensify = value;
    }
    public virtual bool IsDie
    {
        get => isDie;
        set => isDie = value;
    }
    public abstract bool IsEnemy { get; }
    public virtual float DamageIntensify { get => BuffDamageIntensify; }
    public virtual float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0 && maxHealth > 0)
            {
                IsDie = true;
            }
            damageTarget.HealthBar.FillAmount = currentHealth / MaxHealth;
        }
    }
    public virtual float MaxHealth { get => maxHealth; set { maxHealth = value; CurrentHealth = maxHealth; } }

    //buff属性
    private float buffDamageIntensify;
    public virtual float BuffDamageIntensify { get => buffDamageIntensify; set => buffDamageIntensify = value; }


    public DamageStrategy(IDamage damageTarget)
    {
        this.damageTarget = damageTarget;
    }

    public virtual void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount * (1 + DamageIntensify);
        CurrentHealth -= realDamage;
        GameRes.TotalDamage += (int)realDamage;
        StaticData.Instance.ShowJumpDamage(ModelTrans.position, (int)realDamage, isCritical);

    }


    public virtual void ApplyBuff(EnemyBuffName buffName, float keyvalue, float duration)
    {

    }

    public virtual void ResetStrategy(EnemyAttribute attribute, float intensify,float slowResist)
    {
        IsDie = false;
        this.MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
        BuffDamageIntensify = 0;
    }

    public virtual void StrategyUpdate()
    {

    }
}

public class BasicEnemyStrategy : DamageStrategy
{
    public override bool IsEnemy => true;
    protected Enemy enemy;
    public float MaxFrost = 2f;
    public float UnfrostableTime;
    private FrostEffect m_FrostEffect;

    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            UnFrost();
        }
    }
    private float frostTime;
    private float currentFrost;
    public float CurrentFrost
    {
        get => currentFrost;
        set
        {
            if (UnfrostableTime > 0)
                return;

            currentFrost = value;

            if (currentFrost >= MaxFrost)
            {
                currentFrost = 0;
                FrostEnemy(3f);
            }
            enemy.HealthBar.FrostAmount = CurrentFrost / MaxFrost;
        }
    }

    public override int TrapIntensify
    {
        get => base.TrapIntensify;
        set
        {
            base.TrapIntensify = value;
            enemy.HealthBar.ShowIcon(3, value > 1);
        }
    }

    public BasicEnemyStrategy(IDamage damageTarget) : base(damageTarget)
    {
        this.enemy = damageTarget as Enemy;
        this.ModelTrans = enemy.model;
    }

    public void UnFrost()
    {
        if (m_FrostEffect != null)
        {
            m_FrostEffect.Broke();
            m_FrostEffect = null;
        }
    }

    public override float BuffDamageIntensify
    {
        get => base.BuffDamageIntensify;
        set
        {
            base.BuffDamageIntensify = value;
            enemy.HealthBar.ShowIcon(1, value > 0);
        }
    }

    public override void ApplyBuff(EnemyBuffName buffName, float keyvalue, float duration)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, keyvalue, duration);
        enemy.Buffable.AddBuff(info);
    }

    public override void ResetStrategy(EnemyAttribute attribute, float intensify,float slowResist)
    {
        base.ResetStrategy(attribute, intensify, slowResist);
        MaxFrost = attribute.Frost * slowResist;
        TrapIntensify = 1;
        CurrentFrost = 0;
        StunTime = 0;
        frostTime = 0; 
        UnfrostableTime = 0;
    }

    public void FrostEnemy(float time)
    {
        FrostEffect frosteffect = ObjectPool.Instance.Spawn(StaticData.Instance.FrostEffectPrefab) as FrostEffect;
        frosteffect.transform.position = ModelTrans.position;
        frosteffect.transform.localScale = Vector3.one * 0.85f;
        frostTime += time;
        StunTime += time;
        UnfrostableTime += 6f;//免疫冻结时间
        m_FrostEffect = frosteffect;
        Sound.Instance.PlayEffect("Sound_EnemyExplosionFrost");
    }

    public override void StrategyUpdate()
    {
        if (frostTime > 0)
        {
            frostTime -= Time.deltaTime;
            if (frostTime <= 0.2f)
            {
                UnFrost();
            }
        }
        if (StunTime > 0)
        {
            StunTime -= Time.deltaTime;
        }
        if (UnfrostableTime > 0)
        {
            UnfrostableTime -= Time.deltaTime;
        }
    }

}



public class ArmourStrategy : DamageStrategy
{
    public override bool IsEnemy => false;
    Armor armor;
    public ArmourStrategy(IDamage damageTarget) : base(damageTarget)
    {
        this.damageTarget = damageTarget;
        this.armor = damageTarget as Armor;
        this.ModelTrans = armor.transform;

    }
    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            if (value)
                armor.DisArmor();
        }
    }

}

public class AircraftStrategy : DamageStrategy
{
    public override bool IsEnemy => false;
    Aircraft aircraft;
    public AircraftStrategy(IDamage damageTarget) : base(damageTarget)
    {
        this.damageTarget = damageTarget;
        this.aircraft = damageTarget as Aircraft;
        this.ModelTrans = aircraft.transform;

    }

}

public class RestorerStrategy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;
    public float damagedCounter;

    public RestorerStrategy(IDamage damageTarget) : base(damageTarget)
    {
    }

    public override void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        base.ApplyDamage(amount, out realDamage, isCritical);
        damagedCounter = 0;
    }
}

public class HamsterStrategy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;

    public override float DamageIntensify => base.DamageIntensify + HamsterDamageIntensify;
    public float HamsterDamageIntensify => (-Hamster.HamsterCount * 0.25f) + 0.5f;


    public HamsterStrategy(IDamage damageTarget) : base(damageTarget)
    {
    }

}




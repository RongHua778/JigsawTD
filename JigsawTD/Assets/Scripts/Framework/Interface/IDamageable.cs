using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    //void ApplyDamage(float amount, out float realDamag, bool isCritical = false);

    //void ApplyBuff(EnemyBuffName buffName, float keyValue, float duration);
    //float DamageIntensify { get; set; }
    //float CurrentHealth { get; set; }
    //float MaxHealth { get; set; }
    public bool IsDie { get; set; }
    DamageStrategy DamageStrategy { get; set; }

}

public abstract class DamageStrategy
{
    public GameObject TargetObj;
    protected float damageIntensify;
    protected float currentHealth;
    protected float maxHealth;
    public abstract bool IsEnemy { get; }
    public virtual float DamageIntensify { get => damageIntensify; set => damageIntensify = value; }
    public virtual float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public virtual float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public DamageStrategy(GameObject obj)
    {
        this.TargetObj = obj;
    }

    public virtual void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount * (1 + DamageIntensify);
        CurrentHealth -= realDamage;
        GameEndUI.TotalDamage += (int)realDamage;
        if (isCritical)
        {
            StaticData.Instance.ShowJumpDamage(TargetObj.transform.position, (int)realDamage);
        }
    }


    public virtual void ApplyBuff(EnemyBuffName buffName, float keyvalue, float duration)
    {

    }
}

public class EnemyDamageStrategy : DamageStrategy
{
    public override bool IsEnemy => true;
    Enemy enemy;
    public EnemyDamageStrategy(GameObject obj, Enemy enemy) : base(obj)
    {
        this.TargetObj = obj;
        this.enemy = enemy;
    }
    public override float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            CurrentHealth = maxHealth;
        }
    }
    public override float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0 && maxHealth > 0)
            {
                enemy.IsDie = true;
            }
            enemy.HealthBar.FillAmount = currentHealth / MaxHealth;
        }
    }

    public override void ApplyBuff(EnemyBuffName buffName, float keyvalue, float duration)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, keyvalue, duration);
        enemy.Buffable.AddBuff(info);
    }

}

public class ArmourStrategy : DamageStrategy
{
    public override bool IsEnemy => false;
    Armor armor;
    public ArmourStrategy(GameObject obj, Armor armor) : base(obj)
    {
        this.TargetObj = obj;
        this.armor = armor;
    }
    public override float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                ReusableObject explosion = ObjectPool.Instance.Spawn(armor.explosionPrefab);
                Sound.Instance.PlayEffect(armor.explosionClip);
                explosion.transform.position = TargetObj.transform.position;
                armor.DisArmor();
            }
        }
    }
}




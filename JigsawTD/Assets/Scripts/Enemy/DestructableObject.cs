using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Enemy,Armor
}
public abstract class DestructableObject : ReusableObject, IGameBehavior
{
    [SerializeField] protected ReusableObject exlposionPrefab = default;
    protected AudioClip explosionClip;
    protected Animator anim;

    public GameTile CurrentTile;
    public float TargetDamageCounter { get; set; }

    float damageIntensify;
    public float DamageIntensify { get => damageIntensify; set => damageIntensify = value; }
    public BuffableEntity Buffable { get;  set; }

    private bool isDie = false;
    public bool IsDie
    {
        get => isDie;
        set
        {
            isDie = value;
        }
    }

    private float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    protected float currentHealth;
    public virtual float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0)
            {
                IsDie = true;
            }
        }
    }

    public ObjectType Type { get => type; set => type = value; }

    ObjectType type;

    public virtual void Awake()
    {
        anim = this.GetComponent<Animator>();
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
    }

    public virtual bool GameUpdate()
    {
        if (IsDie)
        {
            //StopAllCoroutines();
            //ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            //Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            //explosion.transform.position = model.transform.position;
            //Debug.LogWarning("“—À¿");
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        return true;
    }

    public virtual void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount * (1 + DamageIntensify);
        CurrentHealth -= realDamage;
        TargetDamageCounter += realDamage;
        GameEndUI.TotalDamage += (int)realDamage;
        
    }

    public override void OnSpawn()
    {
        base.OnUnSpawn();
        IsDie = false;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        TargetDamageCounter = 0;
    }
}

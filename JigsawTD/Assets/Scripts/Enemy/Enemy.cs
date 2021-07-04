using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PathFollower,IDamageable
{
    [SerializeField] protected ReusableObject exlposionPrefab = default;
    protected AudioClip explosionClip;
    protected Animator anim;

    public TrapContent CurrentTrap;
    public abstract EnemyType EnemyType { get; }

    protected bool trapTriggered = false;
    public int ReachDamage { get; set; }
    public int TileStunCounter { get; set; }
    private float stunTime;
    public float StunTime
    {
        get => stunTime;
        set
        {
            stunTime = value;
            ProgressFactor = Speed * Adjust;
        }
    }

    int affectHealerCount = 0;
    public int AffectHealerCount { get => affectHealerCount; set => affectHealerCount = value; }
    float speedIntensify = 0;
    public virtual float SpeedIntensify { get => speedIntensify + AffectHealerCount > 0 ? 0.4f : 0; set => speedIntensify = Mathf.Min(2, value); }

    public float initialSpeed;
    public override float Speed { get => StunTime > 0 ? 0 : Mathf.Max(0.1f, (speed + SpeedIntensify) * (1 - (SlowRate + PathSlow) / (SlowRate + PathSlow + 1))); set => speed = value; }

    float slowRate;
    public float SlowRate
    {
        get => slowRate;
        set
        {
            slowRate = value;
            ProgressFactor = Speed * Adjust;//子弹减速即时更新速度
            healthBar.ShowSlowIcon(slowRate > 0.01f);
        }
    }
    float pathSlow;
    public float PathSlow
    {
        get => pathSlow;
        set
        {
            pathSlow = value;
            ProgressFactor = Speed * Adjust;//子弹减速即时更新速度
        }
    }
    int brokeShell;
    public int BrokeShell { get => brokeShell; set => brokeShell = value; }

    public float TargetDamageCounter { get; set; }

    float damageIntensify;
    public float DamageIntensify { get => damageIntensify; set => damageIntensify = value; }
    public BuffableEntity Buffable { get; set; }

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
            if (currentHealth <= 0 && maxHealth > 0)
            {
                IsDie = true;
            }
            healthBar.FillAmount = currentHealth / MaxHealth;
        }
    }

    [Header("HealthSetting")]
    HealthBar healthBar;

    public virtual void Awake()
    {
        anim = this.GetComponent<Animator>();
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
    }

    public override bool GameUpdate()
    {
        if (IsDie)
        {
            StopAllCoroutines();
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            explosion.transform.position = model.transform.position;
            GameEvents.Instance.EnemyDie(this);
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        if (StunTime >= 0)
        {
            StunTime -= Time.deltaTime;
            if (StunTime < 0)
                ProgressFactor = Speed * Adjust;
        }
        Progress += Time.deltaTime * ProgressFactor;

        if (!trapTriggered && Progress >= 0.5f)
        {
            TriigerTrap();
        }

        while (Progress >= 1f)
        {
            if (PointIndex == pathPoints.Count - 1)
            {
                try
                {
                    StartCoroutine(ExitCor());
                }
                catch
                {
                    Debug.LogAssertion("线程丢失");
                }
                return false;
            }
            trapTriggered = false;
            Progress = 0;
            PrepareNextState();
        }
        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, Progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, Progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    protected void TriigerTrap()
    {
        if (CurrentTrap != null)
            CurrentTrap.OnContentPass(this);
        CurrentTrap = null;
        trapTriggered = true;
    }

    protected override void PrepareNextState()
    {
        base.PrepareNextState();
        Buffable.TileTick();
    }

    protected IEnumerator ExitCor()
    {
        anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this);
    }
    public virtual void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        this.pathOffset = pathOffset;
        this.healthBar = healthBar;
        this.healthBar.followTrans = model;
        Buffable = this.GetComponent<BuffableEntity>();
        CurrentHealth = MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
        Speed = attribute.Speed;
        initialSpeed = attribute.Speed;
        DamageIntensify = attribute.Shell;
        ReachDamage = attribute.ReachDamage;
    }

    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        anim.Play("Default");
        anim.SetTrigger("Enter");
    }

    public virtual void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount * (1 + DamageIntensify);
        CurrentHealth -= realDamage;
        TargetDamageCounter += realDamage;
        GameEndUI.TotalDamage += (int)realDamage;

        if (isCritical)
        {
            healthBar.ShowJumpDamage((int)realDamage);
        }
    }


    public override void OnSpawn()
    {
        base.OnSpawn();
        IsDie = false;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        ObjectPool.Instance.UnSpawn(healthBar);
        TargetDamageCounter = 0;
        TileStunCounter = 0;
        DamageIntensify = 0;
        SpeedIntensify = 0;
        AffectHealerCount = 0;
        PathSlow = 0;
        SlowRate = 0;
        BrokeShell = 0;
        StunTime = 0;
        CurrentTrap = null;
        Buffable.RemoveAllBuffs();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PathFollower, IDamageable
{
    [Header("基本配置")]
    public bool IsBoss = false;
    protected HealthBar healthBar;
    protected SpriteRenderer enemySprite;
    protected CircleCollider2D enemyCol;

    //状态配置
    public float Intensify;
    protected bool isOutTroing = false;//正在消失
    protected bool trapTriggered = false;
    public bool IsEnemy { get => true; }
    public abstract EnemyType EnemyType { get; }


    [Header("资源配置")]
    [SerializeField] protected ReusableObject exlposionPrefab = default;
    protected AudioClip explosionClip;
    private Animator anim;
    public Animator Anim { get => anim; set => anim = value; }

    private TrapContent currentTrap;
    public TrapContent CurrentTrap { get => currentTrap; set => currentTrap = value; }

    private List<TrapContent> passedTraps=new List<TrapContent>();
    public List<TrapContent> PassedTraps { get => passedTraps; set => passedTraps = value; }

    private List<Skill> skills;
    public List<Skill> Skills { get => skills; set => skills = value; }
    public int ReachDamage { get; set; }

    private float stunTime;
    public float StunTime//眩晕时间
    {
        get => stunTime;
        set
        {
            stunTime = value;
            ProgressFactor = Speed * Adjust;
        }
    }

    private int affectHealerCount = 0;
    public int AffectHealerCount { get => affectHealerCount; set => affectHealerCount = value; }
    float speedIntensify = 0;
    public virtual float SpeedIntensify { get => speedIntensify + AffectHealerCount > 0 ? 0.4f : 0; set => speedIntensify = Mathf.Min(2, value); }
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
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            CurrentHealth = maxHealth;
        }
    }
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


    float trapIntentify=1f;
    public float TrapIntentify { get => trapIntentify; set => trapIntentify = value; }


    public virtual void Awake()
    {
        enemySprite = transform.Find("Model").Find("GFX").GetComponent<SpriteRenderer>();
        enemyCol = enemySprite.GetComponent<CircleCollider2D>();
        Anim = GetComponent<Animator>();
        if (IsBoss)
        {
            explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_BossExplosion");
        }
        else
        {
            explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
        }
    }

    public override bool GameUpdate()
    {
        //if (EnemySkills != null)
        //{
            //foreach (Skill enemySkill in EnemySkills)
            //{
            //    enemySkill.OnGameUpdating();
            //}
        //}
        if (PassedTraps != null)
        {
            foreach (TrapContent trap in PassedTraps)
            {
                trap.OnGameUpdating(this);
            }
        }
        OnEnemyUpdate();
        if (IsDie)
        {
            //if (EnemyTraps != null)
            //{
            //    foreach (EnemyTrap enemySkill in EnemyTraps)
            //    {
            //        enemySkill.OnDying();
            //    }
            //}
            OnDie();
            StopAllCoroutines();
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip);
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
            //TriigerTrap();
        }

        while (Progress >= 1f)
        {
            if (PointIndex == PathPoints.Count - 1)
            {
                isOutTroing = true;
                StartCoroutine(ExitCor());
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

    protected virtual void OnEnemyUpdate()
    {

    }
    protected virtual void OnDie()
    {

    }

    public void TriigerTrap()
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
        Anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this);
    }
    public virtual void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        this.PathOffset = pathOffset;
        this.healthBar = healthBar;
        this.healthBar.followTrans = model;
        this.Intensify = intensify;
        Buffable = this.GetComponent<BuffableEntity>();
        MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
        Speed = attribute.Speed;
        DamageIntensify = attribute.Shell;
        ReachDamage = attribute.ReachDamage;
        if (Skills != null)
        {
            foreach (Skill enemySkill in Skills)
            {
                enemySkill.OnBorn();
            }
        }
        PassedTraps = new List<TrapContent>();
    }
   
    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        Anim.Play("Default");
        Anim.SetTrigger("Enter");
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
        isOutTroing = false;
        ObjectPool.Instance.UnSpawn(healthBar);
        TargetDamageCounter = 0;
        DamageIntensify = 0;
        SpeedIntensify = 0;
        AffectHealerCount = 0;
        PathSlow = 0;
        SlowRate = 0;
        StunTime = 0;
        CurrentTrap = null;
        Buffable.RemoveAllBuffs();
        if (skills != null)
        {
            skills.Clear();
        }
        TrapIntentify = 1f;
    }
}

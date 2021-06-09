using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy :PathFollower
{
    public abstract EnemyType EnemyType { get; }

    private Animator anim;
    private AudioClip explosionClip;

    [SerializeField] ReusableObject exlposionPrefab = default;
    public int ReachDamage { get; set; }
    public float TargetDamageCounter { get; set; }
    public int TileStunCounter { get; set; }
    private float stunTime;
    public float StunTime 
    { 
        get=>stunTime; 
        set 
        {
            stunTime = value;
            progressFactor = Speed * adjust;
        } 
    }

    public override float Speed { get => StunTime > 0 ? 0 : speed * (1 - (SlowRate + PathSlow) / (SlowRate + PathSlow + 0.7f)); set => speed = value; }
    int shell;
    public int Shell { get => Mathf.Max(0, shell - BrokeShell); set => shell = value; }
    float slowRate;
    public float SlowRate
    {
        get => slowRate;
        set
        {
            slowRate = value;
            progressFactor = Speed * adjust;//子弹减速即时更新速度
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
            progressFactor = Speed * adjust;//子弹减速即时更新速度
        }
    }
    int brokeShell;
    public int BrokeShell { get => brokeShell; set => brokeShell = value; }


    public BuffableEntity Buffable { get; private set; }



    [Header("HealthSetting")]
    HealthBar healthBar;
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
    private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            healthBar.FillAmount = CurrentHealth / MaxHealth;
            if (currentHealth <= 0)
            {
                IsDie = true;
            }
        }
    }

    private void Awake()
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
                progressFactor = Speed * adjust;
        }
        progress += Time.deltaTime * progressFactor;
        if (!tileEffectTriigger && progress >= 0.5f)
        {
            TriggetTileEffect();
        }
        while (progress >= 1f)
        {
            if (tileTo == null)
            {
                StartCoroutine(ExitCor());
                return false;
            }
            progress = 0;
            tileEffectTriigger = false;
            PrepareNextState();
        }
        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    bool tileEffectTriigger = false;
    private void TriggetTileEffect()
    {
        Buffable.TileTick();//先移除BUFF再加BUFF//放在Prepare前面，因为要提前改变Path速度
        tileFrom.OnTilePass(this);
        tileEffectTriigger = true;
        
    }

    private IEnumerator ExitCor()
    {
        anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this);
    }
    public void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        this.pathOffset = pathOffset;
        this.healthBar = healthBar;
        this.healthBar.followTrans = model;
        Buffable = this.GetComponent<BuffableEntity>();
        CurrentHealth = MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
        Speed = attribute.Speed;
        Shell = attribute.Shell;
        ReachDamage = attribute.ReachDamage;

    }



    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        anim.Play("Default");
        anim.SetTrigger("Enter");

    }

    private void PrepareOutro()
    {
        positionTo = tileFrom.transform.localPosition;
        DirectionChange = DirectionChange.None;
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localRotation = Direction.GetRotation();
        adjust = 2f;
        progressFactor = adjust * Speed;
    }


    public virtual void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount * 5 / (5 + Shell);
        if (isCritical)
        {
            healthBar.ShowJumpDamage((int)realDamage);
        }
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
        ObjectPool.Instance.UnSpawn(healthBar);
        TargetDamageCounter = 0;
        TileStunCounter = 0;
        PathSlow = 0;
        SlowRate = 0;
        BrokeShell = 0;
        StunTime = 0;
        Buffable.RemoveAllBuffs();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PathFollower, IDamageable
{
    [Header("基本配置")]
    public bool IsBoss = false;
    protected HealthBar healthBar;
    public HealthBar HealthBar { get => healthBar; }
    protected SpriteRenderer enemySprite;
    protected CircleCollider2D enemyCol;

    public DamageStrategy DamageStrategy { get; set; }

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


    //上一个经过的陷阱
    private TrapContent lastTrap;
    public TrapContent LastTrap { get => lastTrap; set => lastTrap = value; }
    //经过的陷阱列表
    private List<TrapContent> passedTraps = new List<TrapContent>();
    public List<TrapContent> PassedTraps { get => passedTraps; set => passedTraps = value; }
    //陷阱强化值
    float trapIntentify = 1f;
    public float TrapIntentify
    {
        get => trapIntentify;
        set
        {
            trapIntentify = value;
            healthBar.ShowPromoteIcon(trapIntentify > 1);
        }
    }

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
            healthBar.ShowSlowIcon(slowRate > 0f);
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
    public float DamageIntensify
    {
        get => damageIntensify;
        set
        {
            damageIntensify = value;
            healthBar.ShowDamageIcon(damageIntensify > 0);
        }
    }
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



    public virtual void Awake()
    {
        DamageStrategy = new EnemyDamageStrategy(this.gameObject, this);
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
        PassedTraps = new List<TrapContent>();
    }

    public override bool GameUpdate()
    {
        OnEnemyUpdate();
        if (IsDie)
        {
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
        if (LastTrap != null)
            LastTrap.OnContentPass(this);
        LastTrap = null;
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
        DamageStrategy.MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
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
        DamageStrategy.CurrentHealth -= realDamage;
        TargetDamageCounter += realDamage;
        GameEndUI.TotalDamage += (int)realDamage;
        if (isCritical)
        {
            StaticData.Instance.ShowJumpDamage(transform.position, (int)realDamage);
        }
    }

    public void Flash(int distance)
    {
        PointIndex -= distance;
        if (PointIndex < 0)
        {
            PointIndex = 0;
        }
        else if (PointIndex >= PathPoints.Count - 1)
        {
            PointIndex = PathPoints.Count - 1;
        }
        CurrentPoint = PathPoints[PointIndex];
        transform.localPosition = PathPoints[PointIndex].PathPos;
        PositionFrom = CurrentPoint.PathPos;
        PositionTo = CurrentPoint.ExitPoint;
        Direction = CurrentPoint.PathDirection;
        DirectionChange = DirectionChange.None;
        model.localPosition = new Vector3(PathOffset, 0);
        DirectionAngleFrom = DirectionAngleTo = Direction.GetAngle();
        transform.localRotation = CurrentPoint.PathDirection.GetRotation();
        Progress = 0.5f;
    }

    public void ApplyBuff(EnemyBuffName buffName, float keyvalue, float duration)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, keyvalue, duration);
        Buffable.AddBuff(info);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        IsDie = false;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        PassedTraps.Clear();
        isOutTroing = false;
        ObjectPool.Instance.UnSpawn(healthBar);
        TargetDamageCounter = 0;
        DamageIntensify = 0;
        SpeedIntensify = 0;
        AffectHealerCount = 0;
        PathSlow = 0;
        SlowRate = 0;
        StunTime = 0;
        LastTrap = null;
        Buffable.RemoveAllBuffs();
        if (skills != null)
        {
            skills.Clear();
        }
        TrapIntentify = 1f;
    }
}

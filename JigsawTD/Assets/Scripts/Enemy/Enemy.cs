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

    //寻路及陷阱触发
    private List<BasicTile> pathTiles;
    private BasicTile currentTile;
    public BasicTile CurrentTile
    {
        get => currentTile;
        set
        {
            currentTile = value;
            currentTile.OnTilePass(this);
        }
    }

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


    //经过的陷阱列表
    private List<TrapContent> passedTraps = new List<TrapContent>();
    public List<TrapContent> PassedTraps { get => passedTraps; set => passedTraps = value; }

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
    public override float Speed { get => StunTime > 0 ? 0 : Mathf.Max(0.1f, (speed + SpeedIntensify) * (1 - SlowRate / (SlowRate + 2f))); set => speed = value; }

    float slowRate = 0;
    public float SlowRate
    {
        get => slowRate;
        set
        {
            slowRate = value;
            ProgressFactor = Speed * Adjust;//子弹减速即时更新速度
            healthBar.ShowIcon(0, slowRate > 0f);
        }
    }

    public float TargetDamageCounter { get; set; }

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


    public virtual void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> shortpath)
    {
        this.pathTiles = shortpath;
        this.PathOffset = pathOffset;
        this.healthBar = healthBar;
        this.healthBar.followTrans = model;
        this.Intensify = intensify;
        Buffable = this.GetComponent<BuffableEntity>();
        this.DamageStrategy.MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
        this.DamageStrategy.ResetStrategy();//清除加成
        Speed = attribute.Speed;
        ReachDamage = attribute.ReachDamage;

    }

    public virtual void Awake()
    {
        SetStrategy();
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

    protected virtual void SetStrategy()
    {
        DamageStrategy = new EnemyDamageStrategy(model, this);
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


    protected override void PrepareNextState()
    {
        base.PrepareNextState();
        CurrentTile.OnTileLeave(this);//离开格子效果
        CurrentTile = pathTiles[PointIndex];
        Buffable.TileTick();
    }

    protected IEnumerator ExitCor()
    {
        Anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this);
    }


    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        CurrentTile = pathTiles[PointIndex];
        Anim.Play("Default");
        Anim.SetTrigger("Enter");
    }


    public void Flash(int distance)
    {
        CurrentTile.OnTileLeave(this);
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
        CurrentTile = pathTiles[PointIndex];
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
        SpeedIntensify = 0;
        AffectHealerCount = 0;

        SlowRate = 0;
        StunTime = 0;
        Buffable.RemoveAllBuffs();

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PathFollower, IDamageable
{
    public virtual string ExplosionSound => "Sound_EnemyExplosion";
    public virtual string ExplosionEffect { get; }
    protected ReusableObject ExplosionPrefab;
    [Header("基本配置")]
    protected Animator anim;
    public HealthBar HealthBar { get; set; }
    protected SpriteRenderer enemySprite;
    protected CircleCollider2D enemyCol;
    protected EnemyAttribute m_Attribute;
    public DamageStrategy DamageStrategy { get; set; }

    //寻路及陷阱触发
    private List<BasicTile> pathTiles;
    private BasicTile currentTile;
    public BasicTile CurrentTile
    {
        get => currentTile;
        set
        {
            Buffable.TileTick();
            currentTile = value;
            currentTile.OnTilePass(this);
        }
    }

    //状态配置
    protected float Intensify;
    protected bool isOutTroing = false;//正在消失
    protected bool trapTriggered = false;
    public bool IsEnemy { get => true; }
    public virtual EnemyType EnemyType { get; }

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
    public virtual float SpeedIntensify { get => speedIntensify + (AffectHealerCount > 0 ? 0.6f : 0); set => speedIntensify = value; }
    public override float Speed { get => StunTime > 0 ? 0 : Mathf.Max(0.1f, (speed + SpeedIntensify) * (1 - SlowRate / (SlowRate + 2f))); }

    float slowRate = 0;
    public float SlowRate
    {
        get => slowRate;
        set
        {
            slowRate = value;
            ProgressFactor = Speed * Adjust;//子弹减速即时更新速度
            HealthBar.ShowIcon(0, slowRate > 0f);
        }
    }

    public float TargetDamageCounter { get; set; }

    public BuffableEntity Buffable { get; set; }
    public float SlowResist { get; set; }//减速抗性

    public virtual void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        this.pathTiles = BoardSystem.shortestPath;
        this.PathOffset = pathOffset;
        this.Intensify = intensify;
        this.DamageStrategy.ResetStrategy(Mathf.RoundToInt(attribute.Health * intensify));//清除加成
        this.speed = attribute.Speed;
        this.ReachDamage = attribute.ReachDamage;
        this.SlowResist = (float)GameRes.CurrentWave / (GameRes.CurrentWave + 20);
        SpawnOn(pathIndex, BoardSystem.shortestPoints);

    }

    public virtual void Awake()
    {
        SetStrategy();
        HealthBar = model.GetComponentInChildren<HealthBar>();
        enemySprite = model.Find("GFX").GetComponent<SpriteRenderer>();

        Buffable = this.GetComponent<BuffableEntity>();
        enemyCol = enemySprite.GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();

        ExplosionPrefab = Resources.Load<ReusableObject>("Prefabs/Effects/Enemy/" + ExplosionEffect);


    }

    protected virtual void SetStrategy()
    {
        DamageStrategy = new BasicEnemyStrategy(this);

    }
    public override bool GameUpdate()
    {
        OnEnemyUpdate();
        if (DamageStrategy.IsDie)
        {
            OnDie();
            StopAllCoroutines();
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

        if (Progress >= 0.5f && !trapTriggered)
        {
            trapTriggered = true;
            CurrentTile = pathTiles[PointIndex];
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
        ReusableObject explosion = ObjectPool.Instance.Spawn(ExplosionPrefab);
        explosion.transform.position = model.position;
        Sound.Instance.PlayEffect(ExplosionSound);
    }


    protected override void PrepareNextState()
    {
        base.PrepareNextState();

    }

    protected IEnumerator ExitCor()
    {
        anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this);
    }


    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        CurrentTile = pathTiles[PointIndex];
        anim.Play("Default");
        anim.SetTrigger("Enter");
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

        CurrentTile = pathTiles[PointIndex];
        trapTriggered = true;

        transform.localPosition = PathPoints[PointIndex].PathPos;
        PositionFrom = CurrentPoint.PathPos;
        PositionTo = CurrentPoint.ExitPoint;
        Direction = CurrentPoint.PathDirection;
        DirectionChange = DirectionChange.None;
        model.localPosition = new Vector3(PathOffset, 0);
        DirectionAngleFrom = DirectionAngleTo = Direction.GetAngle();
        transform.localRotation = CurrentPoint.PathDirection.GetRotation();

        Progress = 0f;
        Adjust = 2f;
        ProgressFactor = Adjust * Speed;
        anim.Play("Default");
        anim.SetTrigger("Enter");
    }



    public void ApplyBuff(EnemyBuffName buffName, float keyvalue, float duration)
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, keyvalue, duration);
        Buffable.AddBuff(info);
    }



    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        PassedTraps.Clear();
        isOutTroing = false;
        //ObjectPool.Instance.UnSpawn(healthBar);
        TargetDamageCounter = 0;
        SpeedIntensify = 0;
        AffectHealerCount = 0;

        SlowRate = 0;
        StunTime = 0;
        Buffable.RemoveAllBuffs();

    }
}

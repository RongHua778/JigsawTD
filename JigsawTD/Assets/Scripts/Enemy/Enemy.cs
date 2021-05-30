using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameBehavior
{
    public abstract EnemyType EnemyType { get; }
    private Animator anim;
    [SerializeField] GameObject exlposionPrefab = default;
    public int ReachDamage { get; set; }
    public float TargetDamageCounter { get; set; }
    public int TileStunCounter { get; set; }
    public float StunTime { get; set; }
    Direction direction;
    public Direction Direction { get => direction; set => direction = value; }
    DirectionChange directionChange;
    public virtual DirectionChange DirectionChange { get => directionChange; set => directionChange = value; }
    [SerializeField] Transform model = default;
    [HideInInspector] public GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress, progressFactor, adjust;
    float directionAngleFrom, directionAngleTo;
    float pathOffset;

    [Header("EnemyAttribute")]
    protected float speed;
    public virtual float Speed { get => StunTime > 0 ? 0 : speed * (1 - (SlowRate - PathSlow) / (SlowRate + PathSlow + 0.5f)); set => speed = value; }
    int shell;
    public int Shell { get => Mathf.Max(0, shell - BrokeShell); set => shell = value; }
    float slowRate;
    public float SlowRate
    {
        get => slowRate;
        //slowRate;// Mathf.Min(0.8f, (PathSlow + slowRate) / (PathSlow + slowRate + 1));
        set
        {
            slowRate = value;
            progressFactor = Speed * adjust;//子弹减速即时更新速度
            healthBar.ShowSlowIcon(slowRate > 0);
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
    }

    public override bool GameUpdate()
    {
        if (IsDie)
        {
            StopAllCoroutines();
            GameObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            explosion.transform.position = model.transform.position;
            GameEvents.Instance.EnemyDie(this);
            ObjectPool.Instance.UnSpawn(this.gameObject);
            return false;
        }
        if (StunTime >= 0)
        {
            StunTime -= Time.deltaTime;
            if (StunTime < 0)
                progressFactor = Speed * adjust;
        }
        progress += Time.deltaTime * progressFactor;
        while (progress >= 1f)
        {
            if (tileTo == null)
            {
                StartCoroutine(ExitCor());
                return false;
            }
            progress = (progress - 1f) / progressFactor;
            PrepareNextState();
            progress *= progressFactor;
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


    private IEnumerator ExitCor()
    {
        anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this.gameObject);
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


    public void SpawnOn(GameTile tile)
    {
        Debug.Assert(tile.NextTileOnPath != null, "No where to go", this);
        tileFrom = tile;
        tileTo = tileFrom.NextTileOnPath;
        progress = 0f;
        PrepareIntro();
    }

    private void PrepareIntro()
    {
        anim.Play("Default");
        anim.SetTrigger("Enter");

        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        Direction = tileFrom.PathDirection;
        DirectionChange = DirectionChange.None;
        model.localPosition = new Vector3(pathOffset, 0);
        directionAngleFrom = directionAngleTo = Direction.GetAngle();
        transform.localRotation = tileFrom.PathDirection.GetRotation();
        adjust = 2f;
        progressFactor = adjust * Speed;
    }

    private void PrepareOutro()
    {
        //anim.SetTrigger("Exit");

        positionTo = tileFrom.transform.localPosition;
        DirectionChange = DirectionChange.None;
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localRotation = Direction.GetRotation();
        adjust = 2f;
        progressFactor = adjust * Speed;
    }

    private void PrepareNextState()
    {
        tileFrom = tileTo;
        tileTo = tileTo.NextTileOnPath;
        positionFrom = positionTo;
        if (tileTo == null)
        {
            PrepareOutro();
            return;
        }
        positionTo = tileFrom.ExitPoint;
        DirectionChange = Direction.GetDirectionChangeTo(tileFrom.PathDirection);
        Direction = tileFrom.PathDirection;
        directionAngleFrom = directionAngleTo;

        TileStunCounter++;
        Buffable.TileTick();//先移除BUFF再加BUFF//放在Prepare前面，因为要提前改变Path速度
        tileFrom.OnTilePass(this);

        switch (DirectionChange)
        {
            case DirectionChange.None:
                PrepareForward();
                break;
            case DirectionChange.TurnRight:
                PrepareTurnRight();
                break;
            case DirectionChange.TurnLeft:
                PrepareTurnLeft();
                break;
            case DirectionChange.TurnAround:
                PrepareTurnAround();
                break;
        }


    }

    void PrepareForward()
    {
        transform.localRotation = Direction.GetRotation();
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        adjust = 1f;
        progressFactor = adjust * Speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        adjust = 1 / (Mathf.PI * 0.5f * (0.5f - pathOffset));
        progressFactor = adjust * Speed;
    }
    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        adjust = 1 / (Mathf.PI * 0.5f * (0.5f + pathOffset));
        progressFactor = adjust * Speed;
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localPosition = positionFrom;
        adjust = 1 / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
        progressFactor = adjust * Speed;
    }

    public virtual void ApplyDamage(float amount, out float realDamage,bool isCritical=false)
    {
        realDamage = amount * 5 / (5 + Shell);
        if (isCritical)
        {
            healthBar.ShowJumpDamage((int)realDamage);
        }
        CurrentHealth -= realDamage;
        TargetDamageCounter += realDamage;
    }


    public override void OnSpawn()
    {
        model.localPosition = Vector3.zero;
    }

    public override void OnUnSpawn()
    {
        ObjectPool.Instance.UnSpawn(healthBar.gameObject);
        IsDie = false;
        TargetDamageCounter = 0;
        TileStunCounter = 0;
        PathSlow = 0;
        SlowRate = 0;
        BrokeShell = 0;
        StunTime = 0;
        Buffable.RemoveAllBuffs();
    }
}

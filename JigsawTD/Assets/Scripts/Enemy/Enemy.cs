using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameBehavior
{
    public EnemyAttribute m_Attribute = default;
    Direction direction;
    DirectionChange directionChange;
    [SerializeField] Transform model = default;
    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress, progressFactor;
    float directionAngleFrom, directionAngleTo;
    float pathOffset;

    [Header("EnemyAttribute")]
    float speed;
    public float Speed { get => speed * (1 - SlowRate); set => speed = value; }
    int shell;
    public int Shell { get => Mathf.Max(0, shell - BrokeShell); set => shell = value; }
    float slowRate;
    public float SlowRate { get => slowRate; set => slowRate = value; }
    int brokeShell;
    public int BrokeShell { get => brokeShell; set => brokeShell = value; }


    public BuffableEntity Buffable { get; private set; }


    EnemyFactory originFacoty;
    public EnemyFactory OriginFactory
    {
        get => originFacoty;
        set
        {
            originFacoty = value;
        }
    }

    [Header("HealthSetting")]
    HealthBar healthBar;
    private bool isDie = false;
    public bool IsDie { get => isDie; set => isDie = value; }
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
    public override bool GameUpdate()
    {
        if (IsDie)
        {
            OriginFactory.Reclaim(this);
            return false;
        }
        progress += Time.deltaTime * progressFactor;
        while (progress >= 1f)
        {
            if (tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            progress = (progress - 1f) / progressFactor;
            PrepareNextState();
            progress *= progressFactor;
        }
        if (directionChange == DirectionChange.None)
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

    public void Initialize(float pathOffset, HealthBar healthBar)
    {
        this.pathOffset = pathOffset;
        this.healthBar = healthBar;
        this.healthBar.followTrans = model;
        Buffable = this.GetComponent<BuffableEntity>();
        MaxHealth = CurrentHealth = m_Attribute.Health;
        Speed = m_Attribute.Speed;
        Shell = m_Attribute.Shell;

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
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        direction = tileFrom.PathDirection;
        directionChange = DirectionChange.None;
        model.localPosition = new Vector3(pathOffset, 0);
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        transform.localRotation = tileFrom.PathDirection.GetRotation();
        progressFactor = 2f * Speed;
    }

    private void PrepareOutro()
    {
        positionTo = tileFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * Speed;
    }

    private void PrepareNextState()
    {

        //执行TILETO的TILEPASS效果
        tileTo.OnTilePass(this);
        Buffable.Tick();

        tileFrom = tileTo;
        tileTo = tileTo.NextTileOnPath;
        positionFrom = positionTo;
        if (tileTo == null)
        {
            PrepareOutro();
            return;
        }
        positionTo = tileFrom.ExitPoint;
        directionChange = direction.GetDirectionChangeTo(tileFrom.PathDirection);
        direction = tileFrom.PathDirection;
        directionAngleFrom = directionAngleTo;
        switch (directionChange)
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
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        progressFactor = Speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = Speed / (Mathf.PI * 0.5f * (0.5f - pathOffset));
    }
    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = Speed / (Mathf.PI * 0.5f * (0.5f + pathOffset));
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localPosition = positionFrom;
        progressFactor = Speed / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
    }

    public void ApplyDamage(float amount)
    {
        float damage = amount * 5 / (5 + Shell);
        CurrentHealth -= damage;
    }



    public override void OnSpawn()
    {
        IsDie = false;
    }

    public override void OnUnSpawn()
    {
        model.localPosition = Vector3.zero;
        ObjectPool.Instance.UnSpawn(healthBar.gameObject);
        Buffable.Buffs.Clear();

    }
}

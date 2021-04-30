using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ReusableObject
{
    Direction direction;
    DirectionChange directionChange;
    [SerializeField] Transform model = default;
    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress, progressFactor;
    float directionAngleFrom, directionAngleTo;
    float pathOffset;
    float speed = 2f;


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
    public bool GameUpdate()
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
        //test
        MaxHealth = 100;
        CurrentHealth = 100;
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
        progressFactor = 2f * speed;
    }

    private void PrepareOutro()
    {
        positionTo = tileFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
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
    //public bool GameUpdate()
    //{
    //    if (IsDie)
    //    {
    //        OriginFactory.Reclaim(this);
    //        return false;
    //    }
    //    progress += Time.deltaTime * progressFactor;
    //    while (progress >= 1f)
    //    {
    //        if (currentWayPointIndex >= m_Path.Count - 1)
    //        {
    //            OriginFactory.Reclaim(this);
    //            return false;
    //        }
    //        progress = (progress - 1f) / progressFactor;
    //        PrepareNextState();
    //        progress *= progressFactor;
    //    }
    //    if (directionChange == DirectionChange.None)
    //    {
    //        transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
    //    }
    //    else
    //    {
    //        float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
    //        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    //    }
    //    return true;
    //}

    //public void Initialize(float pathOffset, HealthBar healthBar, List<Vector3> path)
    //{
    //    this.m_Path = path;
    //    this.pathOffset = pathOffset;
    //    this.healthBar = healthBar;
    //    this.healthBar.followTrans = model;
    //    //test
    //    MaxHealth = 100;
    //    CurrentHealth = 100;
    //}

    //public void SpawnOn()
    //{
    //    progress = 0f;
    //    PrepareIntro();
    //}

    //private void PrepareIntro()
    //{
    //    currentWayPointIndex = 0;
    //    positionFrom = m_Path[currentWayPointIndex];
    //    positionTo = (positionFrom + m_Path[currentWayPointIndex + 1]) * 0.5f;
    //    Vector3 moveDirection = positionTo - positionFrom;
    //    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
    //    transform.localRotation = Quaternion.Euler(0, 0, angle);
    //    directionAngleFrom = directionAngleTo = angle;
    //    direction = DirectionExtensions.GetDirection(m_Path[currentWayPointIndex], positionTo);
    //    directionChange = DirectionChange.None;
    //    model.localPosition = new Vector3(pathOffset, 0);
    //    progressFactor = 2f * speed;
    //}

    //private void PrepareOutro()
    //{
    //    positionFrom = positionTo;
    //    positionTo = m_Path[currentWayPointIndex];
    //    directionChange = DirectionChange.None;
    //    directionAngleTo = direction.GetAngle();
    //    model.localPosition = new Vector3(pathOffset, 0);
    //    transform.localRotation = direction.GetRotation();
    //    progressFactor = 2f * speed;
    //}

    //private void PrepareNextState()
    //{
    //    currentWayPointIndex++;
    //    if (currentWayPointIndex >= m_Path.Count-1)
    //    {
    //        PrepareOutro();
    //        return;
    //    }
    //    positionFrom = positionTo;
    //    positionTo = (m_Path[currentWayPointIndex] + m_Path[currentWayPointIndex + 1]) * 0.5f;
    //    Direction newDirection = DirectionExtensions.GetDirection(m_Path[currentWayPointIndex], positionTo);
    //    directionChange = direction.GetDirectionChangeTo(newDirection);
    //    direction = newDirection;
    //    directionAngleFrom = directionAngleTo;
    //    switch (directionChange)
    //    {
    //        case DirectionChange.None:
    //            PrepareForward();
    //            break;
    //        case DirectionChange.TurnRight:
    //            PrepareTurnRight();
    //            break;
    //        case DirectionChange.TurnLeft:
    //            PrepareTurnLeft();
    //            break;
    //        case DirectionChange.TurnAround:
    //            PrepareTurnAround();
    //            break;
    //    }
    //}

    void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        progressFactor = speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.5f * (0.5f - pathOffset));
    }
    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.5f * (0.5f + pathOffset));
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localPosition = positionFrom;
        progressFactor = speed / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
    }



    public override void OnSpawn()
    {
        IsDie = false;
    }

    public override void OnUnSpawn()
    {
        model.localPosition = Vector3.zero;
        ObjectPool.Instance.UnSpawn(healthBar.gameObject);

    }
}

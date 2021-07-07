using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination
{
    boss,target,Random
}

public abstract class Aircraft : ReusableObject, IDamageable, IGameBehavior
{
    [SerializeField] ParticalControl explosionPrefab = default;
    [SerializeField] JumpDamage jumpDamagePrefab = default;

    public AircraftCarrier boss;
    public TurretContent targetTurret;

    protected bool isLeader;
    protected Aircraft predecessor;
    
    public Transform tail;

    Quaternion look_Rotation;
    protected float exploreRange = 10f;
    protected Collider2D[] attachedResult = new Collider2D[20];
    public readonly float minDistanceToLure = .1f;
    public readonly float minDistanceToDealDamage = 0.75f;
    readonly float maxDistanceToReturnToBoss = 5f;
    float movingSpeed=3.5f;
    float rotatingSpeed = 2f;

    protected Vector3 movingDirection;

    protected AudioClip explosionClip;
    public bool IsEnemy { get => false; }

    float maxHealth = 10;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (currentHealth <= 0&&!IsDie)
            {
                ReusableObject explosion = ObjectPool.Instance.Spawn(explosionPrefab);
                Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
                explosion.transform.position = transform.position;
                IsDie = true;
                Reclaim();
            }
        }
    }
    private bool isDie;
    public bool IsDie { get => isDie; set => isDie = value; }
    public BuffableEntity Buffable { get; set; }
    public TrapContent CurrentTrap { get; set; }
    protected FSMSystem fsm;


    public virtual void Initiate(AircraftCarrier boss)
    {
        IsDie = false;
        MaxHealth = boss.Armor;
        CurrentHealth = MaxHealth;
        boss.AddAircraft(this);
    }

    void Awake()
    {
        tail = GetComponentInChildren<Tail>().transform;
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
    }

    public virtual bool GameUpdate()
    {
        return true;
    }
    public void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount;
        CurrentHealth -= realDamage;
        GameEndUI.TotalDamage += (int)realDamage;

        if (isCritical)
        {
            JumpDamage obj = ObjectPool.Instance.Spawn(jumpDamagePrefab) as JumpDamage;
            obj.Jump((int)realDamage, transform.position);
        }

    }
    public void PickRandomDes()
    {
        float randomX = Random.Range(boss.transform.position.x - maxDistanceToReturnToBoss,
            boss.transform.position.x + maxDistanceToReturnToBoss);
        float randomY = Random.Range(boss.transform.position.y - maxDistanceToReturnToBoss,
            boss.transform.position.y + maxDistanceToReturnToBoss);
        movingDirection = new Vector3(randomX,randomY) - transform.position;
    }
    public void MovingToTarget(Destination des)
    {
        switch (des)
        {
            case Destination.boss:
                movingDirection = boss.transform.position - transform.position;
                break;
            case Destination.target:
                movingDirection = targetTurret.transform.position - transform.position;
                break;
            case Destination.Random:
                break;
            default:
                Debug.LogAssertion("飞行目的地错误！");
                break;
        }

        transform.Translate(Vector3.up * Time.deltaTime * movingSpeed);
        RotateTowards();
    }

    private void RotateTowards()
    {
        var angle = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, look_Rotation,
            rotatingSpeed * Time.deltaTime);
    }
    public void SetLeader()
    {
        isLeader = true;
    }

    public void SetPredecessor(Aircraft predecessor)
    {
        this.predecessor = predecessor;
    }
    public virtual void Reclaim()
    {
        ObjectPool.Instance.UnSpawn(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination
{
    boss,target,Random
}

public class Aircraft : ReusableObject, IDamageable, IGameBehavior
{
    [SerializeField] ParticalControl explosionPrefab = default;
    [SerializeField] ParticalControl attackPrefab = default;
    [SerializeField] JumpDamage jumpDamagePrefab = default;
    public FrostEffect frostPrefab = default;

    public AircraftCarrier boss;
    public TurretContent targetTurret;

    Quaternion look_Rotation;
    public float freezeTime = 5f;
    float exploreRange = 10f;
    Collider2D[] attachedResult = new Collider2D[20];
    public readonly float minDistanceToLure = .1f;
    public readonly float minDistanceToDealDamage = 0.75f;
    readonly float maxDistanceToReturnToBoss = 5f;
    float movingSpeed=3.5f;
    float rotatingSpeed = 2f;

    Vector3 movingDirection;

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
    private FSMSystem fsm;

    public void Initiate(AircraftCarrier boss)
    {
        IsDie = false;
        MaxHealth = boss.Armor;
        CurrentHealth = MaxHealth;
        boss.AddAircraft(this);
        if (fsm == null)
        {
            this.boss = boss;
            //以下是状态机的初始化
            fsm = new FSMSystem();

            FSMState patrolState = new PatrolState(fsm);
            patrolState.AddTransition(Transition.AttackTarget, StateID.Track);
            patrolState.AddTransition(Transition.LureTarget, StateID.Lure);
            PickRandomDes();

            FSMState trackState = new TrackState(fsm);
            trackState.AddTransition(Transition.Attacked, StateID.Back);

            FSMState lureState = new LureState(fsm);
            lureState.AddTransition(Transition.Attacked, StateID.Back);

            FSMState backState = new BackState(fsm);
            backState.AddTransition(Transition.BackToBoss, StateID.Patrol);

            fsm.AddState(patrolState);
            fsm.AddState(trackState);
            fsm.AddState(backState);
            fsm.AddState(lureState);
            GameManager.Instance.nonEnemies.Add(this);
        }
    }

    private void Awake()
    {
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
    }

    public virtual bool GameUpdate()
    {
        fsm.Update(this);
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

    public void Lure()
    {
        float distanceToTarget = ((Vector2)transform.position - (Vector2)targetTurret.transform.position).magnitude;
        if (distanceToTarget < minDistanceToLure)
        {
            movingDirection = targetTurret.transform.position - transform.position + new Vector3(0.5f, 0.5f);
            MovingToTarget(Destination.Random);
        }
        else
        {
            movingDirection = targetTurret.transform.position - transform.position;
            MovingToTarget(Destination.Random);
        }
    }

    private void RotateTowards()
    {
        var angle = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, look_Rotation,
            rotatingSpeed * Time.deltaTime);
    }
    public void SearchTarget()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position,
     exploreRange, attachedResult, LayerMask.GetMask(StaticData.TurretMask));
        if (hits > 0)
        {
            List<TurretContent> turrets = new List<TurretContent>();
            for (int i = 0; i < hits; i++)
            {
                if (attachedResult[i].GetComponent<TurretContent>().Activated)
                {
                    turrets.Add(attachedResult[i].GetComponent<TurretContent>());
                }
            }
            if (turrets.Count > 0)
            {
                int temp = Random.Range(0, turrets.Count);
                targetTurret = turrets[temp];
            }
        }
    }

    public void Attack()
    {
        FrostEffect frosteffect = ObjectPool.Instance.Spawn(frostPrefab) as FrostEffect;
        frosteffect.transform.position = targetTurret.transform.position;
        frosteffect.UnspawnAfterTime(freezeTime);
        targetTurret.Frost(freezeTime);
        ReusableObject explosion = ObjectPool.Instance.Spawn(explosionPrefab);
        Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
        explosion.transform.position = targetTurret.transform.position;
        targetTurret = null;
    }
    public void Reclaim()
    {
        ObjectPool.Instance.UnSpawn(this);
    }
}

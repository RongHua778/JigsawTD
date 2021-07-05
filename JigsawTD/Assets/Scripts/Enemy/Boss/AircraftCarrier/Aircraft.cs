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
    [SerializeField] JumpDamage jumpDamagePrefab = default;
    public FrostEffect frostPrefab = default;

    Enemy boss;
    public TurretContent targetTurret;

    Quaternion look_Rotation;
    public float freezeTime = 5f;
    float exploreRange = 10f;
    Collider2D[] attachedResult = new Collider2D[20];
    public readonly float minDistanceToDealDamage = .1f;
    readonly float maxDistanceToReturnToBoss = 5f;
    float movingSpeed=1f;
    float rotatingSpeed = 2f;

    Vector3 movingDirection;

    protected AudioClip explosionClip;

    float maxHealth=10;
    float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                ReusableObject explosion = ObjectPool.Instance.Spawn(explosionPrefab);
                Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
                explosion.transform.position = transform.position;
            }
        }
    }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    private FSMSystem fsm;

    public void Initiate(Enemy boss)
    {

        CurrentHealth = MaxHealth;
        this.boss = boss;

        //以下是状态机的初始化
        fsm = new FSMSystem();

        FSMState patrolState = new PatrolState(fsm);
        patrolState.AddTransition(Transition.WaitingEnough, StateID.Track);

        FSMState trackState = new TrackState(fsm);
        trackState.AddTransition(Transition.ReadyForAttack, StateID.Attack);

        FSMState attackState = new AttackState(fsm);

        fsm.AddState(patrolState);
        fsm.AddState(trackState);
        fsm.AddState(attackState);
        GameManager.Instance.nonEnemies.Add(this);

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
        float randomX = Random.Range(boss.transform.position.x,
            boss.transform.position.x + maxDistanceToReturnToBoss);
        float randomY = Random.Range(boss.transform.position.y,
            boss.transform.position.y + maxDistanceToReturnToBoss);
        movingDirection = new Vector3(randomX,randomY) - transform.position;
        transform.Translate(Vector3.up * Time.deltaTime * movingSpeed);
        RotateTowards();
        transform.Translate(Vector3.up * Time.deltaTime * movingSpeed);
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
    public void SearchTarget()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position,
     exploreRange, attachedResult, LayerMask.GetMask(StaticData.TurretMask));
        if (hits > 0)
        {
            int temp = Random.Range(0, hits);
            targetTurret = attachedResult[temp].GetComponent<TurretContent>();
            if (targetTurret != null)
            {
                fsm.PerformTransition(Transition.WaitingEnough);
            }
        }
    }
    public void Reclaim()
    {
        ObjectPool.Instance.UnSpawn(this);
    }
}

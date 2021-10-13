using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination
{
    boss, target, Random
}

public abstract class Aircraft : ReusableObject, IDamageable, IGameBehavior
{
    [SerializeField] public ParticalControl explosionPrefab = default;
    public DamageStrategy DamageStrategy { get; set; }

    public AircraftCarrier boss;
    public TurretContent targetTurret;

    public bool isFollowing = true;
    protected bool isLeader;
    protected Aircraft predecessor;
    Vector3[] followingPosition = new Vector3[2];


    Quaternion look_Rotation;
    protected float exploreRange = 10f;
    protected Collider2D[] attachedResult = new Collider2D[20];
    public readonly float minDistanceToLure = .1f;
    public readonly float minDistanceToDealDamage = 0.75f;
    readonly float maxDistanceToReturnToBoss = 5f;
    protected float movingSpeed = 3.5f;
    protected float rotatingSpeed = 2f;
    protected float originalMovingSpeed = 3.5f;
    protected float originalRotatingSpeed = 2f;

    protected Vector3 movingDirection;

    protected AudioClip explosionClip;
    //public bool IsEnemy { get => false; }
    //float damageIntensify;
    //public float DamageIntensify { get => damageIntensify; set => damageIntensify = value; }
    //float maxHealth = 10;
    //public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    //float currentHealth;

    //public float CurrentHealth
    //{
    //    get => currentHealth;
    //    set
    //    {
    //        currentHealth = value;
    //        if (currentHealth <= 0&&!IsDie)
    //        {
    //            ReusableObject explosion = ObjectPool.Instance.Spawn(explosionPrefab);
    //            Sound.Instance.PlayEffect(explosionClip);
    //            explosion.transform.position = transform.position;
    //            IsDie = true;
    //            Reclaim();
    //        }
    //    }
    //}
    private bool isDie;
    public bool IsDie { get => isDie; set => isDie = value; }

    protected FSMSystem fsm;


    public virtual void Initiate(AircraftCarrier boss)
    {
        IsDie = false;
        DamageStrategy.CurrentHealth = boss.Armor;
        //CurrentHealth = MaxHealth;
        boss.AddAircraft(this);
        boss.SetQueue();
    }

    void Awake()
    {
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
        DamageStrategy = new AircraftStrategy(this.transform, this);
    }


    public virtual bool GameUpdate()
    {
        if (predecessor)
        {
            followingPosition[1] = followingPosition[0];
            followingPosition[0] = predecessor.transform.position;
        }
        return true;
    }
    //public void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    //{
    //    realDamage = amount;
    //    CurrentHealth -= realDamage;
    //    GameRes.TotalDamage += (int)realDamage;

    //    if (isCritical)
    //    {
    //        StaticData.Instance.ShowJumpDamage(transform.position, (int)realDamage);
    //    }

    //}
    public void PickRandomDes()
    {
        float randomX = Random.Range(boss.transform.position.x - maxDistanceToReturnToBoss,
            boss.transform.position.x + maxDistanceToReturnToBoss);
        float randomY = Random.Range(boss.transform.position.y - maxDistanceToReturnToBoss,
            boss.transform.position.y + maxDistanceToReturnToBoss);
        movingDirection = new Vector3(randomX, randomY) - transform.position;
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

    public void Lure()
    {
        if (!isFollowing || isLeader)
        {
            movingSpeed = originalMovingSpeed;
            rotatingSpeed = originalRotatingSpeed;
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
        else
        {
            Follow();
        }

    }

    public virtual void Attack()
    {

    }

    public void ProtectMe()
    {
        fsm.PerformTransition(Transition.ProtectBoss);
        // isFollowing = true;
    }

    public void Protect()
    {
        if (isLeader || !isFollowing)
        {
            movingSpeed = originalMovingSpeed;
            rotatingSpeed = originalRotatingSpeed;
            float distanceToTarget = ((Vector2)transform.position - (Vector2)boss.transform.position).magnitude;
            if (distanceToTarget < minDistanceToLure)
            {
                movingDirection = boss.model.transform.position - transform.position + new Vector3(0.5f, 0.5f);
            }
            else
            {
                movingDirection = boss.model.transform.position - transform.position;
            }
            MovingToTarget(Destination.Random);
        }
        else
        {
            Follow();
        }

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

    public void Follow()
    {
        float distanceToP = ((Vector2)transform.position - (Vector2)predecessor.transform.position).magnitude;
        if (distanceToP > 1f)
        {
            movingSpeed = 8f + distanceToP * 3f;
            rotatingSpeed = 5f + distanceToP;
        }
        else
        {
            movingSpeed = 3f;
            rotatingSpeed = 2f;
        }
        movingDirection = followingPosition[1] - transform.position;
        MovingToTarget(Destination.Random);
    }

    public virtual void Reclaim()
    {
        boss.aircraftQueue.Remove(this);
        boss.SetQueue();
        ObjectPool.Instance.UnSpawn(this);
    }

    //public void ApplyBuff(EnemyBuffName buffName, float keyValue, float duration)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void ApplyDamage(float amount, out float realDamag)
    //{
    //    throw new System.NotImplementedException();
    //}
}

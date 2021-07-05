using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft : ReusableObject, IDamageable, IGameBehavior
{
    [SerializeField] ParticalControl explosionPrefab = default;
    [SerializeField] JumpDamage jumpDamagePrefab = default;
    public FrostEffect frostPrefab = default;

    public Quaternion look_Rotation;
    public TurretContent targetTurret;
    public float freezeTime = 5f;
    public float exploreRange = 10f;
    public Collider2D[] attachedResult = new Collider2D[20];
    public readonly float minDistanceToDealDamage = .1f;
    public float movingSpeed=20f;

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

        //以下是状态机的初始化
        fsm = new FSMSystem();

        FSMState patrolState = new PatrolState(fsm,boss);
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

    public void Reclaim()
    {
        ObjectPool.Instance.UnSpawn(this);
    }
}

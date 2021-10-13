using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Enemy
{
    [SerializeField] protected float armorIntensify;
    float armor;
    List<Aircraft> aircrafts = new List<Aircraft>();
    public List<Aircraft> aircraftQueue = new List<Aircraft>();
    //float setQueueCD = 3f;
    //float setQueueCounter;

    bool protect = false;
    float bornCounter;
    float bornCD;
    int enemyOneBorn;
    int enemyNumber;
    int maxEnemyNumber;
    [SerializeField] AircraftType aircraftType;

    public float Armor { get => armor; set => armor = value; }

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify, path);
        Armor = DamageStrategy.MaxHealth * armorIntensify;
        bornCD = 4;
        enemyOneBorn = 3;
        maxEnemyNumber = 10;
        //Skills = new List<Skill>();
        //Skills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Aircraft, this));
    }

    //public override bool GameUpdate()
    //{
    //    setQueueCounter += Time.deltaTime;
    //    if (setQueueCounter > setQueueCD)
    //    {
    //        setQueueCD = 0;
    //        SetQueue();
    //    }
    //    return base.GameUpdate();
    //}

    protected override void OnEnemyUpdate()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            if (enemyNumber <= maxEnemyNumber)
                Born();
            bornCounter = 0;
        }
        if (!protect && DamageStrategy.CurrentHealth < DamageStrategy.MaxHealth * 0.5f)
        {
            protect = true;
            foreach (var aircraft in aircrafts)
            {
                aircraft.ProtectMe();
            }
        }
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn; i++)
        {
            switch (aircraftType)
            {
                case AircraftType.Normal:
                    AirAttacker aircraft = StaticData.Instance.NonEnemyFactory.GetAirAttacker();
                    aircraft.transform.localPosition = this.transform.localPosition;
                    aircraft.Initiate(this);
                    break;
                case AircraftType.Stronger:
                    AirProtector strongerAircraft = StaticData.Instance.NonEnemyFactory.GetAirProtector();
                    strongerAircraft.transform.localPosition = this.transform.localPosition;
                    strongerAircraft.Initiate(this);
                    break;
                default:
                    Debug.LogAssertion("·ÉÐÐÆ÷ÀàÐÍ´íÎó£¡");
                    break;
            }

            enemyNumber += 1;
        }
    }

    public virtual void AddAircraft(Aircraft a)
    {
        aircrafts.Add(a);
        if (!aircraftQueue.Contains(a))
        {
            aircraftQueue.Add(a);
        }
    }

    public void SetQueue()
    {
        for (int i = 0; i < aircraftQueue.Count; i++)
        {
            if (i == 0)
            {
                aircraftQueue[i].SetLeader();
            }
            else
            {
                aircraftQueue[i].SetPredecessor(aircraftQueue[i - 1]);
            }
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        enemyNumber = 0;
        protect = false;
        for (int i = 0; i < aircrafts.Count; i++)
        {
            aircrafts[i].DamageStrategy.CurrentHealth = 0;
        }
        aircrafts.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Enemy
{
    [SerializeField] float aircarftIntensify;
    [SerializeField] Aircraft airCraftPrefab = default;
    List<Aircraft> aircrafts = new List<Aircraft>();

    bool protect = false;
    float bornCounter;
    float bornCD;
    int enemyOneBorn;
    int enemyNumber;
    int maxEnemyNumber;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify, path);
        bornCD = 4;
        enemyOneBorn = 4;
        maxEnemyNumber = 12;
    }


    protected override void OnEnemyUpdate()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            if (enemyNumber <= maxEnemyNumber)
                Born();
            bornCounter = 0;
        }
        //if (!protect && DamageStrategy.CurrentHealth < DamageStrategy.MaxHealth * 0.5f)
        //{
        //    protect = true;
        //    Born();
        //    foreach (var aircraft in aircrafts)
        //    {
        //        aircraft.ProtectMe();
        //    }
        //}
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn; i++)
        {
            AirAttacker aircraft = ObjectPool.Instance.Spawn(airCraftPrefab) as AirAttacker;
            aircraft.transform.localPosition = this.transform.localPosition;
            aircraft.Initiate(this, DamageStrategy.MaxHealth * aircarftIntensify);
            enemyNumber += 1;
        }
    }

    public void AddAircraft(Aircraft a)
    {
        aircrafts.Add(a);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Enemy
{
    public override string ExplosionEffect => "BossExplosionBlue";

    [SerializeField] float aircarftIntensify;
    [SerializeField] Aircraft airCraftPrefab = default;
    List<Aircraft> aircrafts = new List<Aircraft>();

    float bornCounter;
    float bornCD;
    int enemyOneBorn;
    int enemyNumber;
    int maxEnemyNumber;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset,float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        bornCD = 3;
        enemyOneBorn = 4;
        maxEnemyNumber = 16;
    }


    protected override void OnEnemyUpdate()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            if (enemyNumber < maxEnemyNumber)
                StartCoroutine(BornCor());
            bornCounter = 0;
        }
    }

    IEnumerator BornCor()
    {
        anim.SetBool("Transform", true);
        Sound.Instance.PlayEffect("Sound_BornerTransform");

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < enemyOneBorn; i++)
        {
            AirAttacker aircraft = ObjectPool.Instance.Spawn(airCraftPrefab) as AirAttacker;
            aircraft.transform.position = this.model.position;
            aircraft.Initiate(this, DamageStrategy.MaxHealth * aircarftIntensify);
            enemyNumber += 1;
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Transform", false);
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
        for (int i = 0; i < aircrafts.Count; i++)
        {
            aircrafts[i].DamageStrategy.CurrentHealth = 0;
        }
        aircrafts.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Boss
{
    public override string ExplosionEffect => "BossExplosionBlue";

    [SerializeField] float aircarftIntensify;
    [SerializeField] Aircraft airCraftPrefab = default;
    List<Aircraft> aircrafts = new List<Aircraft>();

    float bornCounter;
    float bornCD;
    int maxEnemyNumber;
    private bool secondBorn;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        bornCD = 2;
        maxEnemyNumber = 12;
        secondBorn = false;
    }


    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        if (bornCounter < bornCD)
        {
            bornCounter += Time.deltaTime;
            if (bornCounter > bornCD)
            {
                StartCoroutine(BornCor());  //开局3秒后第一次诞生小飞机
            }
        }
        //if (!secondBorn && DamageStrategy.CurrentHealth < DamageStrategy.MaxHealth * 0.5f)
        //{
        //    secondBorn = true;
        //    StopAllCoroutines();
        //    StartCoroutine(BornCor());  //开局后生命值低于50%第二次诞生小飞机
        //}
    }

    IEnumerator BornCor()
    {
        anim.SetBool("Transform", true);
        Sound.Instance.PlayEffect("Sound_BornerTransform");

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < maxEnemyNumber; i++)
        {
            AirAttacker aircraft = ObjectPool.Instance.Spawn(airCraftPrefab) as AirAttacker;
            aircraft.transform.position = this.model.position;
            aircraft.Initiate(this, DamageStrategy.MaxHealth * aircarftIntensify);
            Sound.Instance.PlayEffect("Sound_Aircraft");
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Transform", false);
    }



    public void AddAircraft(Aircraft a)
    {
        aircrafts.Add(a);
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        bornCounter = 0;
        for (int i = 0; i < aircrafts.Count; i++)
        {
            aircrafts[i].DamageStrategy.CurrentHealth = 0;
        }
        aircrafts.Clear();
    }
}

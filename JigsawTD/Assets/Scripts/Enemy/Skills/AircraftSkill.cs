using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftSkill : Skill
{
    float bornCounter;
    float bornCD;
    int enemyOneBorn;
    int enemyNumber;
    int maxEnemyNumber;
    AircraftCarrier boss;

    public AircraftSkill(AircraftCarrier boss, float bornCD, int enemyOneBorn, int maxEnemyNumber)
    {
        this.boss = boss;
        this.bornCD = bornCD;
        this.enemyOneBorn = enemyOneBorn;
        this.maxEnemyNumber = maxEnemyNumber;
    }


    //在enemy的gameupdate中调用
    public override void OnGameUpdating()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            if(enemyNumber<=maxEnemyNumber)
            Born();
            bornCounter = 0;
        }
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn; i++)
        {
            Aircraft aircraft=GameManager.Instance.NonEnemyFactory.GetAircraft();
            aircraft.transform.localPosition = boss.transform.localPosition;
            aircraft.Initiate(boss);
            enemyNumber+=1;
        }
    }


}

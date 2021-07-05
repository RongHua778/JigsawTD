using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftSkill : Skill
{
    float bornCounter;
    float bornCD;
    int enemyOneBorn;

    public AircraftSkill(Enemy enemy, float bornCD, int enemyOneBorn)
    {
        this.enemy = enemy;
        this.bornCD = bornCD;
        this.enemyOneBorn = enemyOneBorn;

    }


    //在enemy的gameupdate中调用
    public override void OnGameUpdating()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            Born();
            bornCounter = 0;
        }
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn; i++)
        {
            Aircraft aircraft=GameManager.Instance.NonEnemyFactory.GetAircraft();
            aircraft.transform.localPosition = enemy.transform.localPosition;
            aircraft.Initiate(enemy);
        }
    }


}

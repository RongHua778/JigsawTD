using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AircraftType
{
    Normal, Stronger
}

public class AircraftSkill : Skill
{
    float bornCounter;
    float bornCD;
    int enemyOneBorn;
    int enemyNumber;
    int maxEnemyNumber;
    AircraftCarrier boss;
    AircraftType aircraftType;

    public AircraftSkill(AircraftType aircraftType,AircraftCarrier boss, float bornCD, int enemyOneBorn, int maxEnemyNumber)
    {
        this.boss = boss;
        this.bornCD = bornCD;
        this.enemyOneBorn = enemyOneBorn;
        this.maxEnemyNumber = maxEnemyNumber;
        this.aircraftType = aircraftType;
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
            switch (aircraftType)
            {
                case AircraftType.Normal:
                    AirAttacker aircraft=GameManager.Instance.NonEnemyFactory.GetAirAttacker();
                    aircraft.transform.localPosition = boss.transform.localPosition;
                    aircraft.Initiate(boss);
                    break;
                case AircraftType.Stronger:
                    AirProtector strongerAircraft = GameManager.Instance.NonEnemyFactory.GetAirProtector();
                    strongerAircraft.transform.localPosition = boss.transform.localPosition;
                    strongerAircraft.Initiate(boss);
                    break;
                default:
                    Debug.LogAssertion("飞行器类型错误！");
                    break;
            }

            enemyNumber+=1;
        }
    }


}

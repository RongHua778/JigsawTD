using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EnemySkill
{
    Born, Blink, Divide, Aircraft, strongerAircraft
}
[CreateAssetMenu(menuName = "Factory/SkillFactory", fileName = "skillFactory")]
public class SkillFactory : GameObjectFactory
{

    [SerializeField] float aircraftBornCD = 4f;
    [SerializeField] int aircraftOneBorn = 1;
    [SerializeField] int maxAircrafts = 1;
    [SerializeField] float strongerAircraftBornCD = 4f;
    [SerializeField] int strongerAircraftOneBorn = 1;


    public Skill GetSkill(EnemySkill skill, Enemy enemy)
    {
        switch (skill)
        {
            //case EnemySkill.Blink:
            //    return new BlinkSkill(enemy, blinkerBlink, holePrefab);
            //case EnemySkill.Divide:
            //    return new DivideSkill(enemy, dividerDividing, dividerSprings, 1, dividerSprites);
            default:
                Debug.LogAssertion("���ܲ�������");
                return null;
        }
    }


    public Skill GetSkill(EnemySkill skill, AircraftCarrier enemy)
    {
        return new AircraftSkill(AircraftType.Normal, enemy, aircraftBornCD, aircraftOneBorn, maxAircrafts);
    }

    public Skill GetSkill(EnemySkill skill, StrongerAircraftCarrier enemy)
    {
        return new AircraftSkill(AircraftType.Stronger, enemy, strongerAircraftBornCD, strongerAircraftOneBorn, maxAircrafts);
    }
}

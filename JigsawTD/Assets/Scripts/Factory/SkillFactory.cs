using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EnemySkill
{
    Born,Blink,Divide,Aircraft
}
[CreateAssetMenu(menuName = "Factory/SkillFactory", fileName = "skillFactory")]
public class SkillFactory:GameObjectFactory 
{
    [SerializeField]float bornerBornCD = 4.5f;
    [SerializeField]int bornerEnemyOneBorn = 3;
    [SerializeField]int blinkerBlink = 3;
    [SerializeField]ReusableObject holePrefab = default;
    [SerializeField]int dividerDividing;
    [SerializeField]int dividerSprings;
    [SerializeField] Sprite[] dividerSprites = default;
    [SerializeField] float aircraftBornCD = 4f;
    [SerializeField] int aircraftOneBorn = 1;
    [SerializeField] int maxAircrafts = 1;
    public Skill GetSkill(EnemySkill skill,Enemy enemy)
    {
        switch (skill)
        {
            case EnemySkill.Born:
                return new BornSkill(enemy, bornerBornCD, bornerEnemyOneBorn);
            case EnemySkill.Blink:
                return new BlinkSkill(enemy, blinkerBlink,holePrefab);
            case EnemySkill.Divide:
                return new DivideSkill(enemy, dividerDividing, dividerSprings,1,dividerSprites);
            default:
                Debug.LogAssertion("技能参数错误！");
                return null;
        }
    }

    public Skill GetSkill(EnemySkill skill, AircraftCarrier enemy)
    {
        return new AircraftSkill(enemy, aircraftBornCD, aircraftOneBorn, maxAircrafts);
    }
}

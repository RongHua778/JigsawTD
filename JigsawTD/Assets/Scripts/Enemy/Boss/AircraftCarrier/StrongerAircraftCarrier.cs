using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongerAircraftCarrier : AircraftCarrier
{
    public override EnemyType EnemyType => EnemyType.StrongerAircraftCarrier;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        Armor = intensify * armorIntensify;
        EnemySkills = new List<Skill>();
        EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.strongerAircraft, this));
    }
}

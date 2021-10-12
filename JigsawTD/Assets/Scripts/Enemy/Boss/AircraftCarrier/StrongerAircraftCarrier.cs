using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongerAircraftCarrier : AircraftCarrier
{

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify,path);
        Armor = intensify * armorIntensify;
        //Skills = new List<Skill>();
        //Skills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.strongerAircraft, this));
    }
}

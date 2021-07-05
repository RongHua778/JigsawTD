using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Enemy
{
    public override EnemyType EnemyType => EnemyType.AircraftCarrier;

    public override void OnSpawn()
    {
        base.OnSpawn();
        if (EnemySkills == null)
        {
            EnemySkills = new List<Skill>();
            EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Aircraft, this));
        }
    }
}

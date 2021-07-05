using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner : Enemy
{
    public override EnemyType EnemyType => EnemyType.Restorer;

    public override void OnSpawn()
    {
        base.OnSpawn();
        EnemySkills = new List<Skill>();
        EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Born, this));
    }

}

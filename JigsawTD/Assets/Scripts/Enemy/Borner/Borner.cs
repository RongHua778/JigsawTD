using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner : Enemy
{
    float bornCounter;
    public override EnemyType EnemyType => EnemyType.Restorer;

    public override void OnSpawn()
    {
        base.OnSpawn();
        if (EnemySkills == null)
        {
            EnemySkills = new List<Skill>();
            EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Born,this));
        }
    }

}

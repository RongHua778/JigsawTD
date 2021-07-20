using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner : Enemy
{
    public int form=2;
    public override EnemyType EnemyType => EnemyType.Borner;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        EnemySkills = new List<Skill>();
        EnemySkills.Add(GameManager.Instance.SkillFactory.GetBornSkill(this, 2));
    }

}

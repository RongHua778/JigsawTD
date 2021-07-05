using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinker : Enemy
{

    public override EnemyType EnemyType => EnemyType.Blinker;

    public override void OnSpawn()
    {
        base.OnSpawn();
        if (EnemySkills == null)
        {
            EnemySkills = new List<Skill>();
            EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Blink, this));
        }
    }

}

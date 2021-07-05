using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Enemy
{
    [SerializeField] public SpriteRenderer EnemySprite = default;
    [SerializeField] Sprite originalSprite = default;

    public override void OnSpawn()
    {
        base.OnSpawn();
        EnemySprite.sprite = originalSprite;
        EnemySkills = new List<Skill>();
        EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Divide, this));       
    }

    public override EnemyType EnemyType => EnemyType.Divider;
}

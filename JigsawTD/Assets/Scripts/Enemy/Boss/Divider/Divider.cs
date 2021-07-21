using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Enemy
{
    //[SerializeField] public SpriteRenderer EnemySprite = default;
    //[SerializeField] Sprite originalSprite = default;
    public int dividing;
    public override EnemyType EnemyType => EnemyType.Divider;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        //EnemySprite.sprite = originalSprite;
        EnemySkills = new List<Skill>();
        DivideSkill sk = GameManager.Instance.SkillFactory.GetDividerSkill(this, dividing) as DivideSkill;
        EnemySkills.Add(sk);
    }
}

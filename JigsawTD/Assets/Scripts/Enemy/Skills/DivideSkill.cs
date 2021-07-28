using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideSkill : Skill
{

    int dividing;
    int springs;
    Sprite[] dividerSprites;
    public DivideSkill(Enemy enemy, int dividing, int springs, Sprite[] divideSprites)
    {
        this.enemy = enemy;
        this.dividing = dividing;
        this.springs = springs;
        this.dividerSprites = divideSprites;
    }

    public override void OnDying()
    {
        GetSprings();
    }

    private void GetSprings()
    {
        if (dividing > 1)
        {
            for (int i = 0; i < springs; i++)
            {
                SpawnEnemy();
            }
        }
    }
    private void SpawnEnemy()
    {
        Divider divider = GameManager.Instance.SpawnEnemy(this.enemy.EnemyType, this.enemy.PointIndex, 1) as Divider;
        divider.dividing = dividing - 1;
        divider.EnemySkills.Clear();
        divider.EnemySkills.Add(GameManager.Instance.SkillFactory.GetDividerSkill(divider, divider.dividing));
        //divider.EnemySprite.sprite = dividerSprites[divider.dividing - 1];
        //divider.StartCoroutine(SetSprite(divider, dividerSprites[divider.dividing - 1]));
        //Debug.Log("Dividing=" + (divider.dividing - 1));
        //Debug.Log(dividerSprites[divider.dividing - 1].name);
        divider.Progress = Mathf.Clamp((this.enemy.Progress + Random.Range(-0.2f, 0.2f)), 0, 1);
        divider.MaxHealth = this.enemy.MaxHealth / 2;
        //divider.enemyCol.radius = 0.4f - 0.1f * (3 - divider.dividing);
        //enemy.dividing = dividing;
        //enemy.enemySprite.sprite = dividerSprites[enemy.dividing];
        //enemy.MaxHealth = this.enemy.MaxHealth / 2;
        //enemy.enemyCol.radius = 0.4f - 0.1f * (3 - dividing);

        //EnemyAttribute attribute = GameManager.Instance.EnemyFactory.Get(EnemyType.Divider);
        //float intensify = ws.RunningSequence.Intensify;
        //Divider enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Divider;
        //HealthBar healthBar = ObjectPool.Instance.Spawn(ws.HealthBarPrefab) as HealthBar;
        //DivideSkill ds = new DivideSkill(enemy, dividing - 1, springs, DividerIntensify, dividerSprites);
        //enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify);
        //enemy.EnemySkills = new List<Skill>();
        //enemy.EnemySkills.Add(ds);
        //enemy.EnemySprite.sprite = dividerSprites[dividing - 1];
        //enemy.MaxHealth = this.enemy.MaxHealth * DividerIntensify;
        //enemy.SpawnOn(this.enemy.PointIndex, board.shortestPoints);
        //GameManager.Instance.enemies.Add(enemy);
        //enemy.Progress = Mathf.Clamp((this.enemy.Progress + Random.Range(-0.2f, 0.2f)), 0, 1);


    }

}

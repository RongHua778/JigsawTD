using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideSkill : Skill
{
    WaveSystem ws;
    BoardSystem bs;
    int dividing;
    int springs;
    float dividerIntensify = 1;
    Sprite[] dividerSprites = default;
    public DivideSkill(Enemy enemy,int dividing, int springs,float dividerIntensify, Sprite[] dividerSprites)
    {
        this.enemy = enemy;
        this.dividing = dividing;
        this.springs = springs;
        this.dividerSprites = dividerSprites;
        this.dividerIntensify = dividerIntensify;
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }

    public float DividerIntensify { get => dividerIntensify; set => dividerIntensify = value; }

    public override void OnDying()
    {
       GetSprings();
    }

    private void GetSprings()
    {
        if (0 < dividing)
        {
            DividerIntensify *= 0.5f;
            for (int i = 0; i < springs; i++)
            {
                ws.EnemyRemain += 1;
                SpawnEnemy(bs);
            }
        }
    }
    private void SpawnEnemy(BoardSystem board)
    {
        EnemyAttribute attribute = GameManager.Instance.EnemyFactory.Get(EnemyType.Divider);
        float intensify = this.enemy.m_Intensify;
        Divider enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Divider;
        HealthBar healthBar = ObjectPool.Instance.Spawn(ws.HealthBarPrefab) as HealthBar;
        DivideSkill ds=new DivideSkill(enemy, dividing-1,springs,DividerIntensify,dividerSprites);
        enemy.EnemySkills = new List<Skill>();
        enemy.EnemySkills.Add(ds);
        enemy.EnemySprite.sprite = dividerSprites[dividing - 1];
        enemy.MaxHealth = this.enemy.MaxHealth * DividerIntensify;
        enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify);
        enemy.EnemySprite.GetComponent<CircleCollider2D>().radius = 0.4f - 0.1f * (3 - dividing);
        enemy.SpawnOn(this.enemy.PointIndex, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
        enemy.Progress = Mathf.Clamp((this.enemy.Progress + Random.Range(-0.2f, 0.2f)), 0, 1);
    }
}

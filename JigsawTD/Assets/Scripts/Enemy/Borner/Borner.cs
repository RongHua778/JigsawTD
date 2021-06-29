using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner : Enemy
{
    WaveSystem ws;
    BoardSystem bs;
    [SerializeField] float bornCD;
    [SerializeField] int enemyOneBorn;
    float bornCounter;
    public override EnemyType EnemyType => EnemyType.Restorer;
    public override void Awake()
    {
        base.Awake();
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }

    public override bool GameUpdate()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            Born();
            bornCounter = 0;
        }
        return base.GameUpdate();
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn; i++)
        {
            int typeInt = Random.Range(0, 6);
            SpawnEnemy(bs, (EnemyType)typeInt);
            ws.EnemyRemain++;
        }
    }
    private void SpawnEnemy(BoardSystem board, EnemyType type)
    {
        EnemyAttribute attribute = GameManager.Instance.EnemyFactory.Get(type);
        float intensify = ws.RunningSequence.Intensify;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        HealthBar healthBar = ObjectPool.Instance.Spawn(ws.HealthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify);
        enemy.SpawnOn(PointIndex, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
    }
}

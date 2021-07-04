using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornSkill : Skill
{
    float bornCounter;
    float bornCD;
    int enemyOneBorn;
    WaveSystem ws;
    BoardSystem bs;

    public BornSkill(Enemy enemy,float bornCD,int enemyOneBorn)
    {
        this.enemy = enemy;
        this.bornCD = bornCD;
        this.enemyOneBorn = enemyOneBorn;
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }


    //在enemy的gameupdate中调用
    public override void OnGameUpdating()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD)
        {
            Born();
            bornCounter = 0;
        }
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
        //减半的敌人强度
        float intensify = ws.RunningSequence.Intensify / 2;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        HealthBar healthBar = ObjectPool.Instance.Spawn(ws.HealthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify);
        enemy.Progress = this.enemy.Progress;
        enemy.SpawnOn(this.enemy.PointIndex, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
    }
}

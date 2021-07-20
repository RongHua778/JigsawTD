using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornSkill : Skill
{
    float bornCounter;
    float[] bornCD;
    int[] enemyOneBorn;
    WaveSystem ws;
    BoardSystem bs;
    int level;
    int form;
    float castleCounter;

    public BornSkill(Enemy enemy,float[] bornCD,int[] enemyOneBorn, int form)
    {
        this.enemy = enemy;
        this.bornCD = bornCD;
        this.enemyOneBorn = enemyOneBorn;
        this.form = form;
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }


    //在enemy的gameupdate中调用
    public override void OnGameUpdating()
    {
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD[level])
        {
            Born();
            bornCounter = 0;
        }
        Castle();
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn[level]; i++)
        {
            int typeInt = Random.Range(0, 6);
            SpawnEnemy(bs, (EnemyType)typeInt);
            ws.EnemyRemain++;
        }
    }
    private void SpawnEnemy(BoardSystem board, EnemyType type)
    {
        EnemyAttribute attribute = GameManager.Instance.EnemyFactory.Get(type);
        //生出来的敌人强度下降
        float intensify = ws.LevelSequence[GameManager.Instance.MainUI.CurrentWave][0].Intensify / 8;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        HealthBar healthBar = ObjectPool.Instance.Spawn(ws.HealthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify);
        enemy.Progress = this.enemy.Progress;
        enemy.SpawnOn(this.enemy.PointIndex, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
    }

    private void Castle()
    {
        if (enemy.CurrentHealth / enemy.MaxHealth <= 0.7f && form == 2)
        {
            enemy.DamageIntensify = -0.8f;
            level = 2;
            enemy.StunTime += 6f;
            form = 1;
            castleCounter = 0;
        }
        if (enemy.CurrentHealth / enemy.MaxHealth<= 0.3f&&form==1)
        {
            enemy.DamageIntensify = -0.8f;
            level = 1;
            enemy.StunTime += 6f;
            form = 0;
            castleCounter = 0;
        }
        castleCounter += Time.deltaTime;

        if (castleCounter > 6f)
        {
            enemy.DamageIntensify = 0f;
            level = 0;
        }

    }
}

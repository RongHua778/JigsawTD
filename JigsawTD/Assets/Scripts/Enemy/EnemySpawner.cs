using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float pathOffset = 0.4f;
    [SerializeField] HealthBar healthBarPrefab = default;
    private EnemyFactory _enemyFactory;
    [SerializeField]
    private EnemySequence runningSequence;
    public Queue<EnemySequence> LevelSequence = new Queue<EnemySequence>();
    public void GameUpdate()
    {
        if (runningSequence != null)
        {
            if (!runningSequence.Progress())
            {
                runningSequence = null;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetSequence();
        }
    }

    public void LevelInitialize(EnemyFactory factory)
    {
        this._enemyFactory = factory;
        float intensify = 1;
        float amountIntensify = 1;
        for (int i = 0; i < 30; i++)
        {
            intensify = (10 + Mathf.Pow(i, 1.2f)) / 10;
            amountIntensify = 1 + (float)i / 10;
            EnemySequence sequence = new EnemySequence(_enemyFactory.Get(EnemyType.Restorer), intensify, amountIntensify);
            LevelSequence.Enqueue(sequence);
        }
        GetSequence();
    }

    private void GetSequence()
    {
        runningSequence = LevelSequence.Dequeue();
    }

    public Enemy SpawnEnemy(EnemyAttribute attribute, float intensify)
    {
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab.gameObject).GetComponent<Enemy>();
        HealthBar healthBar = ObjectPool.Instance.Spawn(healthBarPrefab.gameObject).GetComponent<HealthBar>();
        enemy.Initialize(attribute, UnityEngine.Random.Range(-pathOffset, pathOffset), healthBar, intensify);
        return enemy;
    }


}

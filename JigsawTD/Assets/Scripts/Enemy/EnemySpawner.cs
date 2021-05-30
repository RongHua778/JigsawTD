using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float pathOffset = 0.3f;
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


    public void LevelInitialize(EnemyFactory factory)
    {
        this._enemyFactory = factory;
        float intensify = 1;
        int amount;
        for (int i = 0; i < 30; i++)
        {
            EnemyType type = (EnemyType)UnityEngine.Random.Range(0, 4);
            EnemyAttribute attribute = _enemyFactory.Get(type);
            intensify = (1+i)*3;
            amount = attribute.InitCount + i / 5 * attribute.CountIncrease;
            EnemySequence sequence = new EnemySequence(i + 1, attribute, intensify, amount);
            LevelSequence.Enqueue(sequence);
        }
    }

    public void GetSequence()
    {
        if (LevelSequence.Count > 0)
        {
            runningSequence = LevelSequence.Dequeue();
            GameEvents.Instance.StartNewWave(runningSequence);
        }
        else
        {
            Debug.Log("Running Out Of Sequence");
        }

    }

    public Enemy SpawnEnemy(EnemyAttribute attribute, float intensify)
    {
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab.gameObject).GetComponent<Enemy>();
        HealthBar healthBar = ObjectPool.Instance.Spawn(healthBarPrefab.gameObject).GetComponent<HealthBar>();
        enemy.Initialize(attribute, UnityEngine.Random.Range(-pathOffset, pathOffset), healthBar, intensify);
        return enemy;
    }


}

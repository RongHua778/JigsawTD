using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : IGameSystem
{
    public float waveStage = 1f;
    public float waveCoolDown = 2.5f;
    public bool Running = false;
    int enemyRemain = 0;
    public int EnemyRemain
    {
        get => enemyRemain;
        set
        {
            enemyRemain = value;
            if (enemyRemain <= 0)
            {
                enemyRemain = 0;
                GameManager.Instance.PrepareNextWave();
            }
        }
    }
    [SerializeField]
    float pathOffset = 0.3f;
    [SerializeField] HealthBar healthBarPrefab = default;
    private EnemyFactory _enemyFactory;
    [SerializeField]
    private EnemySequence runningSequence;
    public Queue<EnemySequence> LevelSequence = new Queue<EnemySequence>();

    public EnemySequence RunningSequence { get => runningSequence; set => runningSequence = value; }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        this._enemyFactory = gameManager.EnemyFactory;
        LevelInitialize();
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameEvents.Instance.onEnemyDie += EnemyDie;
    }

    public override void Release()
    {
        base.Release();
        GameEvents.Instance.onEnemyReach -= EnemyReach;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
    }

    private void EnemyReach(Enemy enemy)
    {
        EnemyRemain--;
    }

    private void EnemyDie(Enemy enemy)
    {
        EnemyRemain--;
    }

    public override void GameUpdate()
    {
        if (Running)
        {
            if (!RunningSequence.Progress())
            {

                Running = false;
            }
        }
    }


    public void LevelInitialize()
    {
        LevelSequence.Clear();
        int difficulty = Game.Instance.Difficulty;
        float intensify = 1;
        int amount;
        float stage = waveStage;
        for (int i = 0; i < StaticData.Instance.LevelMaxWave; i++)
        {
            EnemyType type = (EnemyType)UnityEngine.Random.Range(0, 4);

            EnemyAttribute attribute = _enemyFactory.Get(type);
            intensify = stage * (0.5f * i + 1);
            switch (difficulty)
            {
                //简单难度
                case 1:
                    if (i % 3 == 0)
                    {
                        if (i < 10) stage += 0.75f;
                        else if (i >= 10 && i < 20) stage += 1.5f;
                        else if (i >= 20) stage += 2.25f;
                    }
                    break;
                //普通难度
                case 2:
                    if (i % 3 == 0)
                    {
                        if (i < 10) stage += 1f;
                        else if (i >= 10 && i < 20) stage += 2.5f;
                        else if (i >= 20 && i < 30) stage += 4f;
                        else if (i >= 30) stage += 5.5f;
                    }
                    break;
                //专家难度
                case 3:
                    if (i % 2 == 0)
                    {
                        if (i < 10) stage += 2f;
                        else if (i >= 10 && i < 20) stage += 3.5f;
                        else if (i >= 20 && i < 30) stage += 5f;
                        else if (i >= 30) stage += 6.5f;
                    }
                    break;
                default:
                    Debug.LogAssertion("难度参数错误");
                    break;
            }

            amount = attribute.InitCount + i / 4 * attribute.CountIncrease;
            float coolDown = attribute.CoolDown;
            coolDown = attribute.CoolDown - i * 0.01f;
            //soldier出来的越来越多
            if (type == EnemyType.Soilder)
            {
                coolDown = coolDown - i / 4 * 0.05f;
            }
            if (i < 4)
            {
                coolDown = waveCoolDown;//2.5
            }
            EnemySequence sequence = new EnemySequence(i + 1, attribute, intensify, amount, coolDown);
            LevelSequence.Enqueue(sequence);
        }
    }


    public void GetSequence()
    {
        if (LevelSequence.Count > 0)
        {
            RunningSequence = LevelSequence.Dequeue();

            //参数设置
            EnemyRemain = RunningSequence.Amount;

        }
        else
        {
            Debug.Log("所有波次都生成完了");
        }

    }

    public void SpawnEnemy(BoardSystem board)
    {
        EnemyAttribute attribute = RunningSequence.EnemyAttribute;
        float intensify = RunningSequence.Intensify;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        HealthBar healthBar = ObjectPool.Instance.Spawn(healthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, UnityEngine.Random.Range(-pathOffset, pathOffset), healthBar, intensify);
        enemy.SpawnOn(0, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
    }


}

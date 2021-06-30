using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : IGameSystem
{
    public float waveStage = 1f;
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

    private List<int[]> BossLevels;

    public EnemySequence RunningSequence { get => runningSequence; set => runningSequence = value; }
    public HealthBar HealthBarPrefab { get => healthBarPrefab; set => healthBarPrefab = value; }

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        this._enemyFactory = gameManager.EnemyFactory;
        BossLevels = new List<int[]>();
        //关卡1的boss关
        BossLevels.Add(new int[] { 9, 16, 22, 29 });
        //关卡2的boss关
        BossLevels.Add(new int[] { 9, 17, 25, 34 });
        //关卡3的boss关
        BossLevels.Add(new int[] { 9, 19, 29, 39 });
        //关卡4的boss关
        BossLevels.Add(new int[] { 9, 19, 29, 39 });
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
        float stage = waveStage;
        EnemySequence sequence=null;
        for (int i = 0; i < StaticData.Instance.LevelMaxWave; i++)
        {
            switch (difficulty)
            {
                case 1:
                    if (i % 3 == 0)
                    {
                        if (i < 8) stage += 0.5f;
                        if (i < 15) stage += 0.75f;
                        else if (i >= 15 && i < 22) stage += 1.25f;
                        else if (i >= 22) stage += 1.75f;
                    }
                    break;
                //简单难度
                case 2:
                    if (i % 3 == 0)
                    {
                        if (i < 10) stage += 0.75f;
                        else if (i >= 10 && i < 20) stage += 1.5f;
                        else if (i >= 20) stage += 2.25f;
                    }
                    break;
                //普通难度
                case 3:
                    if (i % 3 == 0)
                    {
                        if (i < 10) stage += 1f;
                        else if (i >= 10 && i < 20) stage += 2.5f;
                        else if (i >= 20 && i < 30) stage += 4f;
                        else if (i >= 30) stage += 5.5f;
                    }
                    break;
                //专家难度
                case 4:
                    if (i % 2 == 0)
                    {
                        if (i < 10) stage += 2f;
                        else if (i >= 10 && i < 20) stage += 3.5f;
                        else if (i >= 20 && i < 30) stage += 5f;
                        else if (i >= 30) stage += 6.5f;
                    }
                    break;
                default:
                    //Debug.LogAssertion("难度参数错误");
                    sequence = new EnemySequence(_enemyFactory, i + 1, 5f, EnemyType.Blinker);
                    break;
            }
            if (difficulty <5)
            {
                if (i == BossLevels[difficulty - 1][0])
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Divider);
                }
                else if(i == BossLevels[difficulty - 1][1])
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.SixArmor);
                }
                else if (i == BossLevels[difficulty - 1][2])
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Blinker);
                }
                else if (i == BossLevels[difficulty - 1][3])
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Borner);
                }
                else if (i % 7 == 0 && i > 0)
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Random, 2);
                }
                else if (i % 9 == 0 && i > 0)
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Random, 3);
                }
                //前三波难度修正
                else if (i < 3)
                {
                    stage = (i + 1) * 0.5f;
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Random);
                }
                else
                {
                    sequence = new EnemySequence(_enemyFactory, i + 1, stage, EnemyType.Random);
                }
            }

            LevelSequence.Enqueue(sequence);
        }
    }


    public void GetSequence()
    {
        if (LevelSequence.Count > 0)
        {
            RunningSequence = LevelSequence.Dequeue();
            //参数设置
            enemyRemain = runningSequence.index.Count;
        }
        else
        {
            Debug.Log("所有波次都生成完了");
        }

    }

    public void SpawnEnemy(BoardSystem board, int type)
    {
        EnemyAttribute attribute = RunningSequence.EnemyAttribute[type];
        float intensify = RunningSequence.Intensify;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        HealthBar healthBar = ObjectPool.Instance.Spawn(HealthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, UnityEngine.Random.Range(-pathOffset, pathOffset), healthBar, intensify);
        enemy.SpawnOn(0, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
    }


}

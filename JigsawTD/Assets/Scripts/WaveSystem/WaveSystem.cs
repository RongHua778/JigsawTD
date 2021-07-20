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
            if (enemyRemain <= 0 && !Running)
            {
                enemyRemain = 0;
                LevelSequence.RemoveAt(0);
                GameManager.Instance.PrepareNextWave();
            }
        }
    }
    [SerializeField]
    float pathOffset = 0.3f;
    [SerializeField] HealthBar healthBarPrefab = default;
    private EnemyFactory _enemyFactory;

    public List<List<EnemySequence>> LevelSequence = new List<List<EnemySequence>>();

    //private List<int[]> BossLevels;

    LevelAttribute LevelAttribute;
    [SerializeField] private List<EnemySequence> runningSequence;
    public List<EnemySequence> RunningSequence { get => runningSequence; set => runningSequence = value; }
    public HealthBar HealthBarPrefab { get => healthBarPrefab; set => healthBarPrefab = value; }

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        LevelAttribute = LevelManager.Instance.CurrentLevel;
        this._enemyFactory = gameManager.EnemyFactory;
        //BossLevels = new List<int[]>();
        ////关卡1的boss关
        //BossLevels.Add(new int[] { 9, 19, 29, 39 });
        ////关卡2的boss关
        //BossLevels.Add(new int[] { 9, 19, 29, 39 });
        ////关卡3的boss关
        //BossLevels.Add(new int[] { 9, 19, 29, 39 });
        ////关卡4的boss关
        //BossLevels.Add(new int[] { 9, 19, 29, 39 });
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
            for (int i = 0; i < RunningSequence.Count; i++)
            {
                if (!RunningSequence[i].Progress())
                {
                    RunningSequence.Remove(RunningSequence[i]);
                    if (RunningSequence.Count == 0)
                        Running = false;
                }
            }
            //if (!RunningSequence.Progress())
            //{
            //    Running = false;
            //}
        }
    }


    public void LevelInitialize()
    {
        LevelSequence.Clear();
        //int difficulty = Game.Instance.Difficulty;
        float stage = waveStage;
        List<EnemySequence> sequences = null;
        for (int i = 0; i < StaticData.Instance.LevelMaxWave; i++)
        {
            //switch (difficulty)
            //{
            //    case 1:
            //        if (i % 3 == 0)
            //        {
            //            if (i < 8) stage += 0.5f;
            //            if (i < 15) stage += 0.75f;
            //            else if (i >= 15 && i < 22) stage += 1.25f;
            //            else if (i >= 22) stage += 1.75f;
            //        }
            //        break;
            //    //简单难度
            //    case 2:
            //        if (i % 3 == 0)
            //        {
            //            if (i < 10) stage += 0.75f;
            //            else if (i >= 10 && i < 20) stage += 1.5f;
            //            else if (i >= 20) stage += 2.25f;
            //        }
            //        break;
            //    //普通难度
            //    case 3:
            //        if (i % 3 == 0)
            //        {
            //            if (i < 10) stage += 1.5f;
            //            else if (i >= 10 && i < 20) stage += 2.5f;
            //            else if (i >= 20 && i < 30) stage += 4f;
            //            else if (i >= 30) stage += 5f;
            //        }
            //        break;
            //    //专家难度
            //    case 4:
            //        if (i % 2 == 0)
            //        {
            //            stage += ((i / 10) + 1) * LevelAttribute.LevelIntensify;
            //        }
            //        break;
            //    default:
            //        //Debug.LogAssertion("难度参数错误");
            //        break;
            //}
            if (i % 3 == 0)
            {
                stage += ((i / 10) + 1) * LevelAttribute.LevelIntensify;
            }
            //if (difficulty < 5)
            //{

            //前三波难度修正
            if (i < 3)
            {
                stage = (i + 1) * 0.5f;
                sequences = GenerateRandomSequence(1, stage, i);
            }
            else if (i == 9)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.Boss[0].EnemyType, stage, i);
            }
            else if (i == 19)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.Boss[1].EnemyType, stage, i);
            }
            else if (i == 29)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.Boss[2].EnemyType, stage, i);
            }
            else if ((i + 1) % 10 == 0)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.Boss[3].EnemyType, stage, i);
            }
            else if (i % 7 == 0 && i > 0)
            {
                sequences = GenerateRandomSequence(2, stage, i);
            }
            else if (i % 9 == 0 && i > 0)
            {
                sequences = GenerateRandomSequence(3, stage, i);
            }
            else
            {
                sequences = GenerateRandomSequence(1, stage, i);
            }

            // }
            sequences = GenerateSpecificSequence(EnemyType.Divider, 5f, i);

            LevelSequence.Add(sequences);
        }
    }


    private List<EnemySequence> GenerateRandomSequence(int genres, float stage, int wave)
    {
        int maxRandom = wave > 9 ? 6 : 4;
        List<EnemySequence> sequencesToReturn = new List<EnemySequence>();
        List<int> indexs = StaticData.SelectNoRepeat(maxRandom, genres);
        foreach (int index in indexs)
        {
            EnemyType type = LevelAttribute.NormalEnemies[index].EnemyType;
            EnemySequence sequence = SequenceInfoSet(genres, stage, wave, type);
            sequencesToReturn.Add(sequence);
        }
        return sequencesToReturn;
    }

    private List<EnemySequence> GenerateSpecificSequence(EnemyType type, float stage, int wave)
    {
        List<EnemySequence> sequencesToReturn = new List<EnemySequence>();
        EnemySequence sequence = SequenceInfoSet(1, stage, wave, type);
        sequencesToReturn.Add(sequence);
        return sequencesToReturn;
    }

    private EnemySequence SequenceInfoSet(int genres, float stage, int wave, EnemyType type)
    {
        EnemyAttribute attribute = _enemyFactory.Get(type);
        float intensify = stage * (0.5f * wave + 1);
        int amount = Mathf.RoundToInt(attribute.InitCount + ((float)wave / 5) * (float)attribute.CountIncrease / genres);
        float coolDown = (float)(5f + wave / 2) / (float)amount;
        EnemySequence sequence = new EnemySequence(type, amount, coolDown, intensify);
        return sequence;
    }


    public void GetSequence()
    {
        if (LevelSequence.Count > 0)
        {
            RunningSequence = LevelSequence[0];
        }
        else
        {
            Debug.Log("所有波次都生成完了");
        }

    }

    public Enemy SpawnEnemy(BoardSystem board, EnemyAttribute attribute, int pathIndex, float intensify)
    {
        EnemyRemain++;
        //float intensify = RunningSequence.Intensify;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        HealthBar healthBar = ObjectPool.Instance.Spawn(HealthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, UnityEngine.Random.Range(-pathOffset, pathOffset), healthBar, intensify);
        enemy.SpawnOn(pathIndex, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
        return enemy;
    }


}

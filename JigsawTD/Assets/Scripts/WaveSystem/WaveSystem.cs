using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WaveSystem : IGameSystem
{
    [SerializeField] EnemyType TestType = default;
    public bool RunningSpawn = false;//是否正在生产敌人

    public Queue<List<EnemySequence>> LevelSequence = new Queue<List<EnemySequence>>();
    LevelAttribute LevelAttribute;
    [SerializeField] private List<EnemySequence> runningSequence;
    public List<EnemySequence> RunningSequence { get => runningSequence; set => runningSequence = value; }

    [SerializeField] BossComeAnim bossWarningUIAnim = default;

    public override void Initialize()
    {
        LevelAttribute = LevelManager.Instance.CurrentLevel;
        LevelInitialize();
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameEvents.Instance.onEnemyDie += EnemyDie;
        bossWarningUIAnim.Initialize();
    }

    public override void Release()
    {
        base.Release();
        GameEvents.Instance.onEnemyReach -= EnemyReach;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
    }

    private void EnemyReach(Enemy enemy)
    {
        GameRes.EnemyRemain--;
    }

    private void EnemyDie(Enemy enemy)
    {
        GameRes.EnemyRemain--;
    }

    public override void GameUpdate()
    {
        if (RunningSpawn)
        {
            for (int i = 0; i < RunningSequence.Count; i++)
            {
                if (RunningSequence[i].Progress())
                {
                    continue;
                }
                RunningSpawn = false;
            }
        }
    }


    public void LevelInitialize()
    {
        LevelSequence.Clear();
        float stage = 1;
        List<EnemySequence> sequences = null;
        for (int i = 0; i < LevelAttribute.Wave; i++)
        {

            //if (i % 3 == 0)
            //{
            //    stage += (((float)i / 10) + 1) * LevelAttribute.LevelIntensify * (0.5f * i + 1);
            //}
            //if (i > 39)
            //{
            //    float number = 2f;
            //    stage += ((i / 10) + 1) * LevelAttribute.LevelIntensify * number;
            //}
            //stage += (((float)i / 10) + 1) * LevelAttribute.LevelIntensify * (0.5f * i + 1);
            if (i <= 39)
            {
                stage += LevelAttribute.LevelIntensify * (0.05f * Mathf.Pow(i, 1.85f) + 1);
            }
            else
            {
                stage += LevelAttribute.LevelIntensify * (0.04f * Mathf.Pow(i, 2f) + 1);//40波后难度快速成长，期望在100波内解决玩家
            }


            if (i < 3)
            {
                stage = (i + 1) * 0.5f;
                sequences = GenerateRandomSequence(1, stage, i);
            }
            else if (i == 9)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.GetRandomBoss(1).EnemyType, stage, i, true);
            }
            else if (i == 19)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.GetRandomBoss(2).EnemyType, stage, i, true);
            }
            else if (i == 29)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.GetRandomBoss(3).EnemyType, stage, i, true);
            }
            else if ((i + 1) % 10 == 0)
            {
                sequences = GenerateSpecificSequence(LevelAttribute.GetRandomBoss(4).EnemyType, stage, i, true);
            }
            else if ((i + 4) % 10 == 0)
            {
                sequences = GenerateSpecificSequence(EnemyType.GoldKeeper, stage, i);
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

            if (TestType != EnemyType.None)//测试特定敌人用
            {
                sequences = GenerateSpecificSequence(TestType, 50f, i);
            }
            LevelSequence.Enqueue(sequences);
        }
    }


    private List<EnemySequence> GenerateRandomSequence(int genres, float stage, int wave)
    {
        int maxRandom = wave > LevelAttribute.EliteWave ? LevelAttribute.NormalEnemies.Count : 4;
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

    private List<EnemySequence> GenerateSpecificSequence(EnemyType type, float stage, int wave, bool isBoss = false)
    {
        List<EnemySequence> sequencesToReturn = new List<EnemySequence>();
        EnemySequence sequence = SequenceInfoSet(1, stage, wave, type, isBoss);
        sequencesToReturn.Add(sequence);
        return sequencesToReturn;
    }

    private EnemySequence SequenceInfoSet(int genres, float stage, int wave, EnemyType type, bool isBoss = false)
    {
        EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(type);
        int amount = Mathf.RoundToInt(attribute.InitCount + ((float)wave / 5) * attribute.CountIncrease / genres);
        //float coolDown = (float)(5f + wave / 2) / amount;
        float coolDown = attribute.CoolDown * genres;
        EnemySequence sequence = new EnemySequence(type, amount, coolDown, stage, isBoss);
        return sequence;
    }


    public void GetSequence()
    {
        if (LevelSequence.Count > 0)
        {
            RunningSequence = LevelSequence.Dequeue();
            if (RunningSequence[0].IsBoss)
            {
                EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(RunningSequence[0].EnemyType);
                attribute.isLock = false;//见到就解锁BOSS
                bossWarningUIAnim.Show();
            }
        }
        else
        {
            Debug.Log("所有波次都生成完了");
        }

    }

    public Enemy SpawnEnemy(EnemyAttribute attribute, int pathIndex, float intensify)
    {
        GameRes.EnemyRemain++;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        enemy.Initialize(pathIndex, attribute, UnityEngine.Random.Range(-0.3f, 0.3f), intensify);
        GameManager.Instance.enemies.Add(enemy);
        return enemy;
    }


}

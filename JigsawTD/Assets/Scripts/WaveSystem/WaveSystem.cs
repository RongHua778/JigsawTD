using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WaveSystem : IGameSystem
{
    [Header("测试用")]
    [SerializeField] float testIntensify = default;
    [SerializeField] EnemyType TestType = default;
    [Space]
    public bool RunningSpawn = false;//是否正在生产敌人

    public static List<List<EnemySequence>> LevelSequence = new List<List<EnemySequence>>();
    LevelAttribute LevelAttribute;


    private List<EnemySequence> runningSequence;
    public List<EnemySequence> RunningSequence { get => runningSequence; set => runningSequence = value; }

    [SerializeField] BossComeAnim bossWarningUIAnim = default;
    [SerializeField] GoldKeeperAnim goldKeeperUIAnim = default;

    public EnemyType NextBoss;
    public int NextBossWave;

    public override void Initialize()
    {
        LevelAttribute = LevelManager.Instance.CurrentLevel;
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameEvents.Instance.onEnemyDie += EnemyDie;
        bossWarningUIAnim.Initialize();
        goldKeeperUIAnim.Initialize();
    }

    public override void Release()
    {
        base.Release();
        GameEvents.Instance.onEnemyReach -= EnemyReach;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
    }

    private void EnemyReach(Enemy enemy)
    {
        GameRes.Life -= enemy.ReachDamage;//必须先减少LIFE判失败
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

    public void LoadSaveWave()
    {
        LevelSequence.Clear();
        foreach (var saveStruct in LevelManager.Instance.LastGameSave.SaveSequences)
        {
            LevelSequence.Add(saveStruct.SequencesList);
        }
    }
    public void LevelInitialize()
    {
        LevelSequence.Clear();
        float stage = 1;
        List<EnemySequence> sequences = null;
        for (int i = 0; i < LevelAttribute.Wave; i++)
        {

            if (i <= 29)
            {
                stage += LevelAttribute.LevelIntensify * (0.05f * Mathf.Pow(i, 1.8f) + 1);
            }
            else
            {
                stage += LevelAttribute.LevelIntensify * (0.00004f * Mathf.Pow(i, 4f) + 1);//30波后难度快速成长，期望在100波内解决玩家
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
            //else if (i == 39)
            //{
            //    sequences = GenerateSpecificSequence(LevelAttribute.GetRandomBoss(4).EnemyType, stage, i, true);
            //}
            else if (i > 29 && (i + 1) % 5 == 0)//无尽模式30波后每5波一个BOSS
            {
                sequences = GenerateSpecificSequence(LevelAttribute.GetRandomBoss(4).EnemyType, stage, i, true);
            }
            else if ((i + 4) % 10 == 0 && i < 29)//30波前
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
                sequences = GenerateSpecificSequence(TestType, testIntensify, i);
            }
            LevelSequence.Add(sequences);
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

    public void ManualSetSequence(EnemyType type, float stage, int wave)
    {
        LevelSequence[wave] = GenerateSpecificSequence(type, stage, wave);
        PrepareNextWave();
    }

    public void PrepareNextWave()
    {
        RunningSequence = LevelSequence[GameRes.CurrentWave - 1];
        foreach (var sequence in RunningSequence)
        {
            sequence.Initialize();
        }
        if (RunningSequence[0].IsBoss)
        {
            //EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(RunningSequence[0].EnemyType);
            //attribute.isLock = false;//见到就解锁BOSS
            bossWarningUIAnim.Show();
        }
        else if (RunningSequence[0].EnemyType == EnemyType.GoldKeeper)
        {
            goldKeeperUIAnim.Show();
        }
        //下个BOSS预告
        for (int i = GameRes.CurrentWave - 1; i < LevelSequence.Count; i++)
        {
            if (LevelSequence[i][0].IsBoss)
            {
                NextBoss = LevelSequence[i][0].EnemyType;
                NextBossWave = i - GameRes.CurrentWave + 1;
                break;
            }
        }
    }


    //public void GetSequence()
    //{
    //    if (LevelSequence.Count > 0)
    //    {
    //        RunningSequence = LevelSequence.Dequeue();
    //        if (RunningSequence[0].IsBoss)
    //        {
    //            EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(RunningSequence[0].EnemyType);
    //            attribute.isLock = false;//见到就解锁BOSS
    //            bossWarningUIAnim.Show();
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("所有波次都生成完了");
    //    }

    //}



    public Enemy SpawnEnemy(EnemyAttribute attribute, int pathIndex, float intensify)
    {
        GameRes.EnemyRemain++;
        Enemy enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Enemy;
        enemy.Initialize(pathIndex, attribute, UnityEngine.Random.Range(-0.3f, 0.3f), intensify);
        GameManager.Instance.enemies.Add(enemy);
        return enemy;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : IGameSystem
{

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

    [SerializeField] Text waveTxt = default;


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
        LevelInitialize(StaticData.Instance.Difficulty);
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

    public void GameUpdate()
    {
        if (RunningSequence != null)
        {
            if (!RunningSequence.Progress())
            {
                RunningSequence = null;
            }
        }
    }


    public void LevelInitialize( int difficulty)
    {
        float intensify = 1;
        int amount;
        float stage = 100f;
        for (int i = 0; i < StaticData.Instance.LevelMaxWave; i++)
        {
            EnemyType type = (EnemyType)UnityEngine.Random.Range(0, 4);

            EnemyAttribute attribute = _enemyFactory.Get(type);
            intensify = stage * (0.5f * i + 1);
            switch (difficulty)
            {
                //���Ѷ�
                case 1:
                    if (i % 3 == 0)
                    {
                        if (i < 10) stage += 0.75f;
                        else if (i >= 10) stage += 1.5f;
                    }
                    break;
                //��ͨ�Ѷ�
                case 2:
                    if (i % 3 == 0)
                    {
                        if (i < 10) stage += 1.5f;
                        else if (i >= 10 && i < 20) stage += 2f;
                        else if (i >= 20 && i < 30) stage += 3f;
                        else if (i >= 30) stage += 4f;
                    }
                    break;
                //ר���Ѷ�
                case 3:
                    if (i % 2 == 0)
                    {
                        if (i < 10) stage += 1.5f;
                        else if (i >= 10 && i < 20) stage += 2f;
                        else if (i >= 20 && i < 30) stage += 3f;
                        else if (i >= 30) stage += 4f;
                    }
                    break;
                default:
                    Debug.LogAssertion("�ѶȲ�������");
                    break;
            }
            amount = attribute.InitCount + i / 4 * attribute.CountIncrease;
            //ÿ4�غ�
            float coolDown = attribute.CoolDown;
            //TankerԽ��Խ��
            if (type == EnemyType.Tanker)
            {
                intensify = intensify + i * 0.6f * stage;
            }
            coolDown = attribute.CoolDown - i * 0.01f;
            //soldier������Խ��Խ��
            if (type == EnemyType.Soilder)
            {
                coolDown = coolDown - i / 4 * 0.015f;
            }

            //if (i < 4)
            //{
            //    coolDown = 2.5f;
            //    intensify = 1f;
            //}
            EnemySequence sequence = new EnemySequence(i + 1, attribute, intensify, amount, coolDown);
            LevelSequence.Enqueue(sequence);
        }
    }


    public void GetSequence()
    {
        if (LevelSequence.Count > 0)
        {
            RunningSequence = LevelSequence.Dequeue();

            //��������
            EnemyRemain = RunningSequence.Amount;

            //������������
            if (RunningSequence.Wave == StaticData.Instance.LevelMaxWave)
            {
                Sound.Instance.PlayBg(Sound.Instance.LastWaveClip);
            }
            else
            {
                switch (RunningSequence.EnemyAttribute.EnemyType)
                {
                    case EnemyType.Soilder:
                        Sound.Instance.PlayBg(Sound.Instance.SoilderClip);
                        break;
                    case EnemyType.Runner:
                        Sound.Instance.PlayBg(Sound.Instance.RunnerClip);
                        break;
                    case EnemyType.Restorer:
                        Sound.Instance.PlayBg(Sound.Instance.RestorerClip);
                        break;
                    case EnemyType.Tanker:
                        Sound.Instance.PlayBg(Sound.Instance.TankerClip);
                        break;
                }
            }
        }
        else
        {

            Debug.Log("���в��ζ���������");
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

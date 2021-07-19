using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    //int wave;
    public float Intensify;
    public EnemyType EnemyType;

    public int Amount;
    public float AmountIntensify;
    public float CoolDown;
    private float SpawnTimer;
    //public float waveCoolDown = 2.5f;

    //public List<int> index = new List<int>();
    //public bool IsEnd;
    //public int Wave { get => wave; set => wave = value; }

    public EnemySequence(EnemyType type, int amount, float cooldown, float intensify)
    {
        EnemyType = type;
        Amount = amount;
        CoolDown = cooldown;
        Intensify = intensify;
        SpawnTimer = 0;
        //Initiate(wave,enemy,enemiesN);

    }

    //private void Initiate(int wave, EnemyType enemy,int enemiesN=1,int maxRandom=4)
    //{
    //    if (wave > 10)
    //    {
    //        maxRandom = 6;
    //    }
    //    //初始化enemytype
    //    if (enemy!=EnemyType.Random)
    //    {
    //        EnemyAttribute.Add(enemyFactory.Get(enemy));
    //    }
    //    else
    //    {
    //        List<int> types = StaticData.SelectNoRepeat(maxRandom, enemiesN);
    //        for (int i = 0; i < types.Count; i++)
    //        {
    //            EnemyAttribute.Add(enemyFactory.Get((EnemyType)types[i]));
    //        }
    //    }
    //    //初始化敌人的数量以及cooldown
    //    for (int j = 0; j < EnemyAttribute.Count; j++)
    //    {
    //        Amount.Add((EnemyAttribute[j].InitCount + wave / 4 * EnemyAttribute[j].CountIncrease) / EnemyAttribute.Count);
    //        if (wave < 4)
    //        {
    //            CoolDown.Add(waveCoolDown);
    //        }
    //        else
    //        {
    //            CoolDown.Add(EnemyAttribute[j].CoolDown - wave * 0.01f);
    //        }
    //    }
    //    //打乱顺序将敌人按不同类型的编号加入一个列表以便战斗时一个个取出
    //    for (int i = 0; i < Amount.Count; i++)
    //    {
    //        for (int j = 0; j < Amount[i]; j++)
    //        {
    //            index.Add(i);
    //        }
    //    }
    //    index = StaticData.RandomSort(index);
    //    //soldier的cool down进一步降低
    //    if (enemy == EnemyType.Soilder)
    //    {
    //        CoolDown.Add(EnemyAttribute[0].CoolDown - wave * 0.01f - wave / 4 * 0.05f);
    //    }
    //    //敌人强度提升的计算公式
    //    Intensify = Stage * (0.5f * wave + 1);

    //}

    //public void AddEnemy(EnemyType enemyType)
    //{
    //    EnemyAttribute.Add(enemyFactory.Get(enemyType));
    //    int temp = EnemyAttribute.Count - 1;
    //    Amount.Add(EnemyAttribute[temp].InitCount + Wave / 4 * EnemyAttribute[temp].CountIncrease);
    //    CoolDown.Add(EnemyAttribute[temp].CoolDown - Wave * 0.01f);
    //    for (int j = 0; j < Amount[temp]; j++)
    //    {
    //        index.Add(temp);
    //    }

    //    index = StaticData.RandomSort(index);

    //}

    bool first = true;
    float coolDown;
    public bool Progress()
    {
        SpawnTimer += Time.deltaTime;
        if (Amount > 0)
        {
            if (first)//第一只出怪冷却为0
            {
                first = false;
                coolDown = 0;
            }
            else
            {
                coolDown = CoolDown;

            }
            if (SpawnTimer >= coolDown)
            {
                SpawnTimer = 0;
                Amount--;
                GameManager.Instance.SpawnEnemy(EnemyType, 0, Intensify);
            }
        }
        else
        {
            first = true;
            return false;
        }
        return true;
        //if (index.Count > 0)
        //{
        //    float coolDown;
        //    if (first)//第一只出怪冷却为0
        //    {
        //        first = false;
        //        coolDown = 0;
        //    }
        //    else
        //    {
        //        coolDown = CoolDown;

        //    }
        //    if (SpawnTimer >= coolDown)
        //    {
        //        SpawnTimer = 0;
        //        index.RemoveAt(0);
        //        GameManager.Instance.SpawnEnemy(EnemyAttribute.EnemyType,0);
        //    }
        //}

    }
}

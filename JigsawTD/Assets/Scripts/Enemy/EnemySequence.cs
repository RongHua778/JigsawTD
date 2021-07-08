using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    public int Wave;
    public float Stage=1f;
    public List<EnemyAttribute> EnemyAttribute;
    public List<int> Amount;
    public float Intensify;
    public List<float> CoolDown;
    private float SpawnTimer;
    public float waveCoolDown = 2.5f;

    public List<int> index=new List<int>();
    private EnemyFactory enemyFactory;
    public EnemySequence(EnemyFactory enemyFactory,int wave,float stage,EnemyType enemy,int enemiesN=1)
    {
        EnemyAttribute = new List<EnemyAttribute>();
        Amount = new List<int>();
        CoolDown = new List<float>();
        Wave = wave;
        Stage = stage;
        SpawnTimer = 0;
        this.enemyFactory = enemyFactory;
        Initiate(wave,enemy,enemiesN);
    }

    private void Initiate(int wave, EnemyType enemy,int enemiesN=1,int maxRandom=4)
    {
        if (wave > 10)
        {
            maxRandom = 4;
        }
        //初始化enemytype
        if (enemy!=EnemyType.Random)
        {
            EnemyAttribute.Add(enemyFactory.Get(enemy));
        }
        else
        {
            List<int> types = StaticData.SelectNoRepeat(maxRandom, enemiesN);
            for (int i = 0; i < types.Count; i++)
            {
                EnemyAttribute.Add(enemyFactory.Get((EnemyType)types[i]));
            }
        }
        //初始化敌人的数量以及cooldown
        for (int j = 0; j < EnemyAttribute.Count; j++)
        {
            Amount.Add((EnemyAttribute[j].InitCount + wave / 4 * EnemyAttribute[j].CountIncrease) / EnemyAttribute.Count + 1);
            if (wave < 4)
            {
                CoolDown.Add(waveCoolDown);
            }
            else
            {
                CoolDown.Add(EnemyAttribute[j].CoolDown - wave * 0.01f);
            }
        }
        //打乱顺序将敌人按不同类型的编号加入一个列表以便战斗时一个个取出
        for (int i = 0; i < Amount.Count; i++)
        {
            for (int j = 0; j < Amount[i]; j++)
            {
                index.Add(i);
            }
        }
        index = StaticData.RandomSort(index);
        //soldier的cool down进一步降低
        if (enemy == EnemyType.Soilder)
        {
            CoolDown.Add(EnemyAttribute[0].CoolDown - wave * 0.01f - wave / 4 * 0.05f);
        }
        //敌人强度提升的计算公式
        Intensify = Stage * (0.5f * wave + 1);

    }

    public bool Progress()
    {
    SpawnTimer += Time.deltaTime;
        if (index.Count > 0)
        {
            int type = index[0];
            if (SpawnTimer >= CoolDown[type])
            {
                SpawnTimer = 0;
                index.RemoveAt(0);
                GameManager.Instance.SpawnEnemy(this, type);
            }
        }
        else
        {
            return false;
        }
        return true;
    }
}

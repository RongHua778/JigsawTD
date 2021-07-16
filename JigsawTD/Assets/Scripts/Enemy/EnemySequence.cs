using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    int wave;
    public float Stage=1f;
    public List<EnemyAttribute> EnemyAttribute;
    public List<int> Amount;
    public float AmountIntensify;
    public float Intensify;
    public List<float> CoolDown;
    private float SpawnTimer;
    public float waveCoolDown = 2.5f;

    public List<int> index=new List<int>();
    private EnemyFactory enemyFactory;
    public bool IsEnd;

    public int Wave { get => wave; set => wave = value; }

    public EnemySequence(EnemyFactory enemyFactory,int wave,float stage,EnemyType enemy,int enemiesN=1)
    {
        EnemyAttribute = new List<EnemyAttribute>();
        Amount = new List<int>();
        CoolDown = new List<float>();
        this.Wave = wave;
        Stage = stage;
        SpawnTimer = 0;
        this.enemyFactory = enemyFactory;
        Initiate(wave,enemy,enemiesN);

    }

    private void Initiate(int wave, EnemyType enemy,int enemiesN=1,int maxRandom=4)
    {
        if (wave > 10)
        {
            maxRandom = 6;
        }
        //��ʼ��enemytype
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
        //��ʼ�����˵������Լ�cooldown
        for (int j = 0; j < EnemyAttribute.Count; j++)
        {
            Amount.Add((EnemyAttribute[j].InitCount + wave / 4 * EnemyAttribute[j].CountIncrease) / EnemyAttribute.Count);
            if (wave < 4)
            {
                CoolDown.Add(waveCoolDown);
            }
            else
            {
                CoolDown.Add(EnemyAttribute[j].CoolDown - wave * 0.01f);
            }
        }
        //����˳�򽫵��˰���ͬ���͵ı�ż���һ���б��Ա�ս��ʱһ����ȡ��
        for (int i = 0; i < Amount.Count; i++)
        {
            for (int j = 0; j < Amount[i]; j++)
            {
                index.Add(i);
            }
        }
        index = StaticData.RandomSort(index);
        //soldier��cool down��һ������
        if (enemy == EnemyType.Soilder)
        {
            CoolDown.Add(EnemyAttribute[0].CoolDown - wave * 0.01f - wave / 4 * 0.05f);
        }
        //����ǿ�������ļ��㹫ʽ
        Intensify = Stage * (0.5f * wave + 1);

    }

    public void AddEnemy(EnemyType enemyType)
    {
        EnemyAttribute.Add(enemyFactory.Get(enemyType));
        int temp = EnemyAttribute.Count - 1;
        Amount.Add(EnemyAttribute[temp].InitCount + Wave / 4 * EnemyAttribute[temp].CountIncrease);
        CoolDown.Add(EnemyAttribute[temp].CoolDown - Wave * 0.01f);
        for (int j = 0; j < Amount[temp]; j++)
        {
            index.Add(temp);
        }

        index = StaticData.RandomSort(index);

    }

    bool first = true;
    public bool Progress()
    {
        SpawnTimer += Time.deltaTime;
        if (index.Count > 0)
        {
            int typeIndex = index[0];
            float coolDown;
            if (first)//��һֻ������ȴΪ0
            {
                first = false;
                coolDown = 0;
            }
            else
            {
                coolDown = CoolDown[typeIndex];

            }
            if (SpawnTimer >= coolDown)
            {
                SpawnTimer = 0;
                index.RemoveAt(0);
                GameManager.Instance.SpawnEnemy(EnemyAttribute[typeIndex].EnemyType,0);
            }
        }
        else
        {
            first = true;
            return false;
        }
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    public int Wave;
    public List<EnemyAttribute> EnemyAttribute;
    public List<int> Amount;
    public float AmountIntensify;
    public float Intensify;
    public List<float> CoolDown;
    private float SpawnTimer;

    public List<int> index=new List<int>();
    public EnemySequence(int wave, List<EnemyAttribute> attribute, float intensify,List<int> amount,List<float> coolDown)
    {
        this.Wave = wave;
        this.EnemyAttribute = attribute;
        this.Amount = amount;
        this.Intensify = intensify;
        this.CoolDown = coolDown;
        this.SpawnTimer = 0;
        for(int i = 0; i < amount.Count; i++)
        {
            for (int j = 0; j < amount[i]; j++)
            {
                index.Add(i);
            }
        }
        index = StaticData.RandomSort<int>(index);
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
                GameManager.Instance.SpawnEnemy(type);
            }
        }
        else
        {
            return false;
        }
        return true;
    }
}

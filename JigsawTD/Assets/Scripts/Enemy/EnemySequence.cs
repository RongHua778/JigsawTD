using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    public int Wave;
    public EnemyAttribute EnemyAttribute;
    public int Amount;
    public float AmountIntensify;
    public float Intensify;
    public float CoolDown;
    private float SpawnTimer;
    private int SpawnCounter;
    public EnemySequence(int wave, EnemyAttribute attribute, float intensify,int amount,float coolDown)
    {
        this.Wave = wave;
        this.EnemyAttribute = attribute;
        this.Amount = amount;
        this.Intensify = intensify;
        this.CoolDown = coolDown;
        this.SpawnTimer = 0;
        this.SpawnCounter = 0;
    }

    public bool Progress()
    {
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer >= CoolDown)
        {
            SpawnTimer = 0;
            SpawnCounter += 1;
            GameManager.Instance.SpawnEnemy();
            if (SpawnCounter >= Amount)
            {
                return false;
            }
        }
        return true;
    }
}

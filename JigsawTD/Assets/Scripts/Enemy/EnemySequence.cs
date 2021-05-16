using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    public EnemyAttribute EnemyAttribute;
    public int Amount;
    public float AmountIntensify;
    public float Intensify;
    public float CoolDown;
    private float SpawnTimer;
    private int SpawnCounter;
    public EnemySequence(EnemyAttribute attribute, float intensify,float amountIntensify)
    {
        this.EnemyAttribute = attribute;
        this.AmountIntensify = amountIntensify;
        this.Amount = Mathf.RoundToInt(attribute.CountPerWave * amountIntensify);
        this.Intensify = intensify;
        this.CoolDown = attribute.CoolDown;
        this.SpawnTimer = 0;
        this.SpawnCounter = 0;
    }

    public bool Progress()
    {
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer >= CoolDown)
        {
            SpawnTimer = 0;
            if (SpawnCounter >= Amount)
            {
                return false;
            }
            SpawnCounter += 1;
            GameManager.Instance.SpawnEnemy(this);
        }
        return true;
    }
}

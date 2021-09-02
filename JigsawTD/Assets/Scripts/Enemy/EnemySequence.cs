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

    }



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


    }
}

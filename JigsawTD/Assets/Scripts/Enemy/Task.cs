using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : ReusableObject
{
    TaskDifficulty difficulty;
    EnemyType enemyType;
    int periods;
    int periodsPassed;
    int waitingPeriods;
    WaveSystem ws;
    int rewardMoney;
    bool actived=false;
    bool taskComplete=false;
    public bool inPocket = false;

    public bool Actived { get => actived; set => actived = value; }
    public bool TaskComplete { get => taskComplete; set => taskComplete = value; }

    public void Initiate(WaveSystem ws,TaskDifficulty difficulty, int periods, EnemyType enemyType,int rewardMoney)
    {
        this.ws = ws;
        this.difficulty = difficulty;
        this.enemyType = enemyType;
        this.periods = periods;
        this.rewardMoney = rewardMoney;
        waitingPeriods= 4;
        actived = false;
        taskComplete = false;
        inPocket = false;
        periodsPassed = 0;
    }

    public void PlayTask()
    {
        Actived = true;
        for (int i = 0; i < periods; i++)
        {
        ws.LevelSequence[i].AddEnemy(enemyType);
        }
    }
    public void CountTask()
    {
        if (Actived)
        {
            periodsPassed += 1;
            if (periodsPassed >= periods)
            {
                GameManager.Instance.MainUI.Coin += rewardMoney;
                TaskComplete = true;
            }
        }
        else
        {
            waitingPeriods -= 1;
            if (waitingPeriods <= 0)
            {
                TaskComplete = true;
            }
        }

    }

    public string GetInfo()
    {
        string result;
        if (Actived)
        {
            result = "难度:" + difficulty.ToString() + "\n" +
    "敌人类型:" + enemyType.ToString() + "\n" +
    "完成进度:" + periodsPassed.ToString() + "/" + periods.ToString();
        }
        else
        {
            result = "难度:" + difficulty.ToString() + "\n" +
    "敌人类型:" + enemyType.ToString() + "\n" +
    "需要回合数:" + periods.ToString();
        }
        return result;
    }

    public void Reclaim(List<Task> tasks)
    {
        tasks.Remove(this);
        ObjectPool.Instance.UnSpawn(this);
    }

    public void AddTo(List<Task> tasks)
    {
        tasks.Add(this);
        inPocket = true;
        transform.localPosition = Vector3.one * 99999999;
    }
}

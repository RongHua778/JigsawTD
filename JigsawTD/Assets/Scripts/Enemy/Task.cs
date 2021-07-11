using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    TaskDifficulty difficulty;
    EnemyType enemyType;
    int periods;
    int waitingPeriods;
    WaveSystem ws;
    int rewardMoney;
    bool actived=false;
    public Task(WaveSystem ws,TaskDifficulty difficulty, int periods, EnemyType enemyType)
    {
        this.ws = ws;
        this.difficulty = difficulty;
        this.enemyType = enemyType;
        this.periods = periods;
        waitingPeriods= 4;
    }

    public void PlayTask()
    {
        actived = true;
        for (int i = 0; i < periods; i++)
        {
        ws.LevelSequence[i].AddEnemy(enemyType);
        }
    }
    public void CountTask()
    {
        if (actived)
        {
            periods -= 1;
            if (periods <= 0)
            {
                GameManager.Instance.MainUI.Coin += rewardMoney;
            }
        }
        else
        {
            waitingPeriods -= 1;
            if (waitingPeriods <= 0)
            {
                ReclaimTask();
            }
        }

    }
    public void ReclaimTask()
    {

    }
}

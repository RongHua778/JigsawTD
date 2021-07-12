using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


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
    TaskTips tasktips;
    Sprite icon;

    public bool Actived { get => actived; set => actived = value; }
    public bool TaskComplete { get => taskComplete; set => taskComplete = value; }
    public int Periods { get => periods; set => periods = value; }
    public TaskDifficulty Difficulty { get => difficulty; set => difficulty = value; }
    public Sprite Icon { get => icon; set => icon = value; }

    private void Awake()
    {
        tasktips = GameObject.Find("TaskTips").GetComponent<TaskTips>();
    }

    public void Initiate(WaveSystem ws, TaskDifficulty difficulty,
        int periods, EnemyType enemyType, int rewardMoney,
        Sprite icon)
    {
        this.ws = ws;
        this.Difficulty = difficulty;
        this.enemyType = enemyType;
        this.Periods = periods;
        this.rewardMoney = rewardMoney;
        waitingPeriods= 0;
        actived = false;
        taskComplete = false;
        inPocket = false;
        periodsPassed = 0;
        this.icon = icon;
    }

    public void PlayTask()
    {
        Actived = true;
        for (int i = 0; i < Periods; i++)
        {
        ws.LevelSequence[i].AddEnemy(enemyType);
        }
    }
    public void CountTask()
    {
        if (Actived)
        {
            periodsPassed += 1;
            if (periodsPassed >= Periods)
            {
                GameManager.Instance.MainUI.Coin += rewardMoney;
                TaskComplete = true;
            }
        }
    }
    public void CountDisappear()
    {
        waitingPeriods++;
        if (waitingPeriods >= 3)
        {
            ObjectPool.Instance.UnSpawn(this);
        }
    }
    public string GetInfo()
    {
        string result;
        if (Actived)
        {
            result = "难度:" + Difficulty.ToString() + "\n" +
    "敌人类型:" + enemyType.ToString() + "\n" +
    "完成进度:" + periodsPassed.ToString() + "/" + Periods.ToString();
        }
        else
        {
            result = "难度:" + Difficulty.ToString() + "\n" +
    "敌人类型:" + enemyType.ToString() + "\n" +
    "需要回合数:" + Periods.ToString();
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

    public void Replace(List<Task> tasks,int i)
    {
        if (tasks.Count <= i)
        {
            tasks.Add(this);
        }
        else
        {
            tasks[i] = this;
        }

        inPocket = true;
        transform.localPosition = Vector3.one * 99999999;
    }

    public void OnMouseEnter()
    {
        tasktips.SetInfo(this);
        tasktips.Show();
    }

    public void OnMouseExit()
    {
        tasktips.Hide();
    }

    public void OnMouseDown()
    {
        GameManager.Instance.MainUI.clickedTask = this;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        waitingPeriods = 0;
        actived = false;
        taskComplete = false;
        inPocket = false;
        periodsPassed = 0;
    }
}

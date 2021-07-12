using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public enum TaskDifficulty
{
    Easy,Medium,Difficult
}

[CreateAssetMenu(menuName = "Factory/TaskFactory", fileName = "taskFactory")]
public class TaskFactory : GameObjectFactory
{
    [SerializeField] Task taskPrefab;
    [SerializeField] Sprite[] icons;
    WaveSystem ws;
    public void InitializeFactory()
    {
        ws = GameManager.Instance.WaveSystem;
    }
    public Task GetTask(TaskDifficulty difficulty,int periods,EnemyType enemyType,int rewardMoney)
    {
        Task task = CreateInstance(taskPrefab) as Task;
        task.Initiate(ws, difficulty, periods, enemyType, rewardMoney, icons[0]);
        return task;
    }

    public Task GetRandomTask()
    {
        int temp = Random.Range(0,6);
        Task task = CreateInstance(taskPrefab) as Task;
        switch (temp)
        {
            case 0:
                task.Initiate(ws, TaskDifficulty.Easy, 3, EnemyType.Tanker, 100, icons[0]);
                break;
            case 1:
                task.Initiate(ws, TaskDifficulty.Easy, 3, EnemyType.Runner, 100, icons[1]);
                break;
            case 2:
                task.Initiate(ws, TaskDifficulty.Medium, 2, EnemyType.Froster, 175, icons[2]);
                break;
            case 3:
                task.Initiate(ws, TaskDifficulty.Medium, 2, EnemyType.Healer, 175, icons[3]);
                break;
            case 4:
                task.Initiate(ws, TaskDifficulty.Difficult, 1, EnemyType.Divider, 250, icons[4]);
                break;
            case 5:
                task.Initiate(ws, TaskDifficulty.Difficult, 1, EnemyType.SixArmor, 250, icons[5]);
                break;
        }
        return task;
    }
}

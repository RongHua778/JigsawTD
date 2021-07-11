using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskDifficulty
{
    Easy,Medium,Difficult
}

[CreateAssetMenu(menuName = "Factory/TaskFactory", fileName = "taskFactory")]
public class TaskFactory : GameObjectFactory
{
    [SerializeField] Task taskPrefab;
    WaveSystem ws;
    public void InitializeFactory()
    {
        ws = GameManager.Instance.WaveSystem;
    }
    public Task GetTask(TaskDifficulty difficulty,int periods,EnemyType enemyType,int rewardMoney)
    {
        Task task = CreateInstance(taskPrefab) as Task;
        task.Initiate(ws, difficulty, periods, enemyType, rewardMoney);
        return task;
    }

    public Task GetRandomTask()
    {
        int temp = Random.Range(0,6);
        Task task = CreateInstance(taskPrefab) as Task;
        switch (temp)
        {
            case 0:
                task.Initiate(ws, TaskDifficulty.Easy, 3, EnemyType.Tanker, 100);
                break;
            case 1:
                task.Initiate(ws, TaskDifficulty.Easy, 3, EnemyType.Runner, 100);
                break;
            case 2:
                task.Initiate(ws, TaskDifficulty.Medium, 2, EnemyType.Froster, 175);
                break;
            case 3:
                task.Initiate(ws, TaskDifficulty.Medium, 2, EnemyType.Healer, 175);
                break;
            case 4:
                task.Initiate(ws, TaskDifficulty.Difficult, 1, EnemyType.Divider, 250);
                break;
            case 5:
                task.Initiate(ws, TaskDifficulty.Difficult, 1, EnemyType.SixArmor, 250);
                break;
        }
        return task;
    }
}

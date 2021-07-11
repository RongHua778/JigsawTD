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
    WaveSystem ws;
    public void InitializeFactory()
    {
        ws = GameManager.Instance.WaveSystem;
    }
    public Task GetTask(TaskDifficulty difficulty,int periods,EnemyType enemyType)
    {
        Task task = new Task(ws,difficulty, periods, enemyType);
        return task;
    }

    public Task GetRandomTask()
    {
        int temp = Random.Range(0,6);
        Task task = new Task(ws, TaskDifficulty.Easy, 3, EnemyType.Tanker);
        switch (temp)
        {
            case 0:
                task = new Task(ws, TaskDifficulty.Easy, 3, EnemyType.Tanker);
                break;
            case 1:
                task = new Task(ws, TaskDifficulty.Easy, 3, EnemyType.Runner);
                break;
            case 2:
                task = new Task(ws, TaskDifficulty.Medium, 2, EnemyType.Froster);
                break;
            case 3:
                task = new Task(ws, TaskDifficulty.Medium, 2, EnemyType.Healer);
                break;
            case 4:
                task = new Task(ws, TaskDifficulty.Difficult, 1, EnemyType.Divider);
                break;
            case 5:
                task = new Task(ws, TaskDifficulty.Difficult, 1, EnemyType.SixArmor);
                break;
        }
        return task;
    }
}

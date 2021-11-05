using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/LevelAttribute", fileName = "LevelAttribute")]
public class LevelAttribute : ScriptableObject
{
    public int PlayerHealth;
    public int Wave;
    public float LevelIntensify;
    public List<EnemyAttribute> NormalEnemies;
    public List<EnemyAttribute> Boss1;
    public List<EnemyAttribute> Boss2;
    public List<EnemyAttribute> Boss3;
    public List<EnemyAttribute> Boss4;
    public string LevelInfo;
    public int TrapCount;
    public int EliteWave;

    public string[] Bonus;

    public EnemyAttribute GetRandomBoss(int level)
    {
        switch (level)
        {
            case 1:
                return Boss1[Random.Range(0, Boss1.Count)];
            case 2:
                return Boss2[Random.Range(0, Boss2.Count)];
            case 3:
                return Boss3[Random.Range(0, Boss3.Count)];
            case 4:
                return Boss4[Random.Range(0, Boss4.Count)];
        }
        Debug.LogWarning("没有可以返回的BOSS类型");
        return null;
    }
}

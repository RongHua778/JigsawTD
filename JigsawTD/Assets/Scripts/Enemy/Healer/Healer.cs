using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Enemy
{
    public override EnemyType EnemyType => EnemyType.Healer;
    [SerializeField]
    EnemyDetector detector;
    public float speedUp;
    public override void OnUnSpawn()
    {
        for (int i = 0; i < detector.Enemies.Count; i++)
        {
            detector.Enemies[i].Speed -= speedUp;
        }
    }

}

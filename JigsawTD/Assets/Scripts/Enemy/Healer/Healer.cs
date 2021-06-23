using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Enemy
{
    public override EnemyType EnemyType => EnemyType.Healer;
    [SerializeField]
    EnemyDetector detector;
    public float speedUp;
    public override bool GameUpdate()
    {
        foreach(Enemy e in detector.Enemies)
        {
            e.Speed = e.initialSpeed + speedUp;
        }
        return base.GameUpdate();
    }
}

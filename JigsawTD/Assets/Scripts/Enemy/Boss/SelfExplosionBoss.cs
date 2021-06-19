using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplosionBoss : Enemy
{
    [SerializeField]
    EnemyDetector detector;
    public float freezeTime;
    public override EnemyType EnemyType => EnemyType.SelfExplosion;
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnSpawn()
    {
        foreach(TurretContent t in detector.Turrets)
        {
            t.InActivate(freezeTime);
        }
        base.OnUnSpawn();
    }
}

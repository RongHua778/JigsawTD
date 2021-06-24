using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplosionBoss : Enemy
{
    [SerializeField]
    TurretDetector detector;
    public float freezeTime;
    public override EnemyType EnemyType => EnemyType.BossSelfExplosion;
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override bool GameUpdate()
    {
        if (IsDie)
        {
            foreach (TurretContent t in detector.Turrets)
            {
                t.InActivate(freezeTime);
            }
        }
        return base.GameUpdate();
    }
}

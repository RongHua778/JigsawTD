using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green_Runner : Enemy
{
    public override EnemyType EnemyType => EnemyType.Runner;

    float speedIntensify = 0;
    public float SpeedIntensify { get => speedIntensify; set => speedIntensify = Mathf.Min(2, value); }
    public override float Speed { get => StunTime > 0 ? 0 : (speed + SpeedIntensify) * (1 - SlowRate / (SlowRate + 1)); set => base.Speed = value; }


    public override DirectionChange DirectionChange
    {
        get => base.DirectionChange;
        set
        {
            base.DirectionChange = value;
            if (value != DirectionChange.None)
            {
                SpeedIntensify = 0f;
            }
        }
    }
    public override bool GameUpdate()
    {
        SpeedIntensify += 0.5f * Time.deltaTime;
        return base.GameUpdate();
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        SpeedIntensify = 0;
    }
}

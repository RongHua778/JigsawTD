using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    public override float SpeedIntensify { get => base.SpeedIntensify + speedIncreased; set => base.SpeedIntensify = value; }

    private float speedIncreased = 0;
    public override DirectionChange DirectionChange
    {
        get => base.DirectionChange;
        set
        {
            base.DirectionChange = value;
            if (value != DirectionChange.None)
            {
                speedIncreased = 0f;
            }
            else
            {
                speedIncreased = 2f;
            }
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        speedIncreased = 0;
    }
}

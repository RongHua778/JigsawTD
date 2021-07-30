using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMarkTrap : TrapContent
{
    int trapIndex;

    public override void OnContentPassMoreThanOnce(Enemy enemy)
    {
        trapIndex = enemy.PointIndex;
    }

    public override void OnGameUpdating(Enemy enemy)
    {
        if (trapIndex < enemy.PointIndex)
        {
            enemy.DamageIntensify += (enemy.PointIndex - trapIndex) * 0.05f * trapIntensify2;
            trapIndex = enemy.PointIndex;
        }
    }
}

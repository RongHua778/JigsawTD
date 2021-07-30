using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrap : TrapContent
{
    public override void OnContentPassMoreThanOnce(Enemy enemy)
    {
        enemy.StunTime += enemy.PassedTraps.Count * 0.5f* trapIntensify2;
    }

}
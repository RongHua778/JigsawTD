using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        enemy.StunTime += enemy.PassedTraps.Count * 0.2f * TrapIntensify * enemy.TrapIntentify;
        enemy.TrapIntentify = 0;
    }

}
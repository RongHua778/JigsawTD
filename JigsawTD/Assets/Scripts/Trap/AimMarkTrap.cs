using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMarkTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        enemy.DamageIntensify += 0.1f*TrapIntensify*enemy.TrapIntentify;
        enemy.TrapIntentify = 1;
    }
}

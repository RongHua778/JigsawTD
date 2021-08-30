using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy)
    {
        if (enemy.PassedTraps.Contains(this))
            return;
        base.OnContentPass(enemy);
        enemy.Flash(Mathf.RoundToInt(2 * TrapIntensify * enemy.TrapIntentify));
        enemy.TrapIntentify = 1;
    }

}

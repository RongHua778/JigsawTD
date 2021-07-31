using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitatingTrap : TrapContent
{
    TrapContent previousTrap=null;
    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
        if (!previousTrap)
        {
            previousTrap = enemy.PassedTraps[enemy.PassedTraps.Count - 2];
        }
        previousTrap.OnContentPass(enemy);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteTrap : TrapContent
{
    public override void NextTrap(TrapContent nextTrap)
    {
        base.NextTrap(nextTrap);
        nextTrap.trapIntensify2 += trapIntensify2;
    }

    public override void OnContentPassMoreThanOnce(Enemy enemy)
    {
        for (int i = 0; i < enemy.PassedTraps.Count - 1; i++)
        {
            if(enemy.PassedTraps[i].trap == this)
            {
                enemy.PassedTraps.RemoveAt(i);
            }
        }
        enemy.PassedTraps.Add(new EnemyTrapManager(this ,false));
    }

}
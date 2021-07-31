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

    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
    }

}
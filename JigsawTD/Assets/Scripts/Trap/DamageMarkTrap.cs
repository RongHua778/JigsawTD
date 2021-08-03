using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMarkTrap : TrapContent
{

    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
    }
    public override void OnGameUpdating(Enemy enemy)
    {
        base.OnGameUpdating(enemy);
        if (trapCounter < 5f)
        {
            trapCounter += Time.deltaTime;
        }
        else
        {
            enemy.DamageIntensify += 0.5f * enemy.TrapIntentify;
        }
    }


}

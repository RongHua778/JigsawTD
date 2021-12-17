using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy);
        float stunTime = enemy.PassedTraps.Count * 0.5f * TrapIntensify * enemy.DamageStrategy.TrapIntensify;
        enemy.DamageStrategy.StunTime += stunTime;
        enemy.DamageStrategy.TrapIntensify = 1;
    }

}
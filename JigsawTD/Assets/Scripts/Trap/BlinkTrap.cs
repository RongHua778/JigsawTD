using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTrap : TrapContent
{
    private int Distance;

    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        if (content == null)
            content = this;
        if (enemy.PassedTraps.Contains((TrapContent)content))
            return;
        base.OnContentPass(enemy, content);
        Distance = Mathf.RoundToInt(2 * TrapIntensify * enemy.DamageStrategy.TrapIntensify);
        enemy.Flash(Distance);
        enemy.DamageStrategy.TrapIntensify = 1;
    }

}

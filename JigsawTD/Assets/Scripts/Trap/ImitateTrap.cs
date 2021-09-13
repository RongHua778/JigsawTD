using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitateTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        if (enemy.PassedTraps.Count > 0)
        {
            TrapContent trap = enemy.PassedTraps[enemy.PassedTraps.Count - 1];
            if (trap != this)
                trap.OnContentPass(enemy, this);
        }
    }
}

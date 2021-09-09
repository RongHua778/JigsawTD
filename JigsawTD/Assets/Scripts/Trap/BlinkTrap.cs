using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTrap : TrapContent
{
    private int Distance;
    public override void OnContentPreCheck(int index, List<BasicTile> path)
    {
        base.OnContentPreCheck(index, path);
        Distance = 2 + Mathf.FloorToInt(index / 15 * m_GameTile.TrapIntensify);
    }
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        if (content == null)
            content = this;
        if (enemy.PassedTraps.Contains((TrapContent)content))
            return;
        base.OnContentPass(enemy, content);
        enemy.Flash(Distance);
    }

}
